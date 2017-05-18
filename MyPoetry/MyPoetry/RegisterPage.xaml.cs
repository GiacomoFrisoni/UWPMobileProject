using Microsoft.WindowsAzure.MobileServices;
using MyPoetry.Model;
using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
    }
}
