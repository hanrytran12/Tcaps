using API.Contracts;
using Application.Common;
using Application.Common.Exceptions;

namespace API.Mappings;

internal static class ApiErrorResponseFactory
{
    public static ApiErrorResponse FromResult<T>(Result<T> result, string traceId)
    {
        ArgumentNullException.ThrowIfNull(result);
        return Create(
            result.ErrorType,
            result.ErrorCode,
            result.Error,
            result.ValidationErrors,
            traceId);
    }

    public static ApiErrorResponse FromResult(Result result, string traceId)
    {
        ArgumentNullException.ThrowIfNull(result);
        return Create(
            result.ErrorType,
            result.ErrorCode,
            result.Error,
            result.ValidationErrors,
            traceId);
    }

    public static ApiErrorResponse FromException(Exception exception, string traceId)
    {
        ArgumentNullException.ThrowIfNull(exception);

        if (exception is FluentValidation.ValidationException validationException)
        {
            var errors = validationException.Errors
                .GroupBy(error => error.PropertyName)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(error => error.ErrorMessage).ToArray());

            return new ApiErrorResponse(
                "validation_error",
                "Dữ liệu không hợp lệ",
                StatusCodes.Status400BadRequest,
                traceId,
                errors);
        }

        var details = exception switch
        {
            NotFoundException => ("resource_not_found", "Không tìm thấy tài nguyên.", StatusCodes.Status404NotFound),
            ArgumentException or InvalidOperationException =>
                ("invalid_request", "Dữ liệu yêu cầu không hợp lệ.", StatusCodes.Status400BadRequest),
            UnauthorizedAccessException =>
                ("unauthorized", "Thông tin xác thực không hợp lệ.", StatusCodes.Status401Unauthorized),
            BadRequestException =>
                ("invalid_request", "Yêu cầu không hợp lệ.", StatusCodes.Status400BadRequest),
            ConflictException =>
                ("conflict", "Dữ liệu bị xung đột.", StatusCodes.Status409Conflict),
            ForbiddenException =>
                ("forbidden", "Bạn không có quyền thực hiện thao tác này.", StatusCodes.Status403Forbidden),
            _ =>
                ("internal_server_error", "Đã có lỗi không mong muốn xảy ra.", StatusCodes.Status500InternalServerError)
        };

        return new ApiErrorResponse(details.Item1, details.Item2, details.Item3, traceId);
    }

    private static ApiErrorResponse Create(
        ResultErrorType? errorType,
        string? errorCode,
        string? error,
        IReadOnlyDictionary<string, string[]>? validationErrors,
        string traceId)
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

        return new ApiErrorResponse(
            errorCode ?? "request_failed",
            error ?? "Yêu cầu không hợp lệ.",
            status,
            traceId,
            validationErrors);
    }
}
