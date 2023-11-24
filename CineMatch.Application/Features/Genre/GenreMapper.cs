namespace CineMatch.Application.Features.Genre;

public class GenreMapper
{
    public static List<Domain.Entities.Genre> MapToListGenre(List<GenreDto> genreDtos)
    {
        return genreDtos.Select(genreDto => new Domain.Entities.Genre { Title = genreDto.Title }).ToList();
    }
}