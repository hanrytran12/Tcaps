using API.Hubs;
using API.Middlewares;
using Application;
using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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
        policy.WithOrigins("http://localhost:8081")
        .AllowAnyHeader()
        .AllowAnyMethod();
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

            ClockSkew = TimeSpan.Zero
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

builder.Services.AddSignalR();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var redisConnection = builder.Configuration.GetConnectionString("RedisConnection");

builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("OtpPolicy", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString(),
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 1,
                Window = TimeSpan.FromMinutes(5),
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
app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionHandler();
app.MapControllers();
app.MapHub<NotificationHub>("/notificationHub");
app.UseRateLimiter();

app.Run();
