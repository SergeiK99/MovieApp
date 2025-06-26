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

        public MainWindow()
        {
            InitializeComponent();
            _mainPage = new MainMoviesView(); // создадим отдельный UserControl для главной страницы
            _favoritesPage = new FavoriteMoviesView();
            MainContent.Content = _mainPage;
        }

        private void MainPageButton_Click(object sender, RoutedEventArgs e)
        {
            _mainPage = new MainMoviesView();
            MainContent.Content = _mainPage;
        }

        private void FavoritesPageButton_Click(object sender, RoutedEventArgs e)
        {
            _favoritesPage = new FavoriteMoviesView();
            MainContent.Content = _favoritesPage;
        }
    }
}