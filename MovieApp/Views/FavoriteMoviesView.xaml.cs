using System.Windows.Controls;
using MovieApp.ViewModels;

namespace MovieApp.Views
{
    public partial class FavoriteMoviesView : UserControl
    {
        public FavoriteMoviesView()
        {
            InitializeComponent();
            DataContext = new FavoriteMoviesViewModel();
        }
    }
} 