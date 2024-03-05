namespace FinalWebApi.API.Util;

public abstract class CustomError
{
    public string Message;
}

public class NotFoundError : CustomError
{
    public NotFoundError(string msg)
    {
        Message = msg;
    }
}

public class InternalError : CustomError
{
    public InternalError(string msg)
    {
        Message = msg;
    }
}

// Class that represent Error or TValue
public class ErrorOr<TValue>
{
    public CustomError Error { get; private set; }
    public TValue Value { get; private set; }

    // Check if this object has error;
    public bool HasError = false;

    public ErrorOr<TValue> AddError(CustomError error)
    {
        // if this object already have value, cannot add error
        if (Value != null)
        {
            return this;
        }

        Error = error;
        HasError = true;

        return this;
    }

    public ErrorOr<TValue> AddValue(TValue value)
    {
        // if this object already has error, cannot add value
        if (HasError)
        {
            return this;
        }

        Value = value;
        return this;
    }
}
