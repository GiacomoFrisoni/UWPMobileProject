using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace MyPoetry.UserControls
{
    /// <summary>
    /// This class handles the welcome control dedicated to poetry reading.
    /// </summary>
    public sealed partial class Welcome_ReadPoetries : UserControl, IWelcomeControl
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
