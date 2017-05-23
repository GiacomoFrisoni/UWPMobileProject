using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace MyPoetry.UserControls.Menu
{
    public sealed partial class MenuItem : UserControl
    {

        public MenuItem()
        {
            this.InitializeComponent();
        }


        public MenuItem (string text, Symbol icon)
        {
            this.InitializeComponent();
            this.ItemText = text;
            this.ItemIcon = icon;
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
        
        public Brush ItemBackground
        {
            get
            {
                return Container.Background;
            }
            set
            {
                Container.Background = value;
            }
        }
    }
}
