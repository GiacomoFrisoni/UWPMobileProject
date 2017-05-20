using Microsoft.WindowsAzure.MobileServices;
using MyPoetry.Model;
using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using MyPoetry.UserControls;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Media;
using Windows.Storage.Streams;

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

        private async void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            await RegisterUser(
                TxbEmail.Text,
                PbPassword.Password,
                PbPasswordConfirm.Password,
                TxbName.Text,
                TxbSurname.Text,
                CmbGender.SelectedIndex >= 0 ? (CmbGender.SelectedIndex == 0 ? MALE : FEMALE) : String.Empty,
                bytesPhoto,
                DateTime.Now
            );
        }

        private async Task RegisterUser(string email, string password, string passwordConfirm, string name,
            string surname, string gender, byte[] photo, DateTime registrationDate)
        {
            HalfPageMessage hpm = new HalfPageMessage(GrdParent);

            Exception exception = null;
            try
            {
                hpm.ShowMessage("Registrazione in corso", "Stiamo provvedendo alla registrazione del tuo account", true, false, false, null, null);

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
                    hpm.Title = "Registrazione effettuata!";
                    hpm.Message = "Grandioso! Ora puoi effetuare il login coi tuoi dati di accesso";
                    hpm.SetOkAction(GoBack);
                    hpm.IsOkButtonEnabled = true;                            
                }
            }
        }

        private bool GoBack()
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame != null && rootFrame.CanGoBack)
                rootFrame.GoBack();
            return false;
        }

        private void BtnBackToLogin_Click(object sender, RoutedEventArgs e)
        {
            GoBack();
        }
        
        private async void BtnPhoto_Click(object sender, RoutedEventArgs e)
        {
            CameraCaptureUI captureUI = new CameraCaptureUI();
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;

            StorageFile photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);

            if (photo == null)
            {
                // User cancelled photo capture
                return;
            }
            
            bytesPhoto = await AsByteArray(photo);
            ImgProfile.Source = await GetImageSourceFromFile(photo);

            await photo.DeleteAsync();
        }

        private void BtnDeletePhoto_Click(object sender, RoutedEventArgs e)
        {
            bytesPhoto = null;
            ImgProfile.Source = null;
            SblDefault.Visibility = Visibility.Visible;
        }

        private async void BtnFile_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                bytesPhoto = await AsByteArray(file);
                ImgProfile.Source = await GetImageSourceFromFile(file);
            }
        }

        private async Task<ImageSource> GetImageSourceFromFile(StorageFile file)
        {
            var stream = await file.OpenAsync(FileAccessMode.Read);
            var image = new BitmapImage();
            image.SetSource(stream);
            SblDefault.Visibility = Visibility.Collapsed;
            return image;
        }

        /// <summary>
        /// Converts StorageFile to ByteArray
        /// </summary>
        /// <param name="file">Storage file</param>
        /// <returns>Byte array</returns>
        public static async Task<byte[]> AsByteArray(StorageFile file)
        {
            IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read);
            var reader = new DataReader(fileStream.GetInputStreamAt(0));
            await reader.LoadAsync((uint)fileStream.Size);
            byte[] pixels = new byte[fileStream.Size];
            reader.ReadBytes(pixels);
            return pixels;
        }
    }
}
