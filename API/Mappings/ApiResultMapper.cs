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
        Result<T> result)
    {
        ArgumentNullException.ThrowIfNull(controller);
        ArgumentNullException.ThrowIfNull(result);
        EnsureFailure(result.IsSuccess);

        return controller.ToErrorActionResult(
            ApiErrorResponseFactory.FromResult(result, controller.HttpContext.TraceIdentifier));
    }

    public static IActionResult ToErrorActionResult(
        this ControllerBase controller,
        Result result)
    {
        ArgumentNullException.ThrowIfNull(controller);
        ArgumentNullException.ThrowIfNull(result);
        EnsureFailure(result.IsSuccess);

        return controller.ToErrorActionResult(
            ApiErrorResponseFactory.FromResult(result, controller.HttpContext.TraceIdentifier));
    }

    private static IActionResult ToErrorActionResult(
        this ControllerBase controller,
        ApiErrorResponse payload)
    {
        ArgumentNullException.ThrowIfNull(payload);
        return controller.StatusCode(payload.Status, payload);
    }

    private static void EnsureFailure(bool isSuccess)
    {
        if (isSuccess)
        {
            throw new ArgumentException(
                "Only failed results can be mapped to an error response.",
                "result");
        }
    }
}
