using Application.Common.Behaviors;

namespace Application.Common
{
    public enum ResultErrorType
    {
        Validation,
        NotFound,
        Unauthorized,
        Forbidden,
        Conflict,
        InternalServerError
    }

    public class Result<T> : IResult
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public T? Value { get; }
        public string? Error { get; }
        public ResultErrorType? ErrorType { get; }
        public string? ErrorCode { get; }
        public IReadOnlyDictionary<string, string[]>? ValidationErrors { get; }

        private Result(
            bool isSuccess,
            T? value,
            string? error,
            ResultErrorType? errorType,
            string? errorCode,
            IReadOnlyDictionary<string, string[]>? validationErrors)
        {
            if (isSuccess && error is not null)
            {
                throw new InvalidOperationException();
            }

            if (!isSuccess && error is null)
            {
                throw new InvalidOperationException();
            }

            IsSuccess = isSuccess;
            Value = value;
            Error = error;
            ErrorType = errorType;
            ErrorCode = errorCode;
            ValidationErrors = validationErrors;
        }

        public static Result<T> Success(T value) => new Result<T>(true, value, null, null, null, null);

        public static Result<T> Failure(
            string error,
            string errorCode = "validation_error",
            IReadOnlyDictionary<string, string[]>? validationErrors = null) =>
            new Result<T>(false, default, error, ResultErrorType.Validation, errorCode, validationErrors);

        public static Result<T> NotFound(string error, string errorCode = "resource_not_found") =>
            new Result<T>(false, default, error, ResultErrorType.NotFound, errorCode, null);

        public static Result<T> Unauthorized(string error, string errorCode = "unauthorized") =>
            new Result<T>(false, default, error, ResultErrorType.Unauthorized, errorCode, null);

        public static Result<T> Forbidden(string error, string errorCode = "forbidden") =>
            new Result<T>(false, default, error, ResultErrorType.Forbidden, errorCode, null);

        public static Result<T> Conflict(string error, string errorCode = "conflict") =>
            new Result<T>(false, default, error, ResultErrorType.Conflict, errorCode, null);

        public static Result<T> Internal(string error, string errorCode = "internal_server_error") =>
            new Result<T>(false, default, error, ResultErrorType.InternalServerError, errorCode, null);
    }

    public class Result : IResult
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public string? Error { get; }
        public ResultErrorType? ErrorType { get; }
        public string? ErrorCode { get; }
        public IReadOnlyDictionary<string, string[]>? ValidationErrors { get; }

        private Result(
            bool isSuccess,
            string? error,
            ResultErrorType? errorType,
            string? errorCode,
            IReadOnlyDictionary<string, string[]>? validationErrors)
        {
            if (isSuccess && error is not null)
                throw new InvalidOperationException();
            if (!isSuccess && error is null)
                throw new InvalidOperationException();

            IsSuccess = isSuccess;
            Error = error;
            ErrorType = errorType;
            ErrorCode = errorCode;
            ValidationErrors = validationErrors;
        }

        public static Result Success() => new Result(true, null, null, null, null);

        public static Result Failure(
            string error,
            string errorCode = "validation_error",
            IReadOnlyDictionary<string, string[]>? validationErrors = null) =>
            new Result(false, error, ResultErrorType.Validation, errorCode, validationErrors);

        public static Result NotFound(string error, string errorCode = "resource_not_found") =>
            new Result(false, error, ResultErrorType.NotFound, errorCode, null);

        public static Result Unauthorized(string error, string errorCode = "unauthorized") =>
            new Result(false, error, ResultErrorType.Unauthorized, errorCode, null);

        public static Result Forbidden(string error, string errorCode = "forbidden") =>
            new Result(false, error, ResultErrorType.Forbidden, errorCode, null);

        public static Result Conflict(string error, string errorCode = "conflict") =>
            new Result(false, error, ResultErrorType.Conflict, errorCode, null);

        public static Result Internal(string error, string errorCode = "internal_server_error") =>
            new Result(false, error, ResultErrorType.InternalServerError, errorCode, null);
    }
}
