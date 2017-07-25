using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace MyPoetry.UserControls
{
    public sealed partial class Welcome_Start : UserControl, IWelcomeControl
    {
        public Welcome_Start()
        {
            this.InitializeComponent();
        }

        public void StartAnimation()
        {
            ((Storyboard)this.Resources["Storyboard"]).Begin();
        }
        
        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(MainPage));
        }

        public void Reset()
        {
            ImgLogo.Opacity = 0;
            BtnStart.Opacity = 0;
        }
    }
}
