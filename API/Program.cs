using API.Hubs;
using API.Middlewares;
using Application;
using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);
var conf = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Vui lòng nhập JWT với tiền tố Bearer vào ô dưới đây.\n\nVí dụ: \"Bearer eyJhbGciOiJI...\"",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddApplicationServices(typeof(Program).Assembly, typeof(AppDbContext).Assembly);
builder.Services.AddInfrastructureServices(conf);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowedFrontend", policy =>
    {
        policy.WithOrigins("https://tcapscompany.com", "http://localhost:8081")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

builder.Services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(conf["JwtSettings:SecretKey"])),

            ValidateIssuer = true,
            ValidIssuer = conf["JwtSettings:Issuer"],

            ValidateAudience = true,
            ValidAudience = conf["JwtSettings:Audience"],

            ValidateLifetime = true,

            ClockSkew = TimeSpan.Zero,

            // Map the 'sub' claim to ClaimTypes.NameIdentifier
            NameClaimType = JwtRegisteredClaimNames.Sub
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                // Nếu request đến đường dẫn Hubs thì lấy token từ query string
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("Lead", policy =>
        policy.RequireRole("Lead"));

    options.AddPolicy("QC", policy =>
        policy.RequireRole("QC"));

    options.AddPolicy("GuardQC", policy =>
        policy.RequireRole("GuardQC"));

    options.AddPolicy("QCK", policy =>
        policy.RequireClaim("QCK"));

    options.AddPolicy("QCTransportOnly", policy =>
    {
        policy.RequireRole("QCTransport");
    });

    options.AddPolicy("LeadOrValidQCTransport", policy =>
    {
        policy.RequireAssertion(context =>
        {
            var role = context.User.FindFirstValue(ClaimTypes.Role);

            // Lead luôn được phép
            if (role == "Lead")
                return true;

            // QCTransport phải có claim isQcTransport=true
            if (role == "QCTransport" &&
                context.User.HasClaim("isQcTransport", "true"))
                return true;

            return false;
        });
    });

    options.AddPolicy("CanCreateMaterialRequest", policy =>
        policy.RequireRole("Lead", "QC"));

    options.AddPolicy("CanViewDashboard", policy =>
        policy.RequireRole("Admin", "Lead"));
});

builder.Services.Configure<IdentityOptions>(options =>
{
    options.ClaimsIdentity.UserIdClaimType = JwtRegisteredClaimNames.Sub;
});

builder.Services.AddSignalR();
builder.Services.AddSingleton<IUserIdProvider, Infrastructure.Hubs.CustomUserIdProvider>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var redisConnection = builder.Configuration.GetConnectionString("RedisConnection");

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        await context.HttpContext.Response.WriteAsJsonAsync(new
        {
            message = "Bạn thao tác quá nhanh. Vui lòng thử lại sau vài phút."
        }, token);
    };

    options.AddPolicy("OtpPolicy", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 3,
                Window = TimeSpan.FromMinutes(10),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0
            }));
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnection;
    options.InstanceName = "Tcaps_";
});

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor |
                       Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        var path = ctx.Context.Request.Path.Value?.ToLower() ?? "";
        if (path.Contains("/images/products/") ||
            path.Contains("/images/batches/") ||
            path.Contains("/images/inventories/") ||
            path.Contains("/images/evaluates/"))
        {
            ctx.Context.Response.ContentType = "image/jpeg";
            ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
        }
    },
    ServeUnknownFileTypes = true
});

app.UseCors("AllowedFrontend");
app.UseWebSockets();
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();
app.UseExceptionHandler();
app.MapControllers();
app.MapHub<NotificationHub>("/hubs/notificationHub");

app.Run();
