using Windows.UI.Xaml.Controls;

namespace MyPoetry.UserControls
{
    /// <summary>
    /// This class models a custom page with a title, a main content and
    /// a section with top buttons.
    /// </summary>
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
