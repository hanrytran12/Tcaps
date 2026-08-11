using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace API.Extensions;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddApiAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
            options.AddPolicy("Lead", policy => policy.RequireRole("Lead"));
            options.AddPolicy("QC", policy => policy.RequireRole("QC", "QCK"));
            options.AddPolicy("GuardQC", policy => policy.RequireRole("GuardQC"));
            options.AddPolicy("QCK", policy => policy.RequireRole("QCK"));
            options.AddPolicy("QCTransportOnly", policy => policy.RequireRole("QCTransport"));
            options.AddPolicy("LeadOrValidQCTransport", policy =>
                policy.RequireAssertion(context =>
                {
                    var role = context.User.FindFirstValue(ClaimTypes.Role);
                    return role == "Lead" ||
                           (role == "QCTransport" && context.User.HasClaim("isQcTransport", "true"));
                }));
            options.AddPolicy("CanCreateMaterialRequest", policy => policy.RequireRole("Lead", "QC"));
            options.AddPolicy("CanViewDashboard", policy => policy.RequireRole("Admin", "Lead"));
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });

        return services;
    }
}
