using API.Hubs;
using API.Middlewares;
using Application;
using DotNetEnv;
using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

Env.Load();

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
        policy.AllowAnyOrigin()
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY"))),

            ValidateIssuer = true,
            ValidIssuer = Environment.GetEnvironmentVariable("ISSUER"),

            ValidateAudience = true,
            ValidAudience = Environment.GetEnvironmentVariable("AUDIENCE"),

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

    options.AddPolicy("QCTransportOnly", policy =>
    {
        policy.RequireRole("QCTransport");
        policy.RequireClaim("isQcTransport", "true");
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

// -----------------------------
// Kestrel
// -----------------------------
builder.WebHost.ConfigureKestrel(options =>
{
    // gRPC endpoint  requires HTTP/2
    options.ListenAnyIP(5001, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
    });

    // REST endpoint  uses standard HTTP/1.1
    options.ListenAnyIP(5000, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1;
    });
});


builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = 104857600; // 100MB
    o.MemoryBufferThreshold = int.MaxValue;
});

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 104857600; // 100MB
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowedFrontend");
app.UseExceptionHandler();

// -----------------------------
// DB migration & seeding
// -----------------------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    var retries = 5;
    for (int i = 0; i < retries; i++)
    {
        try
        {
            // Create DB if it doesn’t exist and apply all migrations
            await db.Database.MigrateAsync();

            // Seed Users
            await Infrastructure.Persistence.Seeders.DbSeeder.SeedAllAsync(db);

            Console.WriteLine("Database migrated and seeded successfully.");
            break;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database not ready, retrying in 5s... ({i + 1}/{retries})");
            Console.WriteLine($"Error: {ex.Message}");
            await Task.Delay(5000);

            if (i == retries - 1)
                throw; // rethrow last exception if retries exhausted
        }
    }
}

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

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<NotificationHub>("/notificationHub");

app.Run();