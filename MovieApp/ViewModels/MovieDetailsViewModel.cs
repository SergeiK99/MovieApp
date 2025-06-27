using MovieApp.Helpers;
using MovieApp.Models;
using MovieApp.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MovieApp.ViewModels
{
    public class MovieDetailsViewModel : INotifyPropertyChanged
    {
        private readonly FavoritesService _favoritesService;
        private readonly Action _goBackAction;
        private Movie _movie;
        public Movie Movie
        {
            get => _movie;
            set { _movie = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsFavorite)); OnPropertyChanged(nameof(FavoriteButtonText)); OnPropertyChanged(nameof(GenresString)); OnPropertyChanged(nameof(ActorsString)); OnPropertyChanged(nameof(RatingString)); }
        }

        public string Title => Movie != null ? Movie.Title : "Нет данных";
        public string PosterUrl => Movie != null ? Movie.PosterUrl : null;
        public int? Year => Movie != null ? Movie.Year : null;
        public string Description => Movie != null ? Movie.Description : "Нет описания";
        public bool IsFavorite => Movie != null && _favoritesService.IsFavorite(Movie.Id);
        public string FavoriteButtonText => IsFavorite ? "Удалить из избранного" : "В избранное";
        public string GenresString => Movie != null && Movie.Genres != null && Movie.Genres.Count > 0 ? $"Жанры: {string.Join(", ", Movie.Genres)}" : string.Empty;
        public string ActorsString => Movie != null && Movie.Actors != null && Movie.Actors.Count > 0 ? $"Актёры: {string.Join(", ", Movie.Actors)}" : string.Empty;
        public string RatingString => Movie != null && Movie.Rating > 0 ? $"Рейтинг: {Movie.Rating:F1}" : string.Empty;

        public ICommand ToggleFavoriteCommand { get; }
        public ICommand GoBackCommand { get; }

        public MovieDetailsViewModel(Movie movie, FavoritesService favoritesService, Action goBackAction)
        {
            _favoritesService = favoritesService;
            _goBackAction = goBackAction;
            Movie = movie;
            ToggleFavoriteCommand = new RelayCommand(_ => ToggleFavorite(), _ => Movie != null);
            GoBackCommand = new RelayCommand(_ => _goBackAction?.Invoke());
        }

        private void ToggleFavorite()
        {
            if (IsFavorite)
                _favoritesService.Remove(Movie.Id);
            else
                _favoritesService.Add(Movie);
            OnPropertyChanged(nameof(IsFavorite));
            OnPropertyChanged(nameof(FavoriteButtonText));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
} 