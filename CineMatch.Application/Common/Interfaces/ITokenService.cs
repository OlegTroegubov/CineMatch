using CineMatch.Application.Features.Token;
using CineMatch.Domain.Entities;

namespace CineMatch.Application.Common.Interfaces;

public interface ITokenService
{
    void SetRefreshToken(User user, RefreshToken refreshToken);
    Task<TokensDto> RefreshTokenAsync(CancellationToken cancellationToken);
    RefreshToken GenerateRefreshToken();
    string CreateToken(string username);
}