using Application.Common;
using API.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace API.Mappings;

public static class ApiResultMapper
{
    public static IActionResult ToActionResult<T>(
        this ControllerBase controller,
        Result<T> result,
        string? successMessage = null)
    {
        ArgumentNullException.ThrowIfNull(controller);
        ArgumentNullException.ThrowIfNull(result);

        if (!result.IsSuccess)
        {
            return controller.ToErrorActionResult(result);
        }

        return string.IsNullOrWhiteSpace(successMessage)
            ? controller.Ok(result.Value)
            : controller.Ok(new ApiMessageResponse(successMessage));
    }

    public static IActionResult ToActionResult(
        this ControllerBase controller,
        Result result,
        string? successMessage = null)
    {
        ArgumentNullException.ThrowIfNull(controller);
        ArgumentNullException.ThrowIfNull(result);

        if (!result.IsSuccess)
        {
            return controller.ToErrorActionResult(result);
        }

        return string.IsNullOrWhiteSpace(successMessage)
            ? controller.NoContent()
            : controller.Ok(new ApiMessageResponse(successMessage));
    }

    public static IActionResult ToErrorActionResult<T>(
        this ControllerBase controller,
        Result<T> result) =>
        controller.ToErrorActionResult(
            result.ErrorType,
            result.ErrorCode,
            result.Error,
            result.ValidationErrors);

    public static IActionResult ToErrorActionResult(
        this ControllerBase controller,
        Result result) =>
        controller.ToErrorActionResult(
            result.ErrorType,
            result.ErrorCode,
            result.Error,
            result.ValidationErrors);

    private static IActionResult ToErrorActionResult(
        this ControllerBase controller,
        ResultErrorType? errorType,
        string? errorCode,
        string? error,
        IReadOnlyDictionary<string, string[]>? validationErrors)
    {
        var status = errorType switch
        {
            ResultErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ResultErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ResultErrorType.NotFound => StatusCodes.Status404NotFound,
            ResultErrorType.Conflict => StatusCodes.Status409Conflict,
            ResultErrorType.InternalServerError => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status400BadRequest
        };

        var payload = new ApiErrorResponse(
            errorCode ?? "request_failed",
            error ?? "Yêu cầu không hợp lệ.",
            status,
            controller.HttpContext.TraceIdentifier,
            validationErrors);

        return controller.StatusCode(status, payload);
    }
}
