namespace API.Contracts;

public sealed record ApiMessageResponse(string Message);

public sealed record ApiTokenResponse(string Message, string Token);

public sealed record ApiErrorResponse(
    string Code,
    string Message,
    int Status,
    string TraceId,
    IReadOnlyDictionary<string, string[]>? Errors = null);
