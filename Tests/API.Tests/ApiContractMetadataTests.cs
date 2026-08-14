using API.Controllers;
using Application.Common;
using Application.Features.Auth.Queries;
using Application.Features.Batches.Commands.UpdateLeadForBatch;
using Application.Features.ComponentDefect.Commands.UpdateComponentDefectConfirm;
using Application.Features.ComponentDefect.Commands.UpdateComponentDefectResolve;
using Application.Features.TaskTransferRequests.Command.UpdateApproveTaskTransferRequest;
using Application.Features.Workshop.Command.InsertWorkshop;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Xunit;

namespace API.Tests;

public class ApiContractMetadataTests
{
    [Fact]
    public void Result_failure_types_map_explicitly_without_message_matching()
    {
        var notFound = Result.NotFound("resource missing");
        var conflict = Result.Conflict("duplicate resource");
        var validation = Result.Failure("invalid request");

        Assert.Equal(ResultErrorType.NotFound, notFound.ErrorType);
        Assert.Equal(ResultErrorType.Conflict, conflict.ErrorType);
        Assert.Equal(ResultErrorType.Validation, validation.ErrorType);
    }

    [Fact]
    public void Login_uses_post_and_json_body()
    {
        var action = GetAction<AuthController>(nameof(AuthController.LoginAsync));

        Assert.Equal("login", action.GetCustomAttribute<HttpPostAttribute>()?.Template);
        Assert.Null(action.GetCustomAttribute<HttpGetAttribute>());
        AssertBodyParameter<LoginQuery>(action);
    }

    [Theory]
    [InlineData(typeof(ComponentDefectController), nameof(ComponentDefectController.UpdateResolveAsync), typeof(UpdateComponentDefectResolvedCommand))]
    [InlineData(typeof(ComponentDefectController), nameof(ComponentDefectController.UpdateConfirmAsync), typeof(UpdateComponentDefectConfirmCommand))]
    [InlineData(typeof(TaskTransferRequestController), nameof(TaskTransferRequestController.ApproveRequestAsync), typeof(UpdateApproveTaskTransferRequestCommand))]
    [InlineData(typeof(BatchController), nameof(BatchController.UpdateLeadForBatch), typeof(UpdateLeadForBatchCommand))]
    [InlineData(typeof(WorkshopController), nameof(WorkshopController.InsertWorkshopAsync), typeof(InsertWorkshopCommand))]
    public void Complex_write_commands_are_bound_from_body(Type controllerType, string actionName, Type commandType)
    {
        var action = GetAction(controllerType, actionName);
        Assert.Contains(action.GetParameters(), parameter =>
            parameter.ParameterType == commandType &&
            parameter.GetCustomAttribute<FromBodyAttribute>() is not null);
    }

    [Fact]
    public void Product_update_accepts_multipart_form_data()
    {
        var action = GetAction<ProductController>(nameof(ProductController.UpdateProduct));

        Assert.Contains(action.GetParameters(), parameter =>
            parameter.GetCustomAttribute<FromFormAttribute>() is not null);
    }

    private static MethodInfo GetAction<TController>(string actionName) =>
        GetAction(typeof(TController), actionName);

    private static MethodInfo GetAction(Type controllerType, string actionName) =>
        controllerType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .Single(method => method.Name == actionName);

    private static void AssertBodyParameter<TCommand>(MethodInfo action)
    {
        Assert.Contains(action.GetParameters(), parameter =>
            parameter.ParameterType == typeof(TCommand) &&
            parameter.GetCustomAttribute<FromBodyAttribute>() is not null);
    }
}
