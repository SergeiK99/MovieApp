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
        public ICommand OpenMovieDetailsCommand { get; private set; }
        private FavoritesService _favoritesService;
        private System.Action<Movie> _openMovieDetailsAction;

        public FavoriteMoviesViewModel(FavoritesService favoritesService)
        {
            _favoritesService = favoritesService;
            LoadFavorites();
            RemoveFromFavoritesCommand = new RelayCommand(m => RemoveFromFavorites(m as Movie));
            OpenMovieDetailsCommand = new RelayCommand(m => _openMovieDetailsAction?.Invoke(m as Movie));
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

        public void SetOpenMovieDetailsAction(System.Action<Movie> action)
        {
            _openMovieDetailsAction = action;
        }

        public void Refresh()
        {
            LoadFavorites();
        }
    }
} 