using CineMatch.Domain.Entities;

namespace CineMatch.Application.Common.Interfaces;

public interface ITokenService
{
    void SetRefreshToken(User user, RefreshToken refreshToken);
    Task<string> RefreshToken();
    RefreshToken GenerateRefreshToken();
    string CreateToken(string username);
}