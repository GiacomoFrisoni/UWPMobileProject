using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace MyPoetry.UserControls
{
    public sealed partial class Welcome_WritePoetry : UserControl, IWelcomeControl
    {
        public Welcome_WritePoetry()
        {
            this.InitializeComponent();
        }

        public void StartAnimation()
        {
            ((Storyboard)this.Resources["Storyboard"]).Begin();
        }

        public void Reset()
        {
            ImgPaper.Opacity = 0;
            ImgPencil.Opacity = 0;
        }
    }
}
