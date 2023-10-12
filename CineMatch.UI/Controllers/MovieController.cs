using CineMatch.Application.Features.Genre;
using CineMatch.Application.Features.Movie.Dtos;
using CineMatch.Application.Features.Movie.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CineMatch.UI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovieController : ControllerBase
{
    private readonly IMediator _mediator;

    public MovieController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("getMovies")]
    public async Task<List<MovieDto>> Get(int startReleaseYear, int endReleaseYear, string genres, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new GetMoviesQuery(startReleaseYear, endReleaseYear, genres), cancellationToken);
    }
}