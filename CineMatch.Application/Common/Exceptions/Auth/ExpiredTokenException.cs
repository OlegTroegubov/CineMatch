namespace CineMatch.Application.Common.Exceptions.Auth;

public class ExpiredTokenException : Exception
{
    public ExpiredTokenException()
    {
    }

    public ExpiredTokenException(string message) : base(message)
    {
    }
}