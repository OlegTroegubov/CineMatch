namespace CineMatch.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public RefreshToken RefreshToken { get; set; }
    public List<Movie> LikedMovies { get; }
}