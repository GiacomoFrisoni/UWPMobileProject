using System;
using Windows.UI.Xaml.Data;

namespace MyPoetry.Converters
{
    public class RatingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int num;
            string s = value.ToString();
            bool res = int.TryParse(s, out num);
            if (res == true)
            {
                // String is a number.
                if (num <= 5 && num >= 1)
                {
                    string ret = "";
                    for (int i = 0; i < num; i++)
                        ret += "\u2605";
                    return ret;
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
