using Windows.UI.Xaml.Controls;

// Il modello di elemento Pagina vuota è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=234238

namespace MyPoetry
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        AppLocalSettings settings;

        public LoginPage()
        {
            this.InitializeComponent();
        }

        private void btnLogin_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }

        private void cbStayLogged_Checked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            settings.setSalt(null);
        }

        private void cbStayLogged_Unchecked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            settings.setSalt(null);
        }
    }
}
