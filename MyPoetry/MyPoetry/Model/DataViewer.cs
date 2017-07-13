using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace MyPoetry.Model
{
    class DataViewer
    {
        public DataViewer(string description, string value, Symbol icon, Brush paneColor)
        {
            Description = description;
            Value = value;
            Icon = icon;
            PaneColor = paneColor;
        }

        public Symbol Icon { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public Brush PaneColor { get; set; }
    }
}
