using API.Mappings;
using Microsoft.AspNetCore.Diagnostics;

namespace API.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(
            exception,
            "Unhandled API exception. TraceId: {TraceId}",
            httpContext.TraceIdentifier);

        var payload = ApiErrorResponseFactory.FromException(
            exception,
            httpContext.TraceIdentifier);

        httpContext.Response.StatusCode = payload.Status;
        await httpContext.Response.WriteAsJsonAsync(payload, cancellationToken);

        return true;
    }
}
