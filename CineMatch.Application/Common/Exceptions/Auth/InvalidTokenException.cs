namespace CineMatch.Application.Common.Exceptions.Auth;

public class InvalidTokenException : Exception
{
    public InvalidTokenException()
    {
    }

    public InvalidTokenException(string message) : base(message)
    {
    }
}