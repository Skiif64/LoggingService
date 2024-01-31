namespace LoggingService.Domain.Shared;
public partial class Result
{
    private readonly Error? _error;
    public bool IsSuccess { get; }
    public Error Error
    {
        get
        {
            if (IsSuccess)
                throw new InvalidOperationException("Cannot get error from success result");
            return _error!.Value;
        }
    }

    protected Result(bool isSuccess, Error? error)
    {
        if(isSuccess && error is not null)
        {
            throw new ArgumentException("IsSuccess is true, but Error is not null.");
        }

        IsSuccess = isSuccess;
        _error = error;
    }
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    public TValue Value
    {
        get
        {
            if (!IsSuccess)
                throw new InvalidOperationException("Cannot get value from failure result");
            return _value!;
        }
    }
    
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

        _value = value;
    }
}
