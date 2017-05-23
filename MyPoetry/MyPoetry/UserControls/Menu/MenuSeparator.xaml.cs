using Windows.UI.Xaml.Controls;

namespace MyPoetry.UserControls.Menu
{
    public sealed partial class MenuSeparator : UserControl
    {
        public MenuSeparator()
        {
            this.InitializeComponent();
        }


        public string Key
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
    }
}
