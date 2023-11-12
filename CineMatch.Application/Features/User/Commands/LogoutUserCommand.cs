using CineMatch.Application.Common.Exceptions.User;
using CineMatch.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CineMatch.Application.Features.User.Commands;

public record LogoutUserCommand(string Username) : IRequest;

public class LogoutUserCommandHandler : IRequestHandler<LogoutUserCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public LogoutUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.Include(user => user.RefreshToken)
            .FirstOrDefaultAsync(user => user.Username == request.Username, cancellationToken);
        
        if (user == null)
        {
            throw new NotFoundException("Пользователь не найден");
        }
        user.RefreshToken = null;

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}