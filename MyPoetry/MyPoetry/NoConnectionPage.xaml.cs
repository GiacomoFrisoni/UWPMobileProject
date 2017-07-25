using MyPoetry.Utilities;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MyPoetry
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
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
