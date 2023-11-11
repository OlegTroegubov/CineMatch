namespace CineMatch.Application.Common.Exceptions.User;

public class ExistsException : Exception
{
    public ExistsException()
    {
    }

    public ExistsException(string message) : base(message)
    {
    }
}