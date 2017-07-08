using System;
using Windows.UI.Xaml.Controls;

namespace MyPoetry.UserControls
{
    public sealed partial class PoetryViewer : UserControl
    {
        public PoetryViewer()
        {
            this.InitializeComponent();

            RatingControl.FilledImage = new Uri("ms-appx:///Assets/Rating/staron.png");
            RatingControl.EmptyImage = new Uri("ms-appx:///Assets/Rating/staroff.png");
        }


        private void BtnShare_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }

        private void BtnEdit_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }

        private void BtnDetails_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            SplitView.IsPaneOpen = !SplitView.IsPaneOpen;
        }

        private void BtnDelete_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }
    }
}
