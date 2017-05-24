using Microsoft.WindowsAzure.MobileServices;
using MyPoetry.Model;
using MyPoetry.UserControls;
using MyPoetry.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MyPoetry
{
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
                await AuthenticateAsync(TxbEmail.Text, PbPassword.Password);
            }
            catch (MobileServiceInvalidOperationException ex)
            {
                exception = ex;
            }
            finally
            {
                if (exception != null)
                {
                    hpm.Dismiss();
                    var msg = new MessageDialog(ServerErrorInfo.Instance.GetInfo(exception.Message));
                    await msg.ShowAsync();
                }
                else
                {
                    // Gets and saves the logged user 
                    List<User> res = await App.MobileService.GetTable<User>().Where(user => user.Email == TxbEmail.Text).ToListAsync();
                    UserHandler.Instance.SetUser(res.First());

                    // Handles keeped login
                    if (CbStayLogged.IsChecked.HasValue && CbStayLogged.IsChecked.Value)
                        settings.SetUserLoggedId(UserHandler.Instance.GetUser().Id);

                    // Updates number of accesses
                    UserHandler.Instance.GetUser().AccessesNumber++;
                    await App.MobileService.GetTable<User>().UpdateAsync(UserHandler.Instance.GetUser());
                    hpm.Dismiss();

                    if (!UserHandler.Instance.GetUser().IsActivated)
                        this.Frame.Navigate(typeof(ActivationPage));
                    else
                        this.Frame.Navigate(typeof(MainPage));
                }
            }
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
