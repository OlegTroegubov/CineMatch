namespace CineMatch.Application.Common.Exceptions.Auth;

public class TokenException : Exception
{
    public TokenException()
    {
    }

    public TokenException(string message) : base(message)
    {
    }
}