using CineMatch.Application.Features.Genre;
using CineMatch.Application.Features.Movie.Dtos;
using MediatR;
using Newtonsoft.Json;

namespace CineMatch.Application.Features.Movie.Queries;

//Надо будет добавить в query поиск по годам, жанрам, и по топ 10 или 250
public record GetMovieQuery : IRequest<MovieDto>;

public class GetMovieQueryHandler : IRequestHandler<GetMovieQuery, MovieDto>
{
    //апи ключ, если мой закончится надо взять свой и добавить сюда
    private readonly string _apiKey = "PDA2HPM-7ZA493S-GXGC0W4-HRSD32W";
    
    //статическая переменная хранит одно и то же значение для всех экземпляров хэндлера
    //(поэтому мы получаем каждый раз новый фильм)
    private static int _numberPage = 1;
    
    public async Task<MovieDto> Handle(GetMovieQuery request, CancellationToken cancellationToken)
    {
        using (var httpClient = new HttpClient())
        {
            //Добавляем заголовки
            httpClient.DefaultRequestHeaders.Add("X-API-KEY", _apiKey);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            
            var apiUrl = $"https://api.kinopoisk.dev/v1.3/movie?" +
                         $"selectFields=id" +
                         $"&selectFields=name" +
                         $"&selectFields=shortDescription" +
                         $"&selectFields=genres" +
                         $"&selectFields=poster" +
                         $"&selectFields=rating.imdb" +
                         $"&selectFields=year" +
                         $"&page={_numberPage}" + 
                         $"&limit=1" +
                         $"&poster.url=%21null ";
                    
            var response = await httpClient.GetAsync(apiUrl, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Ошибка с работой api кинопоиска");
            }
            
            return await GetMovieFromResponse(cancellationToken, response);
        }
    }

    /// <summary>
    /// Метод для получения фильма с помощью запроса
    /// </summary>
    /// <param name="cancellationToken">Токен для отмены запроса</param>
    /// <param name="response">Ответ с запроса</param>
    /// <returns>Фильм</returns>
    private static async Task<MovieDto> GetMovieFromResponse(CancellationToken cancellationToken, HttpResponseMessage response)
    {
        string jsonResponse = await response.Content.ReadAsStringAsync(cancellationToken);

        var jsonObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
        var movieData = jsonObject.docs[0];
        if (_numberPage < Convert.ToInt32(jsonObject.pages))
        {
            _numberPage++;
        }

        var genreDtoList = new List<GenreDto>();

        foreach (var genre in movieData.genres)
        {
            genreDtoList.Add(new GenreDto
            {
                Title = genre.name
            });
        }

        var movieDto = new MovieDto
        {
            Id = movieData.id,
            Title = movieData.name,
            ShortDescription = movieData.shortDescription,
            ReleaseYear = movieData.year,
            PosterUrl = movieData.poster.url,
            Rating = movieData.rating.imdb,
            Genres = genreDtoList
        };

        return movieDto;
    }
}