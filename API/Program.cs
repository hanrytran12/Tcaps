using API.Middlewares;
using Application.Common.Behaviors;
using Application.Features.Assignments.Commands.CompleteAssignment;
using Application.Features.Assignments.Commands.PlanAssignments;
using Application.Features.Assignments.Queries.GetAllocatedMaterials;
using Application.Features.Assignments.Queries.GetAssignmentsByStaffId;
using Application.Features.AssingmentTransferRequest.Commands.AddAssignmenTransferRequest;
using Application.Features.AssingmentTransferRequest.Commands.UpdateAssignmentTransferRequest;
using Application.Features.AssingmentTransferRequest.Queries.GetReconciliationSummary;
using Application.Features.Auth.Queries;
using Application.Features.Batches.Commands.AddBatch;
using Application.Features.Batches.Commands.DeleteBatch;
using Application.Features.Batches.Commands.UpdateBatch;
using Application.Features.Batches.Queries.GetAllBatch;
using Application.Features.Batches.Queries.GetBatchById;
using Application.Features.Batches.Queries.GetBatchByWorkshopId;
using Application.Features.Batches.Queries.GetDashboardStats;
using Application.Features.ComponentDefect.Commands.UpdateComponentDefectResolve;
using Application.Features.ComponentDefects.Query.GetComponentDefects;
using Application.Features.Evaluates.Commands.AddEvaluate;
using Application.Features.Evaluates.Commands.UpdateEvaluate;
using Application.Features.Evaluates.Queries.GetAllEvaluate;
using Application.Features.Evaluates.Queries.GetEvaluatesByQCId;
using Application.Features.Incomes.Command.AddIncome;
using Application.Features.Incomes.Queries.GetIncomesByStaffId;
using Application.Features.Inventories.Commands.AddInventory;
using Application.Features.Inventories.Queries.GetInventoryById;
using Application.Features.MaterialRequest.Commands.ApproveRequestFromLead;
using Application.Features.MaterialRequest.Commands.ConfirmRequestFromQc;
using Application.Features.MaterialRequest.Commands.DispatchRequest;
using Application.Features.MaterialRequest.Commands.RejectMaterialRequest;
using Application.Features.MaterialRequest.Queries.GetAllMaterialRequest;
using Application.Features.MaterialRequest.Queries.GetPendingRequestForQc;
using Application.Features.Materials.Commands.AddMaterial;
using Application.Features.Materials.Queries.GetAllMaterialToWatch;
using Application.Features.MaterialWorkshops.Command.AddMaterialWorkshop;
using Application.Features.MaterialWorkshops.Command.UpdateConfirmMaterialWorkshop;
using Application.Features.MaterialWorkshops.Queries.GetAllMaterialWorkshop;
using Application.Features.MaterialWorkshops.Queries.GetMaterialWorkshopByQCId;
using Application.Features.Notifications.Commands.MarkNotificationAsRead;
using Application.Features.Notifications.Queries.GetNotifications;
using Application.Features.Productions.Command.AddProductionReport;
using Application.Features.Productions.Query.GetAllProduction;
using Application.Features.Productions.Query.GetAllProductionByQCId;
using Application.Features.Productions.Query.GetAllProductionByStaffId;
using Application.Features.Products.Commands.AddProduct;
using Application.Features.Products.Commands.UpdateProduct;
using Application.Features.Products.Queries.GetAllProduct;
using Application.Features.TaskTransferRequests.Command.CreateTaskTransferRequest;
using Application.Features.TaskTransferRequests.Command.UpdateApproveTaskTransferRequest;
using Application.Features.TaskTransferRequests.Queries.GetAllTaskTransferRequest;
using Application.Features.TaskTransferRequests.Queries.GetTaskTransferRequestByQCTransportId;
using Application.Features.Users.Commands.AddUser;
using Application.Features.Users.Commands.DeleteUser;
using Application.Features.Users.Queries.GetAllQCTransport;
using Application.Features.Users.Queries.GetAllUser;
using Application.Features.Users.Queries.GetGroupProgress;
using Application.Features.Users.Queries.GetStaffPerformance;
using Application.Features.Users.Queries.GetUserByWorkshopId;
using Application.Features.Workshop.Queries.GetWorkshopTemplate;
using Application.Interfaces;
using Application.Services;
using Azure.Storage.Blobs;
using Domain.Events;
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

