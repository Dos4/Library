namespace Foxminded.Library.Application.Resources.Exceptions;

public class DuplicateBookException : Exception
{
    public DuplicateBookException() : base(TechnicalMessage.DuplicateException)
    {
    }

    public DuplicateBookException(string message) : base(message)
    {
    }

    public DuplicateBookException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
