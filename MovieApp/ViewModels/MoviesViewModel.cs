using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MovieApp.Models;
using System.Collections.Generic;
using MovieApp.Services;
using System.Threading.Tasks;
using MovieApp.Helpers;

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

        public ICommand LoadMoviesCommand { get; }
        public ICommand ChangeSortCommand { get; }

        public MoviesViewModel()
        {
            Movies = new ObservableCollection<Movie>();
            LoadMoviesCommand = new RelayCommand(async load => await LoadMoviesAsync());
            ChangeSortCommand = new RelayCommand(param => ChangeSort(param?.ToString()));
        }

        private async Task LoadMoviesAsync()
        {
            IsLoading = true;
            try
            {
                var movies = await _apiService.GetPopularMoviesAsync();
                Movies = new ObservableCollection<Movie>(movies);
                SortMovies();
            }
            catch
            {
                // TODO: обработка ошибок (например, показать сообщение)
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
} 