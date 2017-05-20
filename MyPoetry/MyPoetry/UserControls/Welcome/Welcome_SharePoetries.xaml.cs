using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace MyPoetry.UserControls
{
    public sealed partial class Welcome_SharePoetries : UserControl
    {
        public Welcome_SharePoetries()
        {
            this.InitializeComponent();
        }

        public void StartAnimation()
        {
            ((Storyboard)this.Resources["Storyboard"]).Begin();
        }

        public void Reset()
        {
            ImgCloud.Opacity = 0;
        }
    }
}
