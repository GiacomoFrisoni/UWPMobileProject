using MyPoetry.Utilities;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MyPoetry
{
    /// <summary>
    /// This class handles the page displayed when there is not Internet connection.
    /// </summary>
    public sealed partial class NoConnectionPage : Page
    {
        public NoConnectionPage()
        {
            this.InitializeComponent();
        }

        private void BtnReconnect_Click(object sender, RoutedEventArgs e)
        {
            BtnReconnect.Visibility = Visibility.Collapsed;
            PrgRing.Visibility = Visibility.Visible;

            if (Connection.HasInternetAccess)
            {
                // Check for the source page
                int? menuIndex = MenuHandler.Instance.GetMenuIndex();
                if (menuIndex == null)
                {
                    this.Frame.Navigate(typeof(LoginPage));
                }
                else
                {
                    this.Frame.Navigate(typeof(MainPage));
                }
            }
            else
            {
                BtnReconnect.Visibility = Visibility.Visible;
                PrgRing.Visibility = Visibility.Collapsed;
                TxbError.Visibility = Visibility.Visible;
            }
        }
    }
}
