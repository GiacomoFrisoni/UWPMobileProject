using Microsoft.Toolkit.Uwp;
using Microsoft.WindowsAzure.MobileServices;
using MyPoetry.Model;
using MyPoetry.Utilities;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using XamlBrewer.Uwp.Controls;

namespace MyPoetry.UserControls
{
    public sealed partial class PoetryViewer : UserControl
    {
        public PoetryViewer()
        {
            this.InitializeComponent();

            RatingControl.FilledImage = new Uri("ms-appx:///Assets/Rating/staron.png");
            RatingControl.EmptyImage = new Uri("ms-appx:///Assets/Rating/staroff.png");

            RegisterForShare();
        }


        public event EventHandler BackEvent;
        public event EventHandler ForwardEvent;
        public event EventHandler RefreshEvent;


        #region Share
        private void RegisterForShare()
        {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += new Windows.Foundation.TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(this.SharePoetry);
        }

        private async void SharePoetry(DataTransferManager sender, DataRequestedEventArgs e)
        {
            var loader = new ResourceLoader();

            // Title and description
            DataRequest request = e.Request;
            request.Data.Properties.Title = ((Poetry)DataContext).Title;
            request.Data.Properties.Description = loader.GetString("SharingDescription");

            // RTF
            string rtfText = ((Poetry)DataContext).Body;
            request.Data.SetRtf(rtfText);
            
            // Prepare data for conversion
            RichEditBox reb = new RichEditBox();
            reb.Document.SetText(TextSetOptions.FormatRtf, rtfText);
            ITextRange poetryText = reb.Document.GetRange(0, TextConstants.MaxUnitCount);
            
            // Plain text
            string plainText = poetryText.Text + "\r" + UserHandler.Instance.GetUser().Name + " " + UserHandler.Instance.GetUser().Surname +
                "\r" + loader.GetString("SharingFooter");
            request.Data.SetText(plainText);

            // HTML
            string htmlFormat = HtmlFormatHelper.CreateHtmlFormat(poetryText.Text.Replace("\r", "<br>"));
            request.Data.SetHtmlFormat(htmlFormat);


            // Because we are making async calls in the DataRequested event handler,
            // we need to get the deferral first
            DataRequestDeferral deferral = request.GetDeferral();

            // Make sure we always call Complete on the deferral
            try
            {
                // Sets the preview image
                StorageFile thumbnailFile = await Package.Current.InstalledLocation.GetFileAsync("Assets\\SmallTile.scale-200.png");
                request.Data.Properties.Thumbnail = RandomAccessStreamReference.CreateFromFile(thumbnailFile);
            }
            finally
            {
                deferral.Complete();
            }
        }

        private void BtnShare_Click(object sender, RoutedEventArgs e)
        {
            DataTransferManager.ShowShareUI();
        }
        #endregion

        #region Edit
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            UserHandler.Instance.SetPoetryToEdit((Poetry)DataContext);
            MenuHandler.Instance.SetMenuIndex(2);
        }
        #endregion

        #region Details
        private void BtnDetails_Click(object sender, RoutedEventArgs e)
        {
            PoetrySplitView.IsPaneOpen = !PoetrySplitView.IsPaneOpen;
        }
        #endregion

        #region Delete
        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var loader = new ResourceLoader();

            // Create the message dialog
            MessageDialog messageDialog = new MessageDialog(loader.GetString("DeleteConfirm"), loader.GetString("Confirm"));

            // YES command
            messageDialog.Commands.Add(new UICommand(loader.GetString("Yes"), async (command) =>
            {
                Exception exception = null;
                HalfPageMessage hpm = new HalfPageMessage(GrdParent);
                try
                {
                    // Shows loading message
                    hpm.ShowMessage(loader.GetString("RemovalInProgress"), loader.GetString("ServerConnection"), true, false, false, null, null);

                    // Deletes poetry
                    await App.MobileService.GetTable<Poetry>().DeleteAsync((Poetry)this.DataContext);
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
                        // Refreshes MasterDetail items
                        RefreshEvent?.Invoke(sender, null);

                        hpm.Dismiss();
                    }
                }
            }));

            // NO command
            messageDialog.Commands.Add(new UICommand(loader.GetString("No"), (command) =>
            {
            }));
            
            // Set the command that will be invoked by default
            messageDialog.DefaultCommandIndex = 1;

            // Show the message dialog
            await messageDialog.ShowAsync();
        }
        #endregion

        #region Back and Forward Navigation
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            BackEvent?.Invoke(sender, null);
        }

        private void BtnForward_Click(object sender, RoutedEventArgs e)
        {
            ForwardEvent?.Invoke(sender, null);
        }
        #endregion

        #region Print
        async private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            StackPanel stp = new StackPanel();
            Grid g = new Grid();
            TextBlock txb = new TextBlock();
            Rectangle rect = new Rectangle();
            RichEditBox rtb = new RichEditBox();

            txb.Text = TxbTitle.Text;
            txb.FontSize = 22;

            rect.Height = 1;
            rect.VerticalAlignment = VerticalAlignment.Bottom;
            rect.Fill = new SolidColorBrush(Colors.LightGray);
            rect.Margin = new Thickness(0, 5, 0, 0);

            g.Children.Add(txb);
            g.Children.Add(rect);

            ITextRange text = RtbView.Document.GetRange(0, TextConstants.MaxUnitCount);
            string s = text.Text;

            rtb.Document.SetText(TextSetOptions.None, s);

            stp.Children.Add(g);
            stp.Children.Add(rtb);
            
            PrintHelper pHelp = new PrintHelper(stp);
            await pHelp.ShowPrintUIAsync("Testo della poesia", true);
        }
        #endregion

        #region Rating
        private void ProgressBarRatingVisible(bool visible)
        {
            ProgressRingRating.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            RatingControl.Visibility = visible ? Visibility.Collapsed : Visibility.Visible;
            ProgressRingRating.IsActive = visible;
        }

        private async void RatingControl_ValueChanged(object sender, RoutedEventArgs e)
        {
            Rating rating = (Rating)sender;
            Poetry poetry = (Poetry)this.DataContext;
            poetry.Rating = Convert.ToInt32(rating.Value);
            ProgressBarRatingVisible(true);
            // Updates rating
            await App.MobileService.GetTable<Poetry>().UpdateAsync(poetry);
            // Refreshes MasterDetail items
            RefreshEvent?.Invoke(sender, null);
            ProgressBarRatingVisible(false);
        }
        #endregion
    }
}
