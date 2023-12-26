namespace LoggingService.Domain.Shared;
public partial class Result
{
    public static Result Success()
        => new Result(true, null);
    public static Result Failure(Error error)
        => new Result(false, error);
    public static Result<TResult> Success<TResult>(TResult value)
        => new Result<TResult>(true, value, null);
    public static Result<TResult> Failure<TResult>(Error error)
       => new Result<TResult>(false, default, error);
}
