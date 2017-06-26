using MyPoetry.Model;
using MyPoetry.Utilities;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MyPoetry.UserControls.Pages
{
    public sealed partial class Home : UserControl
    {
        public Home()
        {
            this.InitializeComponent();
        }

        public CustomPage GetPage { get { return MainContent; } }
        
        private async void PoetriesListView_Loaded(object sender, RoutedEventArgs e)
        {
            ProgressBarVisible(true);

            if (UserHandler.Instance.GetPoetries() == null)
            {
                List<Poetry> poetries = await App.MobileService.GetTable<Poetry>()
                    .Where(p => p.UserId == UserHandler.Instance.GetUser().Id)
                    .ToListAsync();
                UserHandler.Instance.SetPoetries(poetries);
                PoetriesListView.ItemsSource = poetries;
            }
            else
            {
                PoetriesListView.ItemsSource = null;
                PoetriesListView.ItemsSource = UserHandler.Instance.GetPoetries();
            }

            ProgressBarVisible(false);
        }

        private void ProgressBarVisible(bool visible)
        {
            ProgressRingPoetries.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            ProgressRingPoetries.IsActive = visible;
        }
    }
}
