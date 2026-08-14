using Application.Common.Exceptions;
using API.Contracts;
using Microsoft.AspNetCore.Diagnostics;

namespace API.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

            if (exception is FluentValidation.ValidationException validationException)
            {
                var errors = validationException.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );

                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsJsonAsync(
                    new ApiErrorResponse(
                        "validation_error",
                        "Dữ liệu không hợp lệ",
                        StatusCodes.Status400BadRequest,
                        httpContext.TraceIdentifier,
                        errors),
                    cancellationToken);
                return true;
            }

            (int statusCode, string code, string message) = exception switch
            {
                NotFoundException =>
                    (StatusCodes.Status404NotFound, "resource_not_found", exception.Message),

                ArgumentException or ArgumentNullException or InvalidOperationException =>
                    (StatusCodes.Status400BadRequest, "invalid_request", exception.Message),

                UnauthorizedAccessException =>
                    (StatusCodes.Status401Unauthorized, "unauthorized", exception.Message),

                BadRequestException =>
                    (StatusCodes.Status400BadRequest, "invalid_request", exception.Message),

                ConflictException =>
                    (StatusCodes.Status409Conflict, "conflict", exception.Message),

                ForbiddenException =>
                    (StatusCodes.Status403Forbidden, "forbidden", exception.Message),

                _ => (StatusCodes.Status500InternalServerError, "internal_server_error", "Đã có lỗi không mong muốn xảy ra.")
            };

            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(
                new ApiErrorResponse(code, message, statusCode, httpContext.TraceIdentifier),
                cancellationToken);

            return true;
        }
    }
}
