using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using MovieApp.Models;

namespace MovieApp.Services
{
    public class FavoritesService
    {
        private readonly string _filePath;
        private List<Movie> _favorites;

        public FavoritesService()
        {
            var folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
            _filePath = Path.Combine(folder, "MovieApp_favorites.json");
            Load();
        }

        public List<Movie> GetAll() => _favorites.ToList();

        public bool IsFavorite(int movieId) => _favorites.Any(m => m.Id == movieId);

        public void Add(Movie movie)
        {
            if (!_favorites.Any(m => m.Id == movie.Id))
            {
                _favorites.Add(movie);
                Save();
            }
        }

        public void Remove(int movieId)
        {
            var movie = _favorites.FirstOrDefault(m => m.Id == movieId);
            if (movie != null)
            {
                _favorites.Remove(movie);
                Save();
            }
        }

        private void Save()
        {
            var json = JsonSerializer.Serialize(_favorites);
            File.WriteAllText(_filePath, json);
        }

        private void Load()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                _favorites = JsonSerializer.Deserialize<List<Movie>>(json) ?? new List<Movie>();
            }
            else
            {
                _favorites = new List<Movie>();
            }
        }
    }
} 