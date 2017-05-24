using Microsoft.WindowsAzure.MobileServices;
using MyPoetry.Model;
using MyPoetry.UserControls;
using MyPoetry.Utilities;
using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MyPoetry
{
    public sealed partial class ActivationPage : Page
    {
        public ActivationPage()
        {
            this.InitializeComponent();
        }

        private async void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            Exception exception = null;
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            HalfPageMessage hpm = new HalfPageMessage(GrdParent);
            try
            {
                hpm.ShowMessage(loader.GetString("Checking"), loader.GetString("CheckingText"), true, false, false, null, null);
                await ActivateAsync(UserHandler.Instance.GetUser().Email, TxbCode.Text);
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
                else
                {
                    this.Frame.Navigate(typeof(WelcomePage));
                }
            }
        }

        private async void BtnCodeNotReceived_Click(object sender, RoutedEventArgs e)
        {
            Exception exception = null;
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            HalfPageMessage hpm = new HalfPageMessage(GrdParent);
            try
            {
                hpm.ShowMessage(loader.GetString("CodeGenerationInProgress"), loader.GetString("Wait"), true, false, false, null, null);
                await ReSendingActivationAsync(UserHandler.Instance.GetUser().Email);

                // Shows confirm message
                hpm.IsProgressRingEnabled = false;
                hpm.Title = loader.GetString("EmailSent");
                hpm.Message = loader.GetString("CodeRequestMessage");
                hpm.SetOkAction(null, loader.GetString("Ok"));
                hpm.IsOkButtonEnabled = true;
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
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame != null && rootFrame.CanGoBack)
                rootFrame.GoBack();
        }

        private async Task<MobileServiceUser> ActivateAsync(string email, string code)
        {
            // Call the CustomLogin API and set the returned MobileServiceUser
            // as the current user.
            var user = await App.MobileService
                .InvokeApiAsync<ActivationRequest, MobileServiceUser>(
                "CustomActivation", new ActivationRequest()
                {
                    Email = email,
                    Code = code
                });

            return user;
        }

        private async Task<MobileServiceUser> ReSendingActivationAsync(string email)
        {
            // Call the CustomLogin API and set the returned MobileServiceUser
            // as the current user.
            var user = await App.MobileService
                .InvokeApiAsync<ReSendingActivationRequest, MobileServiceUser>(
                "CustomReSendingActivation", new ReSendingActivationRequest()
                {
                    Email = email
                });

            return user;
        }
    }
}
