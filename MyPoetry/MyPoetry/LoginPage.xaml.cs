using Microsoft.WindowsAzure.MobileServices;
using MyPoetry.Model;
using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// Il modello di elemento Pagina vuota è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=234238

namespace MyPoetry
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        private AppLocalSettings settings;

        public LoginPage()
        {
            this.InitializeComponent();

            settings = new AppLocalSettings();

            // Cache the page
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        private async void btnLogin_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Exception exception = null;
            try
            {
                // Sign-in and set the returned user on the context,
                // then load data from the mobile service.
                App.MobileService.CurrentUser = await AuthenticateAsync(txbEmail.Text, pbPassword.Password);
            }
            catch (MobileServiceInvalidOperationException ex)
            {
                exception = ex;
            }
            finally
            {
                if (exception != null)
                {
                    var msg = new MessageDialog(exception.Message);
                    await msg.ShowAsync();
                }
                //this.LoginProgress.IsActive = false;
            }
        }

        private void cbStayLogged_Checked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            settings.SetSalt(null);
        }

        private void cbStayLogged_Unchecked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            settings.SetSalt(null);
        }

        private async Task<MobileServiceUser> AuthenticateAsync(string email, string password)
        {
            // Call the CustomLogin API and set the returned MobileServiceUser
            // as the current user.
            var user = await App.MobileService
                .InvokeApiAsync<LoginRequest, MobileServiceUser>(
                "CustomLogin", new LoginRequest()
                {
                    Email = email,
                    Password = password
                });

            return user;
        }

        private void btnRegister_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(RegisterPage));
        }
    }
}
