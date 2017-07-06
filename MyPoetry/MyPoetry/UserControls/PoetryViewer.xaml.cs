using Windows.UI.Xaml.Controls;

namespace MyPoetry.UserControls
{
    public sealed partial class PoetryViewer : UserControl
    {
        public PoetryViewer()
        {
            this.InitializeComponent();
        }


        public string Title { get; set; }
        public string Body { get; set; }
    }
}
