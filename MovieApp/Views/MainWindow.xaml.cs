using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MovieApp.Models;
using MovieApp.Services;
using MovieApp.ViewModels;
using MovieApp.Views;

namespace MovieApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UserControl _mainPage;
        private UserControl _favoritesPage;
        private UserControl _detailsPage;
        private MoviesViewModel _mainMoviesViewModel;
        private FavoritesService _favoritesService;
        private Movie _lastSelectedMovie;
        private FavoriteMoviesViewModel _favoriteMoviesViewModel;

        public MainWindow()
        {
            InitializeComponent();
            _favoritesService = new FavoritesService();
            _mainMoviesViewModel = new MoviesViewModel(_favoritesService);
            _mainMoviesViewModel.SetOpenMovieDetailsAction(OpenMovieDetails);
            _mainPage = new MainMoviesView { DataContext = _mainMoviesViewModel };
            _favoriteMoviesViewModel = new FavoriteMoviesViewModel(_favoritesService);
            _favoriteMoviesViewModel.SetOpenMovieDetailsAction(OpenMovieDetails);
            _favoritesPage = new FavoriteMoviesView { DataContext = _favoriteMoviesViewModel };
            MainContent.Content = _mainPage;
            _mainMoviesViewModel.LoadMoviesCommand.Execute(null);
        }

        private void MainPageButton_Click(object sender, RoutedEventArgs e)
        {
            _mainPage = new MainMoviesView { DataContext = _mainMoviesViewModel };
            MainContent.Content = _mainPage;
            _mainMoviesViewModel.LoadMoviesCommand.Execute(null);
        }

        private void FavoritesPageButton_Click(object sender, RoutedEventArgs e)
        {
            _favoritesPage = new FavoriteMoviesView { DataContext = _favoriteMoviesViewModel };
            _favoriteMoviesViewModel.Refresh();
            MainContent.Content = _favoritesPage;
        }

        private void OpenMovieDetails(Movie movie)
        {
            _lastSelectedMovie = movie;
            var detailsVm = new MovieDetailsViewModel(movie, _favoritesService, GoBackFromDetails);
            _detailsPage = new MovieDetailsView { DataContext = detailsVm };
            MainContent.Content = _detailsPage;
        }

        private void GoBackFromDetails()
        {
            MainContent.Content = _mainPage;
            _mainMoviesViewModel.LoadMoviesCommand.Execute(null);
        }
    }
}