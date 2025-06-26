using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using MovieApp.Models;
using MovieApp.ViewModels;

namespace MovieApp.Views
{
    public class FavoriteButtonTextMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var vm = values[0] as MoviesViewModel;
            var movie = values[1] as Movie;
            if (vm != null && movie != null)
                return vm.IsFavorite(movie) ? "В избранном" : "В избранное";
            return "В избранное";
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class FavoriteButtonVisibilityMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var vm = values[0] as MoviesViewModel;
            var movie = values[1] as Movie;
            if (vm != null && movie != null)
                return vm.IsFavorite(movie) ? Visibility.Collapsed : Visibility.Visible;
            return Visibility.Visible;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class NotFavoriteButtonVisibilityMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var vm = values[0] as MoviesViewModel;
            var movie = values[1] as Movie;
            if (vm != null && movie != null)
                return vm.IsFavorite(movie) ? Visibility.Visible : Visibility.Collapsed;
            return Visibility.Collapsed;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
} 