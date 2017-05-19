using Microsoft.WindowsAzure.MobileServices;
using MyPoetry.Model;
using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml.Media.Imaging;
using MyPoetry.UserControls;

namespace MyPoetry
{
    /// <summary>
    /// Registration page.
    /// </summary>
    public sealed partial class RegisterPage : Page
    {
        public const string MALE = "M";
        public const string FEMALE = "F";

        private byte[] bytesPhoto = null;
        

        public RegisterPage()
        {
            this.InitializeComponent();
        }

        private async void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            await RegisterUser(
                txbEmail.Text,
                pbPassword.Password,
                pbPasswordConfirm.Password,
                txbName.Text,
                txbSurname.Text,
                cmbGender.SelectedIndex >= 0 ? (cmbGender.SelectedIndex == 0 ? MALE : FEMALE) : String.Empty,
                bytesPhoto,
                DateTime.Now
            );
        }

        private async Task RegisterUser(string email, string password, string passwordConfirm, string name,
            string surname, string gender, byte[] photo, DateTime registrationDate)
        {

            HalfPageMessage hpm = new HalfPageMessage();

            Exception exception = null;
            try
            {
                hpm.ShowMessage(grdParent, "Registrazione in corso", "Stiamo provvedendo alla registrazione del tuo account", true, false, false, null, null);

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
                    var msg = new MessageDialog(ServerErrorInfo.Instance.GetInfo(exception.Message));
                    await msg.ShowAsync();
                    hpm.Dismiss();
                }
                else
                {
                    hpm.IsProgressRingEnabled = false;
                    hpm.Title = "Registrazione effettuata!";
                    hpm.Message = "Grandioso! Ora puoi effetuare il login coi tuoi dati di accesso";
                    hpm.SetOkAction(GoBack);
                    hpm.IsOkButtonEnabled = true;                            
                }
            }
        }

        private bool GoBack()
        {
            this.Frame.Navigate(typeof(LoginPage));
            return false;
        }

        private void btnBackToLogin_Click(object sender, RoutedEventArgs e)
        {
            GoBack();
        }
        
        private async void btnPhoto_Click(object sender, RoutedEventArgs e)
        {
            CameraCaptureUI captureUI = new CameraCaptureUI();
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            captureUI.PhotoSettings.CroppedSizeInPixels = new Size(300, 300);

            StorageFile photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);

            if (photo == null)
            {
                // User cancelled photo capture
                return;
            }
           
            IRandomAccessStream stream = await photo.OpenAsync(FileAccessMode.Read);
            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
            var pixels = await decoder.GetPixelDataAsync();
            bytesPhoto = pixels.DetachPixelData();
            SoftwareBitmap softwareBitmap = await decoder.GetSoftwareBitmapAsync();

            SoftwareBitmap softwareBitmapBGR8 = SoftwareBitmap.Convert(softwareBitmap,BitmapPixelFormat.Bgra8,BitmapAlphaMode.Premultiplied);

            SoftwareBitmapSource bitmapSource = new SoftwareBitmapSource();
            await bitmapSource.SetBitmapAsync(softwareBitmapBGR8);

            imgProfile.Source = bitmapSource;

            await photo.DeleteAsync();
        }

        private void btnDeletePhoto_Click(object sender, RoutedEventArgs e)
        {
            bytesPhoto = null;
            imgProfile.Source = null;
        }
    }
}
