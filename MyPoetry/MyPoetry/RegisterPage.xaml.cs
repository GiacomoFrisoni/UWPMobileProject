using Microsoft.WindowsAzure.MobileServices;
using MyPoetry.Model;
using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MyPoetry.UserControls;
using MyPoetry.Utilities;
using Windows.ApplicationModel.Resources;

namespace MyPoetry
{
    /// <summary>
    /// This class handles the Registration page.
    /// </summary>
    public sealed partial class RegisterPage : Page
    {
        public const string MALE = "M";
        public const string FEMALE = "F";
      

        public RegisterPage()
        {
            this.InitializeComponent();
        }

        private void GrdParent_Loaded(object sender, RoutedEventArgs e)
        {
            var loader = new ResourceLoader();
            RgcRegistration.EnableToRegister(loader.GetString("RegisterNow"), loader.GetString("AlreadyHaveAccount"), Register, GoBack);
        }

        private async void Register()
        {
            if (Connection.HasInternetAccess)
            {
                await RegisterUser(
                    RgcRegistration.GetEmail,
                    RgcRegistration.GetPassword,
                    RgcRegistration.GetPasswordRepeat,
                    RgcRegistration.GetName,
                    RgcRegistration.GetSurname,
                    RgcRegistration.GetGender,
                    RgcRegistration.GetPhoto,
                    DateTime.Now
                );
            }
            else
            {
                this.Frame.Navigate(typeof(NoConnectionPage));
            }
        }

        private async Task RegisterUser(string email, string password, string passwordConfirm, string name,
            string surname, string gender, byte[] photo, DateTime registrationDate)
        {
            HalfPageMessage hpm = new HalfPageMessage(GrdParent);
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();

            Exception exception = null;
            try
            {
                hpm.ShowMessage(loader.GetString("RegisterPlainText"), loader.GetString("RegisterMessage"), true, false, false, null, null);

                var response = await App.MobileService
                    .InvokeApiAsync<RegistrationRequest, string>(
                        "CustomRegistration", new RegistrationRequest()
                        {
                            Email = email,
                            Password = password,
                            PasswordConfirm = passwordConfirm,
                            Name = name,
                            Surname = surname,
                            Gender = gender,
                            Photo = photo,
                            RegistrationDate = registrationDate
                        });
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
                    hpm.IsProgressRingEnabled = false;
                    hpm.Title = loader.GetString("RegisterSuccessful");
                    hpm.Message = loader.GetString("RegisterSuccessfulMessage");
                    hpm.SetOkAction(GoBack, loader.GetString("Ok"));
                    hpm.IsOkButtonEnabled = true;                            
                }
            }
        }

        private void GoBack()
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame != null && rootFrame.CanGoBack)
                rootFrame.GoBack();
        }

    }
}
