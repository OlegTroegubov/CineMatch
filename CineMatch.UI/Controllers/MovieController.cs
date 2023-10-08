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

    [HttpGet("getMovie")]
    public async Task<MovieDto> Get()
    {
        return await _mediator.Send(new GetMovieQuery());
    }

    [HttpGet("getTestMovie")]
    public MovieDto GetTestMovie()
    {
        return new MovieDto
        {
            Id = 1,
            Title = "test",
            PosterUrl = "localhost",
            Rating = 9.7f,
            ReleaseYear = 2008,
            ShortDescription = "test",
            Genres = new List<GenreDto>
            {
                new() { Title = "test" }
            }
        };
    }
}