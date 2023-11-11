namespace CineMatch.Application.Common.Exceptions;

public class ExistsException : Exception
{
    public ExistsException()
    {
    }

    public ExistsException(string message) : base(message)
    {

    }
}