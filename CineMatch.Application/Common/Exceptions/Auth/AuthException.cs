namespace CineMatch.Application.Common.Exceptions.Auth;

public class AuthException : Exception
{
    public AuthException()
    {
    }

    public AuthException(string message) : base(message)
    {
    }
}