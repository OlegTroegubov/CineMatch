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
        var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

        if (movie == null)
        {
            movie = new Domain.Entities.Movie
            {
                Id = request.Id,
                Title = request.Title,
                Rating = request.Rating,
                Description = request.Description,
                ReleaseYear = request.ReleaseYear,
                PosterUrl = request.PosterUrl,
                Genres = new List<Domain.Entities.Genre>()
            };

            foreach (var genreDto in request.Genres)
            {
                var existingGenre = await _context.Genres.FirstOrDefaultAsync(genre => genre.Title == genreDto.Title, cancellationToken);

                if (existingGenre != null)
                {
                    movie.Genres.Add(existingGenre);
                }
                else
                {
                    var newGenre = new Domain.Entities.Genre { Title = genreDto.Title };
                    movie.Genres.Add(newGenre);
                }
            }
            
            await _context.Movies.AddAsync(movie, cancellationToken);
        }

        if (_httpContextAccessor.HttpContext != null)
        {
            var username = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault()?.Value;
            var user = await _context.Users.Include(u => u.LikedMovies)
                .FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
            user.LikedMovies.Add(movie);
        }

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}