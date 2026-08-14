using API.Controllers;
using API.Contracts;
using API.Mappings;
using Application.Common;
using Application.Features.Auth.Queries;
using Application.Features.Batches.Commands.UpdateLeadForBatch;
using Application.Features.ComponentDefect.Commands.UpdateComponentDefectConfirm;
using Application.Features.ComponentDefect.Commands.UpdateComponentDefectResolve;
using Application.Features.Productions.Command.AddProductionReport;
using Application.Features.Productions.Command.UpdateProduction;
using Application.Features.TaskTransferRequests.Command.CreateTaskTransferRequest;
using Application.Features.TaskTransferRequests.Command.UpdateApproveTaskTransferRequest;
using Application.Features.Workshop.Command.InsertWorkshop;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
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

    [Theory]
    [InlineData(ResultErrorType.NotFound, 404, "resource_not_found")]
    [InlineData(ResultErrorType.Conflict, 409, "conflict")]
    [InlineData(ResultErrorType.InternalServerError, 500, "internal_server_error")]
    public void Api_mapper_returns_stable_error_contract(ResultErrorType errorType, int status, string code)
    {
        var controller = new TestController();
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { TraceIdentifier = "trace-123" }
        };

        var result = errorType switch
        {
            ResultErrorType.NotFound => Result.NotFound("Missing"),
            ResultErrorType.Conflict => Result.Conflict("Conflict"),
            _ => Result.Internal("Internal")
        };

        var action = controller.ToActionResult(result);
        var objectResult = Assert.IsType<ObjectResult>(action);
        var payload = Assert.IsType<ApiErrorResponse>(objectResult.Value);

        Assert.Equal(status, objectResult.StatusCode);
        Assert.Equal(code, payload.Code);
        Assert.Equal(status, payload.Status);
        Assert.Equal("trace-123", payload.TraceId);
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
    [InlineData(typeof(ProductionController), nameof(ProductionController.ReportWork), typeof(AddProductionReportCommand))]
    [InlineData(typeof(ProductionController), nameof(ProductionController.UpdateQuantity), typeof(UpdateProductionCommand))]
    [InlineData(typeof(TaskTransferRequestController), nameof(TaskTransferRequestController.CreateAsync), typeof(CreateTaskTransferRequestCommand))]
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

    [Fact]
    public void Legacy_read_routes_match_the_controller_contract()
    {
        Assert.Equal("staff/batches", GetAction<BatchController>(nameof(BatchController.GetBatchesByStaffIdAsync))
            .GetCustomAttribute<HttpGetAttribute>()?.Template);
        Assert.Equal("qc/batches", GetAction<BatchController>(nameof(BatchController.GetBatchesByQCIdAsync))
            .GetCustomAttribute<HttpGetAttribute>()?.Template);
        Assert.Equal("qc-lead-admin/assign-history/{batchId}",
            GetAction<AssignmentController>(nameof(AssignmentController.GetAssignmentHistoryForQCAsync))
                .GetCustomAttribute<HttpGetAttribute>()?.Template);
    }

    [Fact]
    public void Direct_list_read_actions_return_unwrapped_action_results()
    {
        Assert.Contains("List", GetAction<AssignmentController>(nameof(AssignmentController.GetAssignmentHistoryForQCAsync))
            .ReturnType.ToString());
        Assert.Contains("List", GetAction<AssignmentController>(nameof(AssignmentController.GetDetailAssignmentForQCAsync))
            .ReturnType.ToString());
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

    private sealed class TestController : ControllerBase
    {
    }
}
