using CineMatch.Application.Common.Exceptions.Auth;
using CineMatch.Application.Common.Exceptions.User;
using CineMatch.Application.Common.Interfaces;
using CineMatch.Application.Features.Token;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CineMatch.Application.Features.User.Commands;

public record LoginUserCommand(string Username, string Password) : IRequest<TokensDto>;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, TokensDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ITokenService _tokenService;

    public LoginUserCommandHandler(IApplicationDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<TokensDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Username == request.Username,
            cancellationToken);

        if (user == null) throw new NotFoundException("Пользователь не найден");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new AuthException("Неправильный пароль");

        var accessToken = _tokenService.CreateToken(user.Username);
        var refreshToken = _tokenService.GenerateRefreshToken();
        
        _tokenService.SetRefreshToken(user, refreshToken);
        await _context.SaveChangesAsync(cancellationToken);
        return (new TokensDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token
        });
    }
}