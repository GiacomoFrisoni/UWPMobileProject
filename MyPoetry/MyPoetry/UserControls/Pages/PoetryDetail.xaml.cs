using MyPoetry.Model;
using MyPoetry.Utilities;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MyPoetry.UserControls.Pages
{
    public sealed partial class PoetryDetail : UserControl
    {
        public PoetryDetail()
        {
            this.InitializeComponent();

            RatingControl.FilledImage = new Uri("ms-appx:///Assets/Rating/staron.png");
            RatingControl.EmptyImage = new Uri("ms-appx:///Assets/Rating/staroff.png");
        }

        public CustomPage GetPage { get { return MainContent; } }

        private void BtnDetails_Click(object sender, RoutedEventArgs e)
        {
            SplitView.IsPaneOpen = !SplitView.IsPaneOpen;
        }

        private void FlpPoetries_Loaded(object sender, RoutedEventArgs e)
        {
            FlpPoetries.Items.Clear();
            foreach (Poetry p in UserHandler.Instance.GetPoetries()) {
                PoetryViewer pw = new PoetryViewer();
                pw.DataContext = p;
                FlpPoetries.Items.Add(pw);
            }
        }

        private void FlpPoetries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FlpPoetries.SelectedItem != null)
            {
                Poetry p = (Poetry)((PoetryViewer)FlpPoetries.SelectedItem).DataContext;
                RatingControl.Value = p.Rating;
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnEdit_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void BtnShare_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
