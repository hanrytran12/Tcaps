using Application.Common.Behaviors;
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
            // Đăng ký DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Environment.GetEnvironmentVariable("TCAPS_DB_CONNECTION")));

            // Đăng ký các interface của DbContext
            services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());
            services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<AppDbContext>());

            // Đăng ký Azure Blob Service
            services.AddSingleton(x =>
                new BlobServiceClient(Environment.GetEnvironmentVariable("BLOB_STORAGE_SETTINGS")));

            // Đăng ký Behavior của Infrastructure
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

            // Đăng ký Background Service
            services.AddHostedService<DeadlineCheckerService>();

            // Đăng ký các Service của Infrastructure
            services.AddScoped<IFileStorageService, AzureBlobStorageService>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<INotificationService, NotificationServices>();
            services.AddScoped<INotificationRealtimeService, SignalRNotificationService>();

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
            services.AddScoped<IAssignmentTransferRequestRepository, AssisgnmentTransferRequestRepository>();
            services.AddScoped<IMaterialWorkshopRepository, MaterialWorkshopRepository>();
            services.AddScoped<ITaskTransferRequestRepository, TaskTransferRequestRepository>();
            services.AddScoped<IWorkshopInventoryRepository, WorkshopInventoryRepository>();
            services.AddScoped<IReworkRequestRepository, ReworkRequestRepository>();
            services.AddScoped<IMaterialSupplyRepository, MaterialSupplyRepository>();

            return services;
        }
    }
}