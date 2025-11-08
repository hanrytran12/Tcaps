using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace API.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        private readonly IHostEnvironment _env; // <-- THÊM DÒNG NÀY

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IHostEnvironment env) // <-- SỬA CONSTRUCTOR
        {
            _logger = logger;
            _env = env; // <-- THÊM DÒNG NÀY
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

            // Bắt đầu với một lỗi 500 chung chung
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Lỗi hệ thống",
                Detail = "Đã có một lỗi không mong muốn xảy ra. Vui lòng thử lại sau."
            };

            if (_env.IsDevelopment() && problemDetails.Status == 500)
            {
                problemDetails.Title = exception.GetType().Name;
                problemDetails.Detail = exception.ToString();
            }

            httpContext.Response.StatusCode = problemDetails.Status.Value;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}