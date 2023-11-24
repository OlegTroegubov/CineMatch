using CineMatch.Application.Common.Exceptions.User;
using CineMatch.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CineMatch.Application.Features.User.Commands;

public record LogoutUserCommand : IRequest;

public class LogoutUserCommandHandler : IRequestHandler<LogoutUserCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LogoutUserCommandHandler(IApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Unit> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
    {
        if (_httpContextAccessor.HttpContext != null)
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];
            var user = await _context.Users.Include(user => user.RefreshToken)
                .FirstOrDefaultAsync(user => user.RefreshToken.Token == token, cancellationToken);
        
            if (user == null)
            {
                throw new NotFoundException("Пользователь не найден");
            }
            user.RefreshToken = null;
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("refreshToken");
            await _context.SaveChangesAsync(cancellationToken);
        }
        return Unit.Value;
    }
}