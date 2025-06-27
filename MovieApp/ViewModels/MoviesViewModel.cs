using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MovieApp.Models;
using System.Collections.Generic;
using MovieApp.Services;
using System.Threading.Tasks;
using MovieApp.Helpers;
using System.Linq;
using System;

namespace MovieApp.ViewModels
{
    public class MoviesViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Movie> _movies;
        public ObservableCollection<Movie> Movies
        {
            get => _movies;
            set { _movies = value; OnPropertyChanged(); }
        }

        private string _sortBy = "Title";
        public string SortBy
        {
            get => _sortBy;
            set { _sortBy = value; OnPropertyChanged(); SortMovies(); }
        }

        private bool _sortAscending = true;
        public bool SortAscending
        {
            get => _sortAscending;
            set { _sortAscending = value; OnPropertyChanged(); SortMovies(); }
        }

        private KinopoiskApiService _apiService = new KinopoiskApiService();
        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
        }

        private int _currentPage = 1;
        public int CurrentPage
        {
            get => _currentPage;
            set { _currentPage = value; OnPropertyChanged(); }
        }
        private int _totalPages = 1;
        public int TotalPages
        {
            get => _totalPages;
            set { _totalPages = value; OnPropertyChanged(); }
        }

        private FavoritesService _favoritesService;

        public FavoritesService FavoritesService => _favoritesService;

        public ICommand LoadMoviesCommand { get; }
        public ICommand ChangeSortCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand PrevPageCommand { get; }
        public ICommand AddToFavoritesCommand { get; }
        public ICommand RemoveFromFavoritesCommand { get; }
        public ICommand OpenMovieDetailsCommand { get; private set; }

        private Action<Movie> _openMovieDetailsAction;
        public void SetOpenMovieDetailsAction(Action<Movie> action)
        {
            _openMovieDetailsAction = action;
        }

        public MoviesViewModel(FavoritesService favoritesService)
        {
            _favoritesService = favoritesService;
            Movies = new ObservableCollection<Movie>();
            LoadMoviesCommand = new RelayCommand(async _ => await LoadMoviesAsync());
            ChangeSortCommand = new RelayCommand(param => ChangeSort(param?.ToString()));
            NextPageCommand = new RelayCommand(async _ => await GoToPage(CurrentPage + 1), _ => CurrentPage < TotalPages && !IsLoading);
            PrevPageCommand = new RelayCommand(async _ => await GoToPage(CurrentPage - 1), _ => CurrentPage > 1 && !IsLoading);
            AddToFavoritesCommand = new RelayCommand(m => AddToFavorites(m as Movie));
            RemoveFromFavoritesCommand = new RelayCommand(m => RemoveFromFavorites(m as Movie));
            OpenMovieDetailsCommand = new RelayCommand(m => _openMovieDetailsAction?.Invoke(m as Movie));
        }

        private async Task GoToPage(int page)
        {
            if (page < 1 || page > TotalPages) return;
            CurrentPage = page;
            await LoadMoviesAsync();
        }

        private async Task LoadMoviesAsync()
        {
            IsLoading = true;
            try
            {
                var result = await _apiService.GetPopularMoviesAsync(CurrentPage);
                Movies = new ObservableCollection<Movie>(result.Movies);
                TotalPages = result.TotalPages;
                SortMovies();
            }
            catch
            {
                // TODO: обработка ошибок
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void ChangeSort(string sortBy)
        {
            if (SortBy == sortBy)
                SortAscending = !SortAscending;
            else
                SortBy = sortBy;
        }

        private void SortMovies()
        {
            if (Movies == null) return;
            List<Movie> sorted;
            switch (SortBy)
            {
                case "Year":
                    sorted = SortAscending ? new List<Movie>(Movies.OrderBy(m => m.Year)) : new List<Movie>(Movies.OrderByDescending(m => m.Year));
                    break;
                case "Genre":
                    sorted = SortAscending ? new List<Movie>(Movies.OrderBy(m => m.Genres.FirstOrDefault())) : new List<Movie>(Movies.OrderByDescending(m => m.Genres.FirstOrDefault()));
                    break;
                default:
                    sorted = SortAscending ? new List<Movie>(Movies.OrderBy(m => m.Title)) : new List<Movie>(Movies.OrderByDescending(m => m.Title));
                    break;
            }
            Movies = new ObservableCollection<Movie>(sorted);
        }

        public bool IsFavorite(Movie movie) => movie != null && _favoritesService.IsFavorite(movie.Id);

        private void AddToFavorites(Movie movie)
        {
            if (movie == null) return;
            _favoritesService.Add(movie);
            Movies = new ObservableCollection<Movie>(Movies);
        }

        private void RemoveFromFavorites(Movie movie)
        {
            if (movie == null) return;
            _favoritesService.Remove(movie.Id);
            Movies = new ObservableCollection<Movie>(Movies);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
} 