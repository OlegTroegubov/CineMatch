using CineMatch.Application.Common.Exceptions;
using CineMatch.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CineMatch.Application.Features.User.Commands;

public record RegistrationUserCommand(string Username, string Password) : IRequest;

public class RegistrationUserCommandHandler : IRequestHandler<RegistrationUserCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public RegistrationUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(RegistrationUserCommand request, CancellationToken cancellationToken)
    {
        var alreadyExists = await _context.Users.AnyAsync(user => user.Username == request.Username,
            cancellationToken);

        if (alreadyExists)
        {
            throw new ExistsException("Пользователь с таким логином уже существует");
        }

        await _context.Users.AddAsync(new Domain.Entities.User
        {
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        }, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}