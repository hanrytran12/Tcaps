using Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace API.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        private readonly IHostEnvironment _env;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

            (int statusCode, string title, string detail) = exception switch
            {
                NotFoundException =>
                    (StatusCodes.Status404NotFound, "Không tìm thấy tài nguyên", exception.Message),

                ArgumentException or ArgumentNullException or InvalidOperationException =>
                    (StatusCodes.Status400BadRequest, "Dữ liệu không hợp lệ", exception.Message),

                UnauthorizedAccessException =>
                    (StatusCodes.Status401Unauthorized, "Truy cập bị từ chối", exception.Message),

                _ => (StatusCodes.Status500InternalServerError, "Lỗi hệ thống", "Đã có lỗi không mong muốn xảy ra.")
            };

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Instance = httpContext.Request.Path
            };

            if (statusCode == StatusCodes.Status500InternalServerError && _env.IsDevelopment())
            {
                problemDetails.Detail = exception.ToString();
            }

            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}