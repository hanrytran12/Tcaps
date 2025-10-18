using Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

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

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Lỗi hệ thống",
                Detail = "Đã có một lỗi không mong muốn xảy ra. Vui lòng thử lại sau."
            };

            switch (exception)
            {
                case NotFoundException notFoundException:
                    problemDetails.Status = StatusCodes.Status404NotFound;
                    problemDetails.Title = "Không tìm thấy tài nguyên";
                    problemDetails.Detail = notFoundException.Message;
                    break;

                case ValidationException validationException:
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "Yêu cầu không hợp lệ";
                    problemDetails.Detail = validationException.Message;
                    break;
            }

            httpContext.Response.StatusCode = problemDetails.Status.Value;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
