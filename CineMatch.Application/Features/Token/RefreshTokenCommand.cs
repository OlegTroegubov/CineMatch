using CineMatch.Application.Common.Interfaces;
using MediatR;

namespace CineMatch.Application.Features.Token;

public record RefreshTokenCommand : IRequest<string>;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, string>
{
    private readonly ITokenService _tokenService;

    public RefreshTokenCommandHandler(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public async Task<string> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var newAccessToken = await _tokenService.RefreshToken();
        return newAccessToken;
    }
}