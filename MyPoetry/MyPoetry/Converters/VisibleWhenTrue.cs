using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MyPoetry.Converters
{
    class VisibleWhenTrue : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (System.Convert.ToBoolean(value))
                return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
