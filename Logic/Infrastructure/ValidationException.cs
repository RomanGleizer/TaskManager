namespace Logic.Infrastructure;

public class ValidationException : Exception
{
    public string Property { get; init; }

    public ValidationException(string message, string prop) : base(message)
    {
        Property = prop;
    }
}