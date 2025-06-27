using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MovieApp.Models;
using MovieApp.DTOs;
using System.Text.Json;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System;

namespace MovieApp.Services
{
    public class KinopoiskApiOptions
    {
        public string ApiKey { get; set; }
        public string BaseUrl { get; set; }
    }

    public partial class KinopoiskApiService
    {
        private readonly string _apiKey;
        private readonly string _baseUrl;
        private readonly HttpClient _httpClient;

        public KinopoiskApiService()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            var options = config.GetSection("KinopoiskApi").Get<KinopoiskApiOptions>();
            _apiKey = options.ApiKey;
            _baseUrl = options.BaseUrl;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("X-API-KEY", _apiKey);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<KinopoiskPageResult> GetPopularMoviesAsync(int page = 1)
        {
            var url = _baseUrl + $"?type=TOP_250_MOVIES&page={page}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<KinopoiskResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var movies = new List<Movie>();
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
                        Description = f.description,
                        Rating = f.ratingKinopoisk ?? 0
                    });
                }
            }
            return new KinopoiskPageResult
            {
                Movies = movies,
                TotalPages = result?.totalPages ?? 1
            };
        }
    }
} 