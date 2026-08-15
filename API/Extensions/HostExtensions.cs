using Infrastructure.Hubs;
using DotNetEnv;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Seeders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class HostExtensions
{
    private const long MaxRequestSize = 20_971_520;

    public static void LoadDotEnvFromCurrentOrParentDirectory()
    {
        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (directory is not null)
        {
            var envPath = Path.Combine(directory.FullName, ".env");
            if (File.Exists(envPath))
            {
                Env.Load(envPath);
                return;
            }

            directory = directory.Parent;
        }
    }

    public static void ConfigureHost(this WebApplicationBuilder builder)
    {
        ConfigureDevelopmentDataProtection(builder);
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenAnyIP(5001, listenOptions =>
            {
                listenOptions.Protocols = HttpProtocols.Http2;
            });
            options.ListenAnyIP(5000, listenOptions =>
            {
                listenOptions.Protocols = HttpProtocols.Http1;
            });
            options.Limits.MaxRequestBodySize = MaxRequestSize;
        });

        builder.Services.Configure<FormOptions>(options =>
        {
            options.ValueLengthLimit = int.MaxValue;
            options.MultipartBodyLengthLimit = MaxRequestSize;
            options.MemoryBufferThreshold = int.MaxValue;
        });
    }

    public static async Task MigrateAndSeedDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        const int retries = 5;
        for (var attempt = 0; attempt < retries; attempt++)
        {
            try
            {
                await db.Database.MigrateAsync();
                await DbSeeder.SeedAllAsync(db);
                Console.WriteLine("Database migrated and seeded successfully.");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database not ready, retrying in 5s... ({attempt + 1}/{retries})");
                Console.WriteLine($"Error: {ex.Message}");
                await Task.Delay(TimeSpan.FromSeconds(5));

                if (attempt == retries - 1)
                {
                    throw;
                }
            }
        }
    }

    public static void UseApiPipeline(this WebApplication app)
    {
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseCors("AllowedFrontend");
        app.UseStaticFiles(new StaticFileOptions
        {
            OnPrepareResponse = context =>
            {
                var path = context.Context.Request.Path.Value?.ToLower() ?? string.Empty;
                if (path.Contains("/images/products/") ||
                    path.Contains("/images/batches/") ||
                    path.Contains("/images/inventories/") ||
                    path.Contains("/images/evaluates/"))
                {
                    context.Context.Response.ContentType = "image/jpeg";
                    context.Context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
                }
            },
            ServeUnknownFileTypes = true
        });
        app.UseWebSockets();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseRateLimiter();
        app.UseExceptionHandler();
        app.MapControllers();
        app.MapHub<NotificationHub>("/hubs/notificationHub");
    }

    private static void ConfigureDevelopmentDataProtection(WebApplicationBuilder builder)
    {
        if (!builder.Environment.IsDevelopment())
        {
            return;
        }

        var dataProtectionKeysDirectory = Path.Combine(
            builder.Environment.ContentRootPath,
            ".aspnet",
            "DataProtection-Keys");

        Directory.CreateDirectory(dataProtectionKeysDirectory);
        builder.Services.AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo(dataProtectionKeysDirectory));
    }
}
