using API.Controllers;
using API.Contracts;
using API.Middlewares;
using API.Mappings;
using Application.Common;
using Application.Common.Exceptions;
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
using Microsoft.Extensions.Logging.Abstractions;
using System.Reflection;
using System.Text.Json;
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
    [InlineData(ResultErrorType.Validation, 400, "validation_error")]
    [InlineData(ResultErrorType.Unauthorized, 401, "unauthorized")]
    [InlineData(ResultErrorType.Forbidden, 403, "forbidden")]
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
            ResultErrorType.Validation => Result.Failure("Invalid"),
            ResultErrorType.Unauthorized => Result.Unauthorized("Unauthorized"),
            ResultErrorType.Forbidden => Result.Forbidden("Forbidden"),
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
    public void Api_mapper_preserves_validation_details()
    {
        var controller = CreateController("trace-validation");
        var errors = new Dictionary<string, string[]>
        {
            ["Email"] = ["Email is required"]
        };

        var action = controller.ToActionResult(
            Result.Failure("Invalid request", "validation_error", errors));
        var objectResult = Assert.IsType<ObjectResult>(action);
        var payload = Assert.IsType<ApiErrorResponse>(objectResult.Value);

        Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        Assert.Equal(errors, payload.Errors);
    }

    [Fact]
    public void Api_mapper_returns_no_content_for_message_less_success()
    {
        var action = CreateController("trace-success").ToActionResult(Result.Success());

        Assert.IsType<NoContentResult>(action);
    }

    [Fact]
    public void Api_mapper_returns_message_response_for_command_success()
    {
        var action = CreateController("trace-success")
            .ToActionResult(Result.Success(), "Updated");
        var objectResult = Assert.IsType<OkObjectResult>(action);
        var payload = Assert.IsType<ApiMessageResponse>(objectResult.Value);

        Assert.Equal("Updated", payload.Message);
    }

    [Fact]
    public void Error_mapper_rejects_success_result()
    {
        var controller = CreateController("trace-success");

        Assert.Throws<ArgumentException>(() =>
            controller.ToErrorActionResult(Result.Success()));
    }

    [Fact]
    public async Task Global_exception_handler_returns_safe_contract()
    {
        var context = new DefaultHttpContext
        {
            TraceIdentifier = "trace-exception"
        };
        context.Response.Body = new MemoryStream();
        var handler = new GlobalExceptionHandler(NullLogger<GlobalExceptionHandler>.Instance);

        var handled = await handler.TryHandleAsync(
            context,
            new NotFoundException("Internal database details"),
            CancellationToken.None);

        Assert.True(handled);
        Assert.Equal(StatusCodes.Status404NotFound, context.Response.StatusCode);

        context.Response.Body.Position = 0;
        using var document = await JsonDocument.ParseAsync(context.Response.Body);
        var payload = document.RootElement;

        Assert.Equal("resource_not_found", payload.GetProperty("code").GetString());
        Assert.Equal("Không tìm thấy tài nguyên.", payload.GetProperty("message").GetString());
        Assert.Equal("trace-exception", payload.GetProperty("traceId").GetString());
    }

    [Theory]
    [InlineData(typeof(NotFoundException), 404, "resource_not_found", "Không tìm thấy tài nguyên.")]
    [InlineData(typeof(ArgumentException), 400, "invalid_request", "Dữ liệu yêu cầu không hợp lệ.")]
    [InlineData(typeof(UnauthorizedAccessException), 401, "unauthorized", "Thông tin xác thực không hợp lệ.")]
    [InlineData(typeof(BadRequestException), 400, "invalid_request", "Yêu cầu không hợp lệ.")]
    [InlineData(typeof(ConflictException), 409, "conflict", "Dữ liệu bị xung đột.")]
    [InlineData(typeof(ForbiddenException), 403, "forbidden", "Bạn không có quyền thực hiện thao tác này.")]
    [InlineData(typeof(Exception), 500, "internal_server_error", "Đã có lỗi không mong muốn xảy ra.")]
    public async Task Global_exception_handler_maps_exception_types(
        Type exceptionType,
        int status,
        string code,
        string message)
    {
        var context = new DefaultHttpContext
        {
            TraceIdentifier = "trace-exception-type"
        };
        context.Response.Body = new MemoryStream();
        var handler = new GlobalExceptionHandler(NullLogger<GlobalExceptionHandler>.Instance);
        var exception = (Exception)Activator.CreateInstance(exceptionType, "Internal details")!;

        await handler.TryHandleAsync(context, exception, CancellationToken.None);

        Assert.Equal(status, context.Response.StatusCode);
        context.Response.Body.Position = 0;
        using var document = await JsonDocument.ParseAsync(context.Response.Body);
        var payload = document.RootElement;

        Assert.Equal(code, payload.GetProperty("code").GetString());
        Assert.Equal(message, payload.GetProperty("message").GetString());
        Assert.Equal(status, payload.GetProperty("status").GetInt32());
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

    private static TestController CreateController(string traceId)
    {
        var controller = new TestController();
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { TraceIdentifier = traceId }
        };
        return controller;
    }

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
