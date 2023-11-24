using CineMatch.Application.Common.Interfaces;
using CineMatch.Application.Features.Genre;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CineMatch.Application.Features.User.Commands;

public record LikeMovieCommand (int Id, string Title, float Rating, string Description, int ReleaseYear, string PosterUrl, List<GenreDto> Genres) : IRequest;

public class LikeMovieCommandHandler : IRequestHandler<LikeMovieCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LikeMovieCommandHandler(IApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Unit> Handle(LikeMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = await _context.Movies.AddAsync(new Domain.Entities.Movie
        {
            Id = request.Id,
            Title = request.Title,
            Rating = request.Rating,
            Description = request.Description,
            ReleaseYear = request.ReleaseYear,
            PosterUrl = request.PosterUrl,
            Genres = GenreMapper.MapToListGenre(request.Genres)
        }, cancellationToken);

        if (_httpContextAccessor.HttpContext != null)
        {
            var username = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault()?.Value;
            var user = await _context.Users.Include(user => user.LikedMovies)
                .FirstOrDefaultAsync(user => user.Username == username, cancellationToken);
            
            user.LikedMovies.Add(movie.Entity);
        }

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}