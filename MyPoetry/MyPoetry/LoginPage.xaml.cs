using Microsoft.WindowsAzure.MobileServices;
using MyPoetry.Model;
using MyPoetry.UserControls;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

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

        private async void BtnLogin_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Exception exception = null;
            HalfPageMessage hpm = new HalfPageMessage(GrdParent);
            try
            {
                // Shows loading message
                var loader = new ResourceLoader();
                hpm.ShowMessage(loader.GetString("LoginInProgress"), loader.GetString("ServerConnection"), true, false, false, null, null);

                // Sign-in and set the returned user on the context,
                // then load data from the mobile service.
                App.MobileService.CurrentUser = await AuthenticateAsync(TxbEmail.Text, PbPassword.Password);
            }
            catch (MobileServiceInvalidOperationException ex)
            {
                exception = ex;
            }
            finally
            {
                hpm.Dismiss();
                if (exception != null)
                {
                    var msg = new MessageDialog(ServerErrorInfo.Instance.GetInfo(exception.Message));
                    await msg.ShowAsync();
                }
            }
        }

        private void CbStayLogged_Checked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            settings.SetSalt(null);
        }

        private void CbStayLogged_Unchecked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
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
        
        private void BtnRegister_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(RegisterPage));
        }

        private void BtnForgetPassword_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ForgetPasswordPage));
        }
    }
}
