using API.Controllers;
using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Xunit;

namespace API.Tests;

public class AuthorizationMetadataTests
{
    [Fact]
    public void Auth_recovery_actions_are_anonymous()
    {
        var actionNames = new[] { "LoginAsync", "RegisterAsync", "ForgotPassword", "VerifyOtp", "ResetPassword" };

        foreach (var actionName in actionNames)
        {
            var action = GetAction(typeof(AuthController), actionName);

            Assert.NotNull(action.GetCustomAttribute<AllowAnonymousAttribute>());
        }
    }

    [Fact]
    public void User_administration_actions_require_admin()
    {
        foreach (var actionName in new[] { "UpdateUser", "ReactiveUser", "DeleteUser" })
        {
            var authorization = GetAction(typeof(UserController), actionName)
                .GetCustomAttribute<AuthorizeAttribute>();

            Assert.NotNull(authorization);
            Assert.Equal("Admin", authorization!.Roles);
        }
    }

    [Fact]
    public void QC_transport_task_lookup_uses_named_policy()
    {
        var authorization = GetAction(typeof(TaskTransferRequestController), "GetByQcTransportAsync")
            .GetCustomAttribute<AuthorizeAttribute>();

        Assert.NotNull(authorization);
        Assert.Equal("QCTransportOnly", authorization!.Policy);
    }

    [Fact]
    public async Task Named_policies_cover_qck_and_authenticated_fallback()
    {
        using var provider = new ServiceCollection()
            .AddApiAuthorization()
            .BuildServiceProvider();

        var policyProvider = provider.GetRequiredService<IAuthorizationPolicyProvider>();
        var qcPolicy = await policyProvider.GetPolicyAsync("QC");
        var qckPolicy = await policyProvider.GetPolicyAsync("QCK");
        var qcTransportPolicy = await policyProvider.GetPolicyAsync("QCTransportOnly");
        var fallbackPolicy = await policyProvider.GetFallbackPolicyAsync();

        Assert.Contains("QC", GetAllowedRoles(qcPolicy));
        Assert.Contains("QCK", GetAllowedRoles(qcPolicy));
        Assert.Equal(new[] { "QCK" }, GetAllowedRoles(qckPolicy));
        Assert.Equal(new[] { "QCTransport" }, GetAllowedRoles(qcTransportPolicy));
        Assert.NotNull(fallbackPolicy);
        Assert.Contains(fallbackPolicy!.Requirements, requirement =>
            requirement is DenyAnonymousAuthorizationRequirement);
    }

    [Fact]
    public void Every_controller_action_has_explicit_auth_metadata()
    {
        var controllerTypes = typeof(AuthController).Assembly
            .GetTypes()
            .Where(type => type.Namespace == typeof(AuthController).Namespace)
            .Where(type => type.IsClass && !type.IsAbstract && type.Name.EndsWith("Controller"));

        var missing = new List<string>();

        foreach (var controllerType in controllerTypes)
        {
            var controllerAuthorization = controllerType.GetCustomAttribute<AuthorizeAttribute>();

            foreach (var action in controllerType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                         .Where(IsHttpAction))
            {
                var hasActionAuthorization = action.GetCustomAttribute<AuthorizeAttribute>() is not null;
                var isAnonymous = action.GetCustomAttribute<AllowAnonymousAttribute>() is not null;

                if (!hasActionAuthorization && !isAnonymous && controllerAuthorization is null)
                {
                    missing.Add($"{controllerType.Name}.{action.Name}");
                }
            }
        }

        Assert.Empty(missing);
    }

    private static MethodInfo GetAction(Type controllerType, string actionName)
    {
        return controllerType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .Single(method => method.Name == actionName);
    }

    private static bool IsHttpAction(MethodInfo method)
    {
        return method.GetCustomAttributes<HttpMethodAttribute>().Any();
    }

    private static string[] GetAllowedRoles(AuthorizationPolicy? policy)
    {
        Assert.NotNull(policy);
        return policy!.Requirements
            .OfType<RolesAuthorizationRequirement>()
            .Single()
            .AllowedRoles
            .ToArray();
    }
}
