using System.Text;
using CineMatch.Application.Features.Genre;
using CineMatch.Application.Features.Movie.Dtos;
using MediatR;
using Newtonsoft.Json;

namespace CineMatch.Application.Features.Movie.Queries;

//Надо будет добавить в query поиск по годам, жанрам, и по топ 10 или 250
public record GetMoviesQuery(int StartReleaseYear, int EndReleaseYear, string Genres) : IRequest<List<MovieDto>>;

public class GetMovieQueryHandler : IRequestHandler<GetMoviesQuery, List<MovieDto>>
{
    //статическая переменная хранит одно и то же значение для всех экземпляров хэндлера
    //(поэтому мы получаем каждый раз новый фильм)
    private static int _numberPage = 1;

    //апи ключ, если мой закончится надо взять свой и добавить сюда
    private readonly string _apiKey = "PDA2HPM-7ZA493S-GXGC0W4-HRSD32W";

    public async Task<List<MovieDto>> Handle(GetMoviesQuery request, CancellationToken cancellationToken)
    {
        using (var httpClient = new HttpClient())
        {
            //Добавляем заголовки
            httpClient.DefaultRequestHeaders.Add("X-API-KEY", _apiKey);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            var apiUrl = new StringBuilder("https://api.kinopoisk.dev/v1.3/movie?" +
                                           "selectFields=id" +
                                           "&selectFields=name" +
                                           "&selectFields=shortDescription" +
                                           "&selectFields=genres" +
                                           "&selectFields=poster" +
                                           "&selectFields=rating.imdb" +
                                           "&selectFields=year" +
                                           $"&page={_numberPage}" +
                                           "&limit=10");
                
            AppendYearRangeToUrl(request, apiUrl);
            AppendGenresToUrl(request, apiUrl);
            
            var response = await httpClient.GetAsync(apiUrl.ToString(), cancellationToken);

            if (!response.IsSuccessStatusCode) throw new HttpRequestException("Ошибка с работой api кинопоиска");

            return await GetMoviesFromResponse(cancellationToken, response);
        }
    }
    
    /// <summary>
    /// Добавляет жарны в запрос
    /// </summary>
    /// <param name="request"></param>
    /// <param name="apiUrl"></param>
    private static void AppendGenresToUrl(GetMoviesQuery request, StringBuilder apiUrl)
    {
        if (request.Genres != null)
        {
            string[] genres = request.Genres.Split(',');
            foreach (var genre in genres)
            {
                apiUrl.Append($"&genres.name={Uri.EscapeDataString(genre)}");
            }
        }
    }
    /// <summary>
    /// Добавляет в запрос годовой промежуток выхода фильмов
    /// </summary>
    /// <param name="request"></param>
    /// <param name="apiUrl"></param>
    private static void AppendYearRangeToUrl(GetMoviesQuery request, StringBuilder apiUrl)
    {
        var yearRange = (request.StartReleaseYear != 0 && request.EndReleaseYear != 0)
            ? $"{request.StartReleaseYear}-{request.EndReleaseYear}"
            : (request.StartReleaseYear != 0)
                ? request.StartReleaseYear.ToString()
                : (request.EndReleaseYear != 0)
                    ? request.EndReleaseYear.ToString()
                    : "";

        if (!string.IsNullOrEmpty(yearRange))
        {
            apiUrl.Append($"&year={yearRange}");
        }
    }
    
    /// <summary>
    ///     Метод для получения фильма с помощью запроса
    /// </summary>
    /// <param name="cancellationToken">Токен для отмены запроса</param>
    /// <param name="response">Ответ с запроса</param>
    /// <returns>Фильм</returns>
    private static async Task<List<MovieDto>> GetMoviesFromResponse(CancellationToken cancellationToken, HttpResponseMessage response)
    {
        var jsonResponse = await response.Content.ReadAsStringAsync(cancellationToken);

        var jsonObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
        var docs = jsonObject.docs;

        var movieDtos = new List<MovieDto>();

        foreach (var movieData in docs)
        {
            var genreDtoList = new List<GenreDto>();

            foreach (var genre in movieData.genres)
                genreDtoList.Add(new GenreDto
                {
                    Title = genre.name
                });

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

            movieDtos.Add(movieDto);
        }

        return movieDtos;
    }
}