using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace MyPoetry.UserControls.Menu
{
    /// <summary>
    /// This class represents an item inside the hamburger menu.
    /// </summary>
    public class MenuItem
    {
        public enum Groups
        {
            User,
            Home,
            Create,
            Explore,
            Settings
        }

        public string ItemText { get; set; }

        public Symbol ItemIcon { get; set; }

        public ImageSource ItemImage { get; set; }

        public Groups Group { get; set; }

        public CustomPage ItemPage {get; set;}
    }
}
