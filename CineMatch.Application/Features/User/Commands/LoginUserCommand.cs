using CineMatch.Application.Common.Exceptions;
using CineMatch.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CineMatch.Application.Features.User.Commands;

public record LoginUserCommand(string Username, string Password) : IRequest<string>;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, string>
{
    private readonly IApplicationDbContext _context;
    private readonly ITokenService _tokenService;

    public LoginUserCommandHandler(IApplicationDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Username == request.Username,
            cancellationToken);

        if (user == null) throw new NotFoundException("Пользователь не найден");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new AuthException("Неправильный пароль");

        _tokenService.SetRefreshToken(user, _tokenService.GenerateRefreshToken());
        await _context.SaveChangesAsync(cancellationToken);
        return _tokenService.CreateToken(user.Username);
    }
}