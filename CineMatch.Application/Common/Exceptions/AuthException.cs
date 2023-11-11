namespace CineMatch.Application.Common.Exceptions;

public class AuthException : Exception
{
    public AuthException()
    {
    }

    public AuthException(string message) : base(message)
    {
    }
}