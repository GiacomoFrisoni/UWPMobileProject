using MyPoetry.Model;
using MyPoetry.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MyPoetry.UserControls
{
    public sealed partial class RegistrationControl : UserControl
    {
        /// <summary>
        /// Save the reference for the action after pressing button
        /// </summary>
        private Action PrimaryAction, SecondaryAction;

        private const string MALE = "M";
        private const string FEMALE = "F";

        private byte[] bytesPhoto = null;



        public RegistrationControl()
        {
            this.InitializeComponent();
        }

        public RegistrationControl(User user)
        {
            this.InitializeComponent();

            TxbName.Text = user.Name;
            TxbSurname.Text = user.Surname;
            TxbEmail.Text = user.Email;
            CmbGender.SelectedItem = user.Gender == MALE ? 0 : 1;
            SetImage();
        }

        private async void SetImage()
        {
            ImgProfile.Source = await ImageHelper.ImageFromBytes(UserHandler.Instance.GetUser().Photo);
        }


        public string GetName { get { return TxbName.Text; } }
        public string GetSurname { get { return TxbSurname.Text; } }
        public string GetEmail { get { return TxbEmail.Text; } }
        public string GetPassword { get { return PbPassword.Password; } }
        public string GetPasswordRepeat { get { return PbPasswordConfirm.Password; } }
        public byte[] GetPhoto { get { return bytesPhoto; } }
        public string GetGender { get { return CmbGender.SelectedItem as string; } }



        /// <summary>
        /// Get or set the visibility of MAIL input
        /// </summary>
        public bool IsMailEnabled
        {
            get { return TxbEmail.Visibility == Visibility.Collapsed ? false : true; }
            set
            {
                if (value)
                {
                    TxbEmail.Visibility = Visibility.Visible;
                    TxbEmailText.Visibility = Visibility.Visible;
                }
                else
                {
                    TxbEmail.Visibility = Visibility.Collapsed;
                    TxbEmailText.Visibility = Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// Get or Set the primary button text
        /// </summary>
        public string PrimaryButtonText
        {
            get { return BtnPrimary.Content as string; }
            set { BtnPrimary.Content = value; }
        }

        /// <summary>
        /// Get or Set the secondary button text
        /// </summary>
        public string SecondaryButtonText
        {
            get { return BtnSecondary.Content as string; }
            set { BtnSecondary.Content = value; }
        }

        /// <summary>
        /// Get or set the visibility of secondary button
        /// </summary>
        public bool IsSecondaryButtonEnabled
        {
            get { return BtnSecondary.Visibility == Visibility.Collapsed ? false : true; }
            set
            {
                if (value)
                {
                    BtnSecondary.Visibility = Visibility.Visible;
                }
                else
                {
                    BtnSecondary.Visibility = Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// Get or set the title of the page
        /// </summary>
        public string Title
        {
            get { return TxblTitle.Text; }
            set { TxblTitle.Text = value; }
        }

        /// <summary>
        /// Enables the control to get input for user REGISTRATION
        /// </summary>
        /// <param name="primaryString">Text to show in first button</param>
        /// <param name="secondaryString">Text to show in second button</param>
        /// <param name="primaryAction">Action to perform when first buton is clicked</param>
        /// <param name="secondaryAction">Action to perform when second button is clicked</param>
        public void EnableToRegister(string primaryString, string secondaryString, Action primaryAction, Action secondaryAction)
        {
            IsMailEnabled = true;
            IsSecondaryButtonEnabled = true;
            PrimaryButtonText = primaryString;
            SecondaryButtonText = secondaryString;
            PrimaryAction = primaryAction;
            SecondaryAction = secondaryAction;
            Title = "Register";
        }

        /// <summary>
        /// Enables the control to get input for user MODIFY DATA. Secondary button and mail modify is disabled
        /// </summary>
        /// <param name="primaryString">Text to show in first button</param>
        /// <param name="primaryAction">Action to perform when first buton is clicked</param>
        public void EnableToModify(string primaryString, Action primaryAction)
        {
            IsMailEnabled = false;
            IsSecondaryButtonEnabled = false;
            PrimaryButtonText = primaryString;
            PrimaryAction = primaryAction;
            Title = "Modify";
        }



        private void BtnPrimary_Click(object sender, RoutedEventArgs e)
        {
            this.PrimaryAction?.Invoke();
        }

        private void BtnSecondary_Click(object sender, RoutedEventArgs e)
        {
            this.SecondaryAction?.Invoke();
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

        private void StackPanel_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                FocusManager.TryMoveFocus(FocusNavigationDirection.Next);
                e.Handled = true;
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
