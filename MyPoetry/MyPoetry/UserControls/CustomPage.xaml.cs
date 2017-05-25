using Windows.UI.Xaml.Controls;


// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace MyPoetry.UserControls
{
    public sealed partial class CustomPage : UserControl
    {
        public CustomPage()
        {
            this.InitializeComponent();
        }


        public string Title
        {
            get { return TxblTitle.Text; }
            set { TxblTitle.Text = value; }
        }

        public StackPanel TopButtons
        {
            get { return (StackPanel)CtpTopButtons.Content; }
            set { CtpTopButtons.Content = value; }
        }

        public Grid MainContent
        {
            get { return (Grid)CtpContent.Content; }
            set { CtpContent.Content = value; }
        }

    }
}
