using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace MyPoetry.UserControls.Menu
{
    public sealed partial class MenuItemViewer : UserControl
    {
        public MenuItemViewer()
        {
            this.InitializeComponent();
        }

        public string ItemText
        {
            get
            {
                return Text.Text;
            }
            set
            {
                Text.Text = value;
            }
        }

        public Symbol ItemIcon
        {
            get
            {
                return Icon.Symbol;
            }
            set
            {
                Icon.Symbol = value;
            }
        }
<<<<<<< HEAD:MyPoetry/MyPoetry/UserControls/Menu/MenuItemViewer.xaml.cs

        public Style ItemStyle
=======
        
        public Brush ItemBackground
>>>>>>> origin/master:MyPoetry/MyPoetry/UserControls/Menu/MenuItem.xaml.cs
        {
            get
            {
                return Container.Style;
            }
            set
            {
                Container.Style = value;
            }
        }
    }
}
