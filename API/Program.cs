using API.Middlewares;
using Application.Common.Behaviors;
using Application.Features.Assignments.Commands.AddAssignmentCommand;
using Application.Features.Assignments.Commands.CompleteAssignment;
using Application.Features.Auth.Queries;
using Application.Features.Batches.Commands.AddBatch;
using Application.Features.Batches.Commands.DeleteBatch;
using Application.Features.Batches.Commands.UpdateBatch;
using Application.Features.Batches.Queries.GetAllBatch;
using Application.Features.Batches.Queries.GetBatchByWorkshopId;
using Application.Features.Batches.Queries.GetDashboardStats;
using Application.Features.ComponentDefect.Commands.UpdateComponentDefectResolve;
using Application.Features.ComponentDefects.Query.GetComponentDefects;
using Application.Features.Evaluates.Commands.AddEvaluate;
using Application.Features.Evaluates.Commands.UpdateEvaluate;
using Application.Features.Evaluates.Queries.GetAllEvaluate;
using Application.Features.Evaluates.Queries.GetEvaluatesByQCId;
using Application.Features.Inventories.Commands.AddInventory;
using Application.Features.Inventories.Queries.GetInventoryById;
using Application.Features.MaterialRequest.Commands.AddMaterialRequest;
using Application.Features.MaterialRequest.Commands.ApproveRequestFromLead;
using Application.Features.MaterialRequest.Commands.ConfirmRequestFromQc;
using Application.Features.MaterialRequest.Commands.RejectMaterialRequest;
using Application.Features.Notifications.Commands.MarkNotificationAsRead;
using Application.Features.Notifications.Queries.GetNotifications;
using Application.Features.Productions.Command.AddProduction;
using Application.Features.Productions.Query.GetAllProduction;
using Application.Features.Productions.Query.GetAllProductionByQCId;
using Application.Features.Productions.Query.GetAllProductionByStaffId;
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
using Infrastructure.BackgroundServices;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

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
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IComponentDefectRepository, ComponentDefectRepository>();
builder.Services.AddScoped<IMaterialUseRepository, MaterialUseRepository>();
builder.Services.AddScoped<IFileStorageService, FileStorageService>();


builder.Services.AddScoped<IAppDbContext>(provider =>
    provider.GetRequiredService<AppDbContext>());
builder.Services.AddScoped<IUnitOfWork>(provider =>
    provider.GetRequiredService<AppDbContext>());

builder.Services.AddValidatorsFromAssembly(typeof(IAppDbContext).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

builder.Services.AddHostedService<DeadlineCheckerService>();

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
                                                                      typeof(GetBatchByWorkshopIdQuery).Assembly,

                                                                      typeof(GetDashboardStatsQuery).Assembly,
                                                                      typeof(GetStaffPerformanceQuery).Assembly,

                                                                      typeof(AddAssignmentCommand).Assembly,
                                                                      typeof(CompleteAssignmentCommand).Assembly,

                                                                      typeof(AddMaterialRequestCommand).Assembly,
                                                                      typeof(ApproveRequestFromLeadCommand).Assembly,
                                                                      typeof(ConfirmRequestFromQcCommand).Assembly,
                                                                      typeof(RejectMaterialRequestCommand).Assembly,

                                                                      typeof(GetNotificationsQuery).Assembly,
                                                                      typeof(MarkNotificationAsReadCommand).Assembly,

                                                                      typeof(AddInventoryCommand).Assembly,
                                                                      typeof(GetInventoryByIdQuery).Assembly,

                                                                      typeof(GetComponentDefectsQuery).Assembly,

                                                                      typeof(LoginQuery).Assembly,

                                                                      typeof(AddEvaluateCommand).Assembly,
                                                                      typeof(UpdateEvaluateCommand).Assembly,
                                                                      typeof(GetAllEvaluateQuery).Assembly,
                                                                      typeof(GetEvaluatesByQCIdQuery).Assembly,

                                                                      typeof(AddProductionCommand).Assembly,
                                                                      typeof(GetAllProductionQuery).Assembly,
                                                                      typeof(GetAllProductionByQCIdQuery).Assembly,
                                                                      typeof(GetAllProductionByStaffIdQuery).Assembly,

                                                                      typeof(UpdateComponentDefectResolvedCommand).Assembly
                                                                      ));

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddProblemDetails();

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

    options.AddPolicy("CanCreateMaterialRequest", policy =>
        policy.RequireRole("Lead", "QC"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.UseExceptionHandler();
//app.UseDeveloperExceptionPage();


app.MapControllers();

app.Run();
