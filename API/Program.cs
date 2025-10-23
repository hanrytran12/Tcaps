using API.Middlewares;
using Application.Common.Behaviors;
using Application.Features.Assignments.Commands.AddAssignmentCommand;
using Application.Features.Assignments.Commands.CompleteAssignment;
using Application.Features.Batches.Commands.AddBatch;
using Application.Features.Batches.Commands.DeleteBatch;
using Application.Features.Batches.Commands.UpdateBatch;
using Application.Features.Batches.Queries.GetAllBatch;
using Application.Features.Batches.Queries.GetDashboardStats;
using Application.Features.Inventories.Commands.AddInventory;
using Application.Features.Inventories.Queries.GetInventoryById;
using Application.Features.MaterialRequest.Commands.AddMaterialRequest;
using Application.Features.MaterialRequest.Commands.UpdateMaterialRequest;
using Application.Features.Notifications.Commands.MarkNotificationAsRead;
using Application.Features.Notifications.Queries.GetNotifications;
using Application.Features.Products.Commands.AddProduct;
using Application.Features.Products.Commands.UpdateProduct;
using Application.Features.Products.Queries.GetAllProduct;
using Application.Features.Users.Commands.AddUser;
using Application.Features.Users.Commands.DeleteUser;
using Application.Features.Users.Queries.GetAllUser;
using Application.Features.Users.Queries.GetStaffPerformance;
using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using FluentValidation;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBatchRepository, BatchRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationService, NotificationServices>();
builder.Services.AddScoped<IAssignmentRepository, AssignmentRepository>();
builder.Services.AddScoped<IIncomeRepository, IncomeRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IWorkshopRepository, WorkshopRepository>();
builder.Services.AddScoped<IProductionRepository, ProductionRepository>();
builder.Services.AddScoped<IEvaluateRepository, EvaluateRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IStaffService, StaffService>();
builder.Services.AddScoped<IProductionService, ProductionService>();
builder.Services.AddScoped<IMaterialRequestRepository, MaterialRequestRepository>();
builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();



builder.Services.AddScoped<IAppDbContext>(provider =>
    provider.GetRequiredService<AppDbContext>());

builder.Services.AddValidatorsFromAssembly(typeof(IAppDbContext).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly,
                                                                      typeof(AddProductCommand).Assembly,
                                                                      typeof(GetAllProductQuery).Assembly,
                                                                      typeof(UpdateProductCommand).Assembly,

                                                                      typeof(AddUserCommand).Assembly,
                                                                      typeof(GetAllUserQuery).Assembly,
                                                                      typeof(DeleteUserCommand).Assembly,

                                                                      typeof(AddBatchCommand).Assembly,
                                                                      typeof(GetAllBatchQuery).Assembly,
                                                                      typeof(DeleteBatchCommand).Assembly,
                                                                      typeof(UpdateBatchCommand).Assembly,

                                                                      typeof(GetDashboardStatsQuery).Assembly,
                                                                      typeof(GetStaffPerformanceQuery).Assembly,

                                                                      typeof(AddAssignmentCommand).Assembly,
                                                                      typeof(CompleteAssignmentCommand).Assembly,

                                                                      typeof(AddMaterialRequestCommand).Assembly,
                                                                      typeof(UpdateMaterialRequestCommand).Assembly,

                                                                      typeof(GetNotificationsQuery).Assembly,
                                                                      typeof(MarkNotificationAsReadCommand).Assembly,

                                                                      typeof(AddInventoryCommand).Assembly,
                                                                      typeof(GetInventoryByIdQuery).Assembly
                                                                      ));

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();

app.UseExceptionHandler();

app.MapControllers();

app.Run();
