using System.Windows.Controls;
using MovieApp.ViewModels;

namespace MovieApp.Views
{
    public partial class MainMoviesView : UserControl
    {
        public MainMoviesView()
        {
            InitializeComponent();
            var vm = new MoviesViewModel();
            DataContext = vm;
            vm.LoadMoviesCommand.Execute(null);
        }
    }
} 