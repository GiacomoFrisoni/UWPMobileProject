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

// Il modello di elemento Pagina vuota è documentato all'indirizzo http://go.microsoft.com/fwlink/?LinkId=234238

namespace MyPoetry
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class RegisterPage : Page
    {
        public const string MALE = "M";
        public const string FEMALE = "F";

        public RegisterPage()
        {
            this.InitializeComponent();
        }

        private async void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txbEmail.Text) &&
                !string.IsNullOrWhiteSpace(pbPassword.Password) &&
                !string.IsNullOrWhiteSpace(txbName.Text) &&
                !string.IsNullOrWhiteSpace(txbSurname.Text) &&
                cmbGender.SelectedIndex > 0)
            {
                await RegisterUser(
                    txbEmail.Text,
                    pbPassword.Password,
                    txbName.Text,
                    txbSurname.Text,
                    cmbGender.SelectedIndex == 0 ? MALE : FEMALE,
                    null,
                    DateTime.Now
                );
            }
        }

        private async Task RegisterUser(string email, string password, string name, string surname, string gender, byte[] photo, DateTime registrationDate)
        {
            Exception exception = null;
            try
            {
                // Make sure that the user is registered, using the hard-coded
                // dummy registration credentials. In a real app, you must get these at runtime.
                var response = await App.MobileService
                    .InvokeApiAsync<RegistrationRequest, string>(
                        "CustomRegistration", new RegistrationRequest()
                        {
                            Email = email,
                            Password = password,
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
                    var msg = new MessageDialog(exception.Message);
                    await msg.ShowAsync();
                }
            }
        }

        private void btnBackToLogin_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(LoginPage));
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

            StorageFolder destinationFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("ProfilePhotoFolder", CreationCollisionOption.OpenIfExists);

            await photo.CopyAsync(destinationFolder, "ProfilePhoto.jpg", NameCollisionOption.ReplaceExisting);
           
            IRandomAccessStream stream = await photo.OpenAsync(FileAccessMode.Read);
            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
            SoftwareBitmap softwareBitmap = await decoder.GetSoftwareBitmapAsync();

            SoftwareBitmap softwareBitmapBGR8 = SoftwareBitmap.Convert(softwareBitmap,BitmapPixelFormat.Bgra8,BitmapAlphaMode.Premultiplied);

            SoftwareBitmapSource bitmapSource = new SoftwareBitmapSource();
            await bitmapSource.SetBitmapAsync(softwareBitmapBGR8);

            imgProfile.Source = bitmapSource;

            await photo.DeleteAsync();
        }
    }
}
