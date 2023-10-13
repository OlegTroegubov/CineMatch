namespace CineMatch.Domain.Entities;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; }
    public float Rating { get; set; }
    public string Description { get; set; }
    public DateTime ReleaseYear { get; set; }
    public string PosterUrl { get; set; }
    public List<Genre> Genres { get; set; }
}