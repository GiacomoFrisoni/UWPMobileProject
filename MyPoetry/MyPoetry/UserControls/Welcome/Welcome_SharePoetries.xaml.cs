using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace MyPoetry.UserControls
{
    /// <summary>
    /// This class handles the welcome control dedicated to poetry sharing.
    /// </summary>
    public sealed partial class Welcome_SharePoetries : UserControl, IWelcomeControl
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
