using Application.Common.Behaviors;
using Application.Interfaces;
using Application.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, Assembly apiAssembly, Assembly infrastructureAssembly)
        {
            // Đăng ký AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Đăng ký FluentValidator
            services.AddValidatorsFromAssembly(typeof(IAppDbContext).Assembly);

            // Đăng ký Behavior
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            // Đăng ký MediatR
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(IAppDbContext).Assembly,
                                                                        apiAssembly,
                                                                        infrastructureAssembly));

            // Đăng ký các Service
            services.AddScoped<IAssignmentCompletionService, AssignmentCompletionService>();
            services.AddScoped<IStaffService, StaffService>();
            services.AddScoped<IProductionService, ProductionService>();

            return services;
        }
    }
}