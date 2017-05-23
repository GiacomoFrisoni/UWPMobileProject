using Windows.UI.Xaml;
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


        public Style ItemStyle 
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
