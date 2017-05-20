using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace MyPoetry.UserControls
{
    public sealed partial class Welcome_ReadPoetries : UserControl
    {
        public Welcome_ReadPoetries()
        {
            this.InitializeComponent();
        }

        public void StartAnimation()
        {
            ((Storyboard)this.Resources["Storyboard"]).Begin();
        }

        public void Reset()
        {
            ImgBook.Opacity = 0;
        }
    }
}
