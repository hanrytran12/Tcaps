using API.Middlewares;
using Application;
using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.RateLimiting;

namespace API.Extensions;

public static class ApiServiceExtensions
{
    public static WebApplicationBuilder AddApiServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        AddSwagger(builder.Services);
        builder.Services.AddApplicationServices(typeof(Program).Assembly, typeof(AppDbContext).Assembly);
        builder.Services.AddInfrastructureServices(builder.Configuration);
        AddCors(builder.Services);
        AddAuthentication(builder.Services);
        builder.Services.AddApiAuthorization();
        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.ClaimsIdentity.UserIdClaimType = JwtRegisteredClaimNames.Sub;
        });
        builder.Services.AddSignalR();
        builder.Services.AddSingleton<IUserIdProvider, Infrastructure.Hubs.CustomUserIdProvider>();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();
        AddRateLimiter(builder.Services);
        AddRedisCache(builder.Services, builder.Configuration);
        return builder;
    }

    private static void AddSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
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
                    Array.Empty<string>()
                }
            });
        });
    }

    private static void AddCors(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowedFrontend", policy =>
            {
                policy.WithOrigins("https://tcapscompany.com", "http://localhost:8081")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }

    private static void AddAuthentication(IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY")
                    ?? throw new InvalidOperationException("JWT_KEY environment variable is required.");

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    ValidateIssuer = true,
                    ValidIssuer = Environment.GetEnvironmentVariable("ISSUER"),
                    ValidateAudience = true,
                    ValidAudience = Environment.GetEnvironmentVariable("AUDIENCE"),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    NameClaimType = JwtRegisteredClaimNames.Sub
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });
    }

    private static void AddRateLimiter(IServiceCollection services)
    {
        services.AddRateLimiter(options =>
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
                    context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 3,
                        Window = TimeSpan.FromMinutes(10),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    }));
        });
    }

    private static void AddRedisCache(IServiceCollection services, IConfiguration configuration)
    {
        var redisConnection = configuration.GetConnectionString("RedisConnection");
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnection;
            options.InstanceName = "Tcaps_";
        });
    }
}
