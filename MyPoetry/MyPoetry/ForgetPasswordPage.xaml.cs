using Microsoft.WindowsAzure.MobileServices;
using MyPoetry.Model;
using MyPoetry.UserControls;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MyPoetry
{
    /// <summary>
    /// This page handles password recovery.
    /// </summary>
    public sealed partial class ForgetPasswordPage : Page
    {
        public ForgetPasswordPage()
        {
            this.InitializeComponent();
        }

        private async void BtnRecover_Click(object sender, RoutedEventArgs e)
        {
            Exception exception = null;
            HalfPageMessage hpm = new HalfPageMessage(GrdParent);
            try
            {
                // Shows loading message
                var loader = new ResourceLoader();
                hpm.ShowMessage(loader.GetString("RecoveryInProgress"), loader.GetString("ServerConnection"), true, false, false, null, null);

                // Sign-in and set the returned user on the context,
                // then load data from the mobile service.
                await RecoveryAsync(TxbEmail.Text);

                // Shows confirm message
                hpm.IsProgressRingEnabled = false;
                hpm.Title = loader.GetString("EmailSent");
                hpm.Message = loader.GetString("PasswordRecoveryMessage");
                hpm.SetOkAction(GoBack, loader.GetString("Ok"));
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

        private async Task RecoveryAsync(string email)
        {
            // Call the CustomPasswordRecover API
            var response = await App.MobileService
                    .InvokeApiAsync<RecoveryPasswordRequest, string>(
                        "CustomPasswordRecover", new RecoveryPasswordRequest()
                        {
                            Email = email
                        });
        }

        private void GoBack()
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame != null && rootFrame.CanGoBack)
                rootFrame.GoBack();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            GoBack();
        }
    }
}
