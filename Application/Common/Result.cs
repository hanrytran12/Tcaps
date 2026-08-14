using Application.Common.Behaviors;

namespace Application.Common
{
    public enum ResultErrorType
    {
        Validation,
        NotFound,
        Conflict
    }

    public class Result<T> : IResult
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public T? Value { get; }
        public string? Error { get; }
        public ResultErrorType? ErrorType { get; }

        private Result(bool isSuccess, T? value, string? error, ResultErrorType? errorType)
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
        }

        public static Result<T> Success(T value) => new Result<T>(true, value, null, null);
        public static Result<T> Failure(string error) => new Result<T>(false, default, error, ResultErrorType.Validation);
        public static Result<T> NotFound(string error) => new Result<T>(false, default, error, ResultErrorType.NotFound);
        public static Result<T> Conflict(string error) => new Result<T>(false, default, error, ResultErrorType.Conflict);
    }

    public class Result : IResult
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public string? Error { get; }
        public ResultErrorType? ErrorType { get; }

        private Result(bool isSuccess, string? error, ResultErrorType? errorType)
        {
            if (isSuccess && error is not null)
                throw new InvalidOperationException();
            if (!isSuccess && error is null)
                throw new InvalidOperationException();

            IsSuccess = isSuccess;
            Error = error;
            ErrorType = errorType;
        }

        public static Result Success() => new Result(true, null, null);
        public static Result Failure(string error) => new Result(false, error, ResultErrorType.Validation);
        public static Result NotFound(string error) => new Result(false, error, ResultErrorType.NotFound);
        public static Result Conflict(string error) => new Result(false, error, ResultErrorType.Conflict);
    }
}
