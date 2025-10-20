using API.Middlewares;
using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Features.Products.Commands.AddProduct;
using Application.Features.Batches.Commands.AddBatch;
using Application.Features.Users.Commands.AddUser;
using Application.Features.Batches.Commands.DeleteBatch;
using Application.Features.Users.Commands.DeleteUser;
using Application.Features.Batches.Commands.UpdateBatch;
using Application.Features.Products.Commands.UpdateProduct;
using Application.Features.Batches.Queries.GetAllBatch;
using Application.Features.Products.Queries.GetAllProduct;
using Application.Features.Users.Queries.GetAllUser;
using Application.Features.Batches.Queries.GetDashboardStats;

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

builder.Services.AddScoped<IAssignmentRepository, AssignmentRepository>();
builder.Services.AddScoped<IIncomeRepository, IncomeRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IWorkshopRepository, WorkshopRepository>();
builder.Services.AddScoped<IProductionRepository, ProductionRepository>();
builder.Services.AddScoped<IStaffService, StaffService>();


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
                                                                      typeof(GetDashboardStatsQuery).Assembly));

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseExceptionHandler();

app.MapControllers();

app.Run();
