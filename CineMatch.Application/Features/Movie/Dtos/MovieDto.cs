using CineMatch.Application.Features.Genre;

namespace CineMatch.Application.Features.Movie.Dtos;

public class MovieDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public float Rating { get; set; }
    public string ShortDescription { get; set; }
    public int ReleaseYear { get; set; }
    public string PosterUrl { get; set; }
    public List<GenreDto> Genres { get; set; }
}