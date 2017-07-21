using Microsoft.Toolkit.Uwp;
using Microsoft.WindowsAzure.MobileServices;
using MyPoetry.Model;
using MyPoetry.Utilities;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
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
        }


        public event EventHandler BackEvent;
        public event EventHandler ForwardEvent;
        public event EventHandler RefreshEvent;


        private void BtnShare_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDetails_Click(object sender, RoutedEventArgs e)
        {
            PoetrySplitView.IsPaneOpen = !PoetrySplitView.IsPaneOpen;
        }

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

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            BackEvent?.Invoke(sender, null);
        }

        private void BtnForward_Click(object sender, RoutedEventArgs e)
        {
            ForwardEvent?.Invoke(sender, null);
        }

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
    }
}
