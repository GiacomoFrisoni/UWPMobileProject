using Microsoft.WindowsAzure.MobileServices;
using MyPoetry.Model;
using MyPoetry.Utilities;
using System;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Resources;
using Windows.Graphics.Printing;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using XamlBrewer.Uwp.Controls;

namespace MyPoetry.UserControls
{
    public sealed partial class PoetryViewer : UserControl
    {
        PrintHelper printHelper;

        public PoetryViewer()
        {
            this.InitializeComponent();

            RatingControl.FilledImage = new Uri("ms-appx:///Assets/Rating/staron.png");
            RatingControl.EmptyImage = new Uri("ms-appx:///Assets/Rating/staroff.png");

            RegisterForShare();

            // Sets animations
            backStoryboard = (Storyboard)this.Resources["BackAnimation"];
            backStoryboard.Completed += (s, ev) =>
            {
                backStoryboard.Stop();
                BackEvent?.Invoke(s, null);
                fadeInStoryboard.Begin();
            };
            forwardStoryboard = (Storyboard)this.Resources["ForwardAnimation"];
            forwardStoryboard.Completed += (s, ev) =>
            {
                forwardStoryboard.Stop();
                ForwardEvent?.Invoke(s, null);
                fadeInStoryboard.Begin();
            };
            fadeInStoryboard = (Storyboard)this.Resources["InAnimation"];
        }
        
        private void PoetryViewer_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            CheckNavigationEnabling();

            if (DataContext != null)
            {
                if (printHelper != null)
                {
                    printHelper.UnregisterForPrinting();
                }

                //-----------------------------

                if (!PrintManager.IsSupported())
                {
                    // Remove the print button
                    BtnPrint.IsEnabled = false;
                }

                // Initalize common helper class and register for printing
                printHelper = new CustomOptionsPrintHelper(this);
                printHelper.RegisterForPrinting();

                // Initialize print content for this scenario
                printHelper.PreparePrintContent(new PageToPrint((Poetry)DataContext));
            }
        }

        Storyboard backStoryboard;
        Storyboard forwardStoryboard;
        Storyboard fadeInStoryboard;

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
                if (Connection.HasInternetAccess)
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
                }
                else
                {
                    ((Frame)Window.Current.Content).Navigate(typeof(NoConnectionPage));
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
            if (backStoryboard.GetCurrentState() != ClockState.Active)
                backStoryboard.Begin();
        }
        

        private void BtnForward_Click(object sender, RoutedEventArgs e)
        {
            if (forwardStoryboard.GetCurrentState() != ClockState.Active)
                forwardStoryboard.Begin();
        }

        private void CheckNavigationEnabling()
        {
            if (DataContext != null)
            {
                if (UserHandler.Instance.GetPoetries().Count > 0)
                {
                    BtnBack.IsEnabled = UserHandler.Instance.GetPoetries().First().Equals((Poetry)DataContext) ? false : true;
                    BtnForward.IsEnabled = UserHandler.Instance.GetPoetries().Last().Equals((Poetry)DataContext) ? false : true;
                }
                else
                {
                    BtnBack.IsEnabled = false;
                    BtnForward.IsEnabled = false;
                }
            }
        }
        #endregion

        #region Print
        private async void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (Windows.Graphics.Printing.PrintManager.IsSupported())
            {
                try
                {
                    // Show print UI
                    await Windows.Graphics.Printing.PrintManager.ShowPrintUIAsync();

                }
                catch
                {
                    // Printing cannot proceed at this time
                    ContentDialog noPrintingDialog = new ContentDialog()
                    {
                        Title = "Printing error",
                        Content = "\nSorry, printing can' t proceed at this time.",
                        PrimaryButtonText = "OK"
                    };
                    await noPrintingDialog.ShowAsync();
                }
            }
            else
            {
                // Printing is not supported on this device
                ContentDialog noPrintingDialog = new ContentDialog()
                {
                    Title = "Printing not supported",
                    Content = "\nSorry, printing is not supported on this device.",
                    PrimaryButtonText = "OK"
                };
                await noPrintingDialog.ShowAsync();
            }
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
            if (Connection.HasInternetAccess)
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
            else
            {
                ((Frame)Window.Current.Content).Navigate(typeof(NoConnectionPage));
            }
        }
        #endregion
        
    }
}
