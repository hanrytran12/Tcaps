using Application.Common.Behaviors;
using Application.DTOs.Request;
using Application.Interfaces;
using Azure.Storage.Blobs;
using Domain.Interfaces;
using Infrastructure.BackgroundServices;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services, IConfiguration configuration)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var isDevelopment = string.Equals(environmentName, "Development", StringComparison.OrdinalIgnoreCase);
            var isRunningInContainer = string.Equals(
                Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"),
                "true",
                StringComparison.OrdinalIgnoreCase);

            var configuredDbConnectionString = configuration.GetConnectionString("DefaultConnection");
            var environmentDbConnectionString = Environment.GetEnvironmentVariable("TCAPS_DB_CONNECTION");

            var dbConnectionString = isDevelopment && !isRunningInContainer
                ? configuredDbConnectionString ?? environmentDbConnectionString
                : environmentDbConnectionString ?? configuredDbConnectionString;

            if (string.IsNullOrWhiteSpace(dbConnectionString))
            {
                throw new InvalidOperationException(
                    "Missing database connection string. Set TCAPS_DB_CONNECTION or ConnectionStrings:DefaultConnection.");
            }

            var blobConnectionString = Environment.GetEnvironmentVariable("BLOB_STORAGE_SETTINGS")
                ?? configuration["BlobStorageSettings:ConnectionString"];

            if (string.IsNullOrWhiteSpace(blobConnectionString))
            {
                throw new InvalidOperationException(
                    "Missing blob storage connection string. Set BLOB_STORAGE_SETTINGS or BlobStorageSettings:ConnectionString.");
            }

            // Đăng ký DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(dbConnectionString, sqlOptions => sqlOptions.EnableRetryOnFailure()));

            // Đăng ký các interface của DbContext
            services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());
            services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<AppDbContext>());

            // Đăng ký Azure Blob Service
            services.AddSingleton(x =>
                new BlobServiceClient(blobConnectionString));

            // Đăng ký Behavior của Infrastructure
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

            // Đăng ký Background Service
            services.AddHostedService<DeadlineCheckerService>();

            // Đăng ký các Service của Infrastructure
            services.AddScoped<IFileStorageService, AzureBlobStorageService>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IOtpService, OtpService>();
            services.AddScoped<IAssignmentAutomationService, AssignmentAutomationService>();

            // Đăng ký Repositories 
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBatchRepository, BatchRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IAssignmentRepository, AssignmentRepository>();
            services.AddScoped<IIncomeRepository, IncomeRepository>();
            services.AddScoped<IWorkshopRepository, WorkshopRepository>();
            services.AddScoped<IProductionRepository, ProductionRepository>();
            services.AddScoped<IEvaluateRepository, EvaluateRepository>();
            services.AddScoped<IMaterialRequestRepository, MaterialRequestRepository>();
            services.AddScoped<IMaterialRepository, MaterialRepository>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddScoped<IComponentDefectRepository, ComponentDefectRepository>();
            services.AddScoped<IMaterialUseRepository, MaterialUseRepository>();
            services.AddScoped<IAssignmentTransferRequestRepository, AssignmentTransferRequestRepository>();
            services.AddScoped<IMaterialWorkshopRepository, MaterialWorkshopRepository>();
            services.AddScoped<ITaskTransferRequestRepository, TaskTransferRequestRepository>();
            services.AddScoped<IWorkshopInventoryRepository, WorkshopInventoryRepository>();
            services.AddScoped<IReworkRequestRepository, ReworkRequestRepository>();
            services.AddScoped<IMaterialSupplyRepository, MaterialSupplyRepository>();
            services.AddScoped<IFinalTransferRequestRepository, FinalTransferRequestRepository>();

            services.Configure<BrevoSettingsDTO>(configuration.GetSection("BrevoSettings"));

            services.AddHttpClient<IEmailService, EmailService>();

            return services;
        }
    }
}
