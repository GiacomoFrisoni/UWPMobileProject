using Windows.UI.Xaml.Controls;

namespace MyPoetry.UserControls.Pages
{
    public sealed partial class Home : UserControl
    {
        public Home()
        {
            this.InitializeComponent();
        }

        /*
        public CustomPage GetPage { get { return MainContent; } }
        
        private async void PoetriesListView_Loaded(object sender, RoutedEventArgs e)
        {
            ProgressBarVisible(true);

            if (UserHandler.Instance.GetPoetries() == null)
            {
                List<Poetry> poetries = await App.MobileService.GetTable<Poetry>()
                    .Where(p => p.UserId == UserHandler.Instance.GetUser().Id)
                    .OrderByDescending(poetry => poetry.CreationDate)
                    .ToListAsync();
                UserHandler.Instance.SetPoetries(poetries);
                PoetriesListView.ItemsSource = poetries;

                //var groups = from c in UserHandler.Instance.GetPoetries() group c by c.CreationDate.Month;
                //PoetriesListView.ItemsSource = groups;
            }
            else
            {
                PoetriesListView.ItemsSource = null;
                //var groups = from c in UserHandler.Instance.GetPoetries() group c by c.CreationDate.Month;
                //PoetriesListView.ItemsSource = groups;
                PoetriesListView.ItemsSource = UserHandler.Instance.GetPoetries();
            }

            ProgressBarVisible(false);
        }

        private void ProgressBarVisible(bool visible)
        {
            ProgressRingPoetries.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            ProgressRingPoetries.IsActive = visible;
        }

        private void PoetryWidget_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            MenuHandler.Instance.SetMenuIndex(2);
        }


        private async void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            ProgressBarVisible(true);

            if (UserHandler.Instance.GetPoetries() == null)
            {
                List<Poetry> poetries = await App.MobileService.GetTable<Poetry>()
                    .Where(p => p.UserId == UserHandler.Instance.GetUser().Id &&
                                (
                                 p.Title.ToLower().Contains(SearchBox.Text) ||
                                 p.Body.Contains(SearchBox.Text)
                                )
                          )
                    .OrderByDescending(poetry => poetry.CreationDate)
                    .ToListAsync();

                PoetriesListView.ItemsSource = poetries;

                //var groups = from c in UserHandler.Instance.GetPoetries() group c by c.CreationDate.Month;
                //PoetriesListView.ItemsSource = groups;
            }
            else
            {
                PoetriesListView.ItemsSource = null;
                //var groups = from c in UserHandler.Instance.GetPoetries() group c by c.CreationDate.Month;
                //PoetriesListView.ItemsSource = groups;
                PoetriesListView.ItemsSource = UserHandler.Instance.GetPoetries().Where(p => p.Title.ToLower().Contains(SearchBox.Text) ||
                                                                                             p.Body.Contains(SearchBox.Text)
                                                                                       )
                                                                                .OrderByDescending(poetry => poetry.CreationDate);
            }

            ProgressBarVisible(false);
        }*/
    }
}