builder.Services.AddSingleton(x =>
    new BlobServiceClient(conf["BlobStorageSettings:ConnectionString"]));

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
builder.Services.AddScoped<IAssignmentTransferRequestRepository, AssisgnmentTransferRequestRepository>();
builder.Services.AddScoped<IAssignmentCompletionService, AssignmentCompletionService>();
builder.Services.AddScoped<IMaterialWorkshopRepository, MaterialWorkshopRepository>();
builder.Services.AddScoped<ITaskTransferRequestRepository, TaskTransferRequestRepository>();
builder.Services.AddScoped<IWorkshopInventoryRepository, WorkshopInventoryRepository>();


builder.Services.AddScoped<IAppDbContext>(provider =>
    provider.GetRequiredService<AppDbContext>());
builder.Services.AddScoped<IUnitOfWork>(provider =>
    provider.GetRequiredService<AppDbContext>());

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowedFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:8081")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

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
                                                                      typeof(GetBatchByIdQuery).Assembly,

                                                                      typeof(GetDashboardStatsQuery).Assembly,
                                                                      typeof(GetStaffPerformanceQuery).Assembly,

                                                                      typeof(PlanAssignmentsCommand).Assembly,
                                                                      typeof(CompleteAssignmentCommand).Assembly,
                                                                      typeof(GetAssignmentsByStaffIdQuery).Assembly,

                                                                      typeof(DispatchRequestCommand).Assembly,
                                                                      typeof(ApproveRequestFromLeadCommand).Assembly,
                                                                      typeof(ConfirmRequestFromQcCommand).Assembly,
                                                                      typeof(RejectMaterialRequestCommand).Assembly,
                                                                      typeof(GetPendingRequestForQcQuery).Assembly,

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

                                                                      typeof(AddProductionReportCommand).Assembly,
                                                                      typeof(GetAllProductionQuery).Assembly,
                                                                      typeof(GetAllProductionByQCIdQuery).Assembly,
                                                                      typeof(GetAllProductionByStaffIdQuery).Assembly,

                                                                      typeof(UpdateComponentDefectResolvedCommand).Assembly,

                                                                      typeof(AddAssignmentTransferRequestCommand).Assembly,
                                                                      typeof(UpdateAssignmentTransferRequestCommand).Assembly,

                                                                      typeof(GetWorkshopTemplateQuery).Assembly,

                                                                      typeof(AddIncomeCommand).Assembly,
                                                                      typeof(GetIncomesByStaffIdQuery).Assembly,
                                                                      typeof(EvaluateCreatedEvent).Assembly,
                                                                      typeof(AddIncomeCommandHandler).Assembly,

                                                                      typeof(AddMaterialWorkshopCommand).Assembly,
                                                                      typeof(UpdateConfirmMaterialWorkshopCommand).Assembly,
                                                                      typeof(GetAllMaterialWorkshopQuery).Assembly,
                                                                      typeof(GetMaterialWorkshopByQCIdQuery).Assembly,

                                                                      typeof(GetReconciliationSummaryQuery).Assembly,
                                                                      typeof(GetAllocatedMaterialsQuery).Assembly,
                                                                      typeof(GetGroupProgressQuery).Assembly,

                                                                      typeof(CreateTaskTransferRequestCommand).Assembly,
                                                                      typeof(UpdateApproveTaskTransferRequestCommand).Assembly,
                                                                      typeof(GetAllTaskTransferRequestQuery).Assembly,
                                                                      typeof(GetTaskTransferRequestByQCTransportIdQuery).Assembly,

                                                                      typeof(GetAllQCTransportQuery).Assembly,

                                                                      typeof(GetAllMaterialToWatchQuery).Assembly,
                                                                      typeof(AddMaterialCommand).Assembly,

                                                                      typeof(GetUserByWorkshopIdQuery).Assembly,
                                                                      typeof(GetAllMaterialRequestQuery).Assembly
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

    options.AddPolicy("QCTransport", policy =>
        policy.RequireRole("QCTransport"));

    options.AddPolicy("CanCreateMaterialRequest", policy =>
        policy.RequireRole("Lead", "QC"));

    options.AddPolicy("CanViewDashboard", policy =>
        policy.RequireRole("Admin", "Lead"));
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
            path.Contains("/images/inventories/"))
        {
            // Set content type for files without extension
            ctx.Context.Response.ContentType = "image/jpeg";

            // Allow CORS for images
            ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
        }
    },
    ServeUnknownFileTypes = true // Allow serving files without extension
});

app.UseCors("AllowedFrontend");

app.UseAuthentication();

app.UseAuthorization();

app.UseExceptionHandler();
//app.UseDeveloperExceptionPage();

app.MapControllers();

app.Run();
