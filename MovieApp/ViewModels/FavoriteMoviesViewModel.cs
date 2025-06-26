using System.Collections.ObjectModel;
using System.Windows.Input;
using MovieApp.Models;
using MovieApp.Services;
using MovieApp.Helpers;

namespace MovieApp.ViewModels
{
    public class FavoriteMoviesViewModel
    {
        public ObservableCollection<Movie> FavoriteMovies { get; set; }
        public ICommand RemoveFromFavoritesCommand { get; }
        private FavoritesService _favoritesService = new FavoritesService();

        public FavoriteMoviesViewModel()
        {
            LoadFavorites();
            RemoveFromFavoritesCommand = new RelayCommand(m => RemoveFromFavorites(m as Movie));
        }

        private void LoadFavorites()
        {
            var movies = _favoritesService.GetAll();
            FavoriteMovies = new ObservableCollection<Movie>(movies);
        }

        private void RemoveFromFavorites(Movie movie)
        {
            if (movie == null) return;
            _favoritesService.Remove(movie.Id);
            FavoriteMovies.Remove(movie);
        }
    }
} 