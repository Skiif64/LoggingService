namespace LoggingService.Domain.Shared;
public partial class Result
{
    public bool IsSuccess { get; }
    public Error? Error { get; }

    protected Result(bool isSuccess, Error? error)
    {
        if(isSuccess && error is not null)
        {
            throw new ArgumentException("IsSuccess is true, but Error is not null.");
        }

        IsSuccess = isSuccess;
        Error = error;
    }
}

public class Result<TValue> : Result
{
    public TValue? Value { get; }
    protected internal Result(bool isSuccess, TValue? value, Error? error) 
        : base(isSuccess, error)
    {
        if(!isSuccess && value is not null)
        {
            throw new ArgumentException("IsSuccess is false, but Value is not null.");
        }
        if(isSuccess && value is null)
        {
            throw new ArgumentException("IsSuccess is true, but Value is null.");
        }

        Value = value;
    }
}
