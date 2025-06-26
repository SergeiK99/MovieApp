using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MovieApp.Models;
using System.Text.Json;
using System.Linq;

namespace MovieApp.Services
{
    public class KinopoiskApiService
    {
        private const string ApiKey = "64e60f8c-806b-4772-94b6-17b21cc4d09c";
        private const string BaseUrl = "https://kinopoiskapiunofficial.tech/api/v2.2/films/collections";
        private readonly HttpClient _httpClient;

        public KinopoiskApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("X-API-KEY", ApiKey);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<Movie>> GetPopularMoviesAsync()
        {
            var url = BaseUrl + "?type=TOP_250_MOVIES&page=1";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine(json);
            var result = JsonSerializer.Deserialize<KinopoiskResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var movies = new List<Movie>();
            System.Diagnostics.Debug.WriteLine($"result.items count: {result?.items?.Count}");
            if (result?.items != null)
            {
                foreach (var f in result.items)
                {
                    movies.Add(new Movie
                    {
                        Id = f.kinopoiskId,
                        Title = f.nameRu ?? f.nameEn ?? f.nameOriginal,
                        Year = f.year ?? 0,
                        PosterUrl = f.posterUrlPreview,
                        Genres = f.genres != null ? f.genres.Select(g => g.genre).ToList() : new List<string>(),
                        Actors = new List<string>() // В этом ответе нет актёров
                    });
                }
            }
            return movies;
        }

        private class KinopoiskResponse
        {
            public List<FilmDto> items { get; set; }
        }
        private class FilmDto
        {
            public int kinopoiskId { get; set; }
            public string nameRu { get; set; }
            public string nameEn { get; set; }
            public string nameOriginal { get; set; }
            public int? year { get; set; }
            public string posterUrlPreview { get; set; }
            public List<GenreDto> genres { get; set; }
            public List<CountryDto> countries { get; set; }
            public string type { get; set; }
            public string description { get; set; }
        }
        private class GenreDto
        {
            public string genre { get; set; }
        }
        private class CountryDto
        {
            public string country { get; set; }
        }
    }
} 