namespace Desyco.Dms.Application.Common.Results;

public record Result<TValue>
{
    private readonly TValue? _value;
    private readonly Error.Error? _error;

    public bool IsError { get; }

    public bool IsSuccess => !IsError;

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Cannot access Value when in error state");

    public Error.Error Error => IsError
        ? _error!
        : throw new InvalidOperationException("Cannot access Error when in success state");

    public Result(TValue value)
    {
        _value = value;
        _error = null;
        IsError = false;
    }

    public Result(Error.Error error)
    {
        _value = default;
        _error = error;
        IsError = true;
    }

    public static implicit operator Result<TValue>(TValue value) => new(value);
    public static implicit operator Result<TValue>(Error.Error error) => new(error);
}

public record Result
{
    private readonly Error.Error? _error;

    public bool IsError { get; }
    public bool IsSuccess => !IsError;

    public Error.Error Error => IsError
        ? _error!
        : throw new InvalidOperationException("Cannot access Error when in success state");

    public Result()
    {
        _error = null;
        IsError = false;
    }

    public Result(Error.Error error)
    {
        _error = error;
        IsError = true;
    }

    public static Result Success() => new();
    public static implicit operator Result(Error.Error error) => new(error);
}
