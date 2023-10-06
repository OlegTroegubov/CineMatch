using CineMatch.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CineMatch.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<Movie> Movies { get; }
    public DbSet<Genre> Genres { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}