namespace FPIS.Domain.ViewModels;

public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string? Error { get; }
    public int? ErrorCode { get; }

    private Result(bool isSuccess, T? value, string? error, int? errorCode)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
        ErrorCode = errorCode;
    }

    public static Result<T> Success(T value) => new(true, value, null, null);
    public static Result<T> Failure(string error, int? errorCode = null) => new(false, default, error, errorCode);
}
