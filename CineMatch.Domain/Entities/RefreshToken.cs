namespace CineMatch.Domain.Entities;

public class RefreshToken
{
    public int Id { get; set; }
    public required string Token { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime Expired { get; set; }
}