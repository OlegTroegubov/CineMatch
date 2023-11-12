using CineMatch.Application.Common.Interfaces;
using MediatR;

namespace CineMatch.Application.Features.Token;

public record RefreshTokenCommand : IRequest<TokensDto>;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokensDto>
{
    private readonly ITokenService _tokenService;

    public RefreshTokenCommandHandler(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public async Task<TokensDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        return await _tokenService.RefreshTokenAsync(cancellationToken);
    }
}