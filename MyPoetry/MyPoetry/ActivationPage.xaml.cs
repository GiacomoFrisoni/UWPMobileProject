using MyPoetry.UserControls;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

// Il modello di elemento Pagina vuota è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=234238

namespace MyPoetry
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class ActivationPage : Page
    {
        public ActivationPage()
        {
            this.InitializeComponent();
        }

        private async void BtnConfirm_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();

            HalfPageMessage hpm = new HalfPageMessage(GrdParent);
            hpm.ShowMessage(loader.GetString("Checking"), loader.GetString("CheckingText"), true, false, false, null, null);

            await SimulateLoadingData();

            //Codice verificato!
            if (true)
            {
                hpm.Dismiss();
                this.Frame.Navigate(typeof(WelcomePage));
            }
            else
            {
                hpm.IsProgressRingEnabled = false;
                hpm.Title = loader.GetString("Error");
                hpm.Message = loader.GetString("Err_InvalidCode");
                hpm.IsOkButtonEnabled = true;
                hpm.SetOkAction(null, loader.GetString("Ok"));
            }
            
        }

        private async Task SimulateLoadingData()
        {
            await Task.Delay(2500);
        }



        private void BtnCodeNotReceived_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            HalfPageMessage hpm = new HalfPageMessage(GrdParent);
            hpm.ShowMessage(loader.GetString("CodeRequestTitle"), loader.GetString("CodeRequestMessage"), false, false, true, null, null);
        }

        private void BtnBack_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(LoginPage));
        }
    }
}
