using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.Graphics.Printing;
using Windows.Graphics.Printing.OptionDetails;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Printing;

namespace MyPoetry.Utilities
{
    class CustomOptionsPrintHelper : PrintHelper
    {
        /// <summary>
        /// Variable for localized string resources
        /// </summary>
        ResourceLoader loader = new ResourceLoader();

        /// <summary>
        /// A flag that determines if title and body are to be shown
        /// </summary>
        internal DisplayContent titleBody = DisplayContent.TitleAndBody;

        /// <summary>
        /// Helper getter for title showing
        /// </summary>
        private bool ShowTitle
        {
            get { return titleBody == DisplayContent.TitleAndBody; }
        }

        public CustomOptionsPrintHelper(UserControl uc) : base(uc) { }

        /// <summary>
        /// This is the event handler for PrintManager.PrintTaskRequested.
        /// In order to ensure a good user experience, the system requires that the app handle the PrintTaskRequested event within the time specified by PrintTaskRequestedEventArgs.Request.Deadline.
        /// Therefore, we use this handler to only create the print task.
        /// The print settings customization can be done when the print document source is requested.
        /// </summary>
        /// <param name="sender">PrintManager</param>
        /// <param name="e">PrintTaskRequestedEventArgs</param>
        protected override void PrintTaskRequested(PrintManager sender, PrintTaskRequestedEventArgs e)
        {
            PrintTask printTask = null;
            printTask = e.Request.CreatePrintTask(loader.GetString("PoetryPrinting"), sourceRequestedArgs =>
            {
                PrintTaskOptionDetails printDetailedOptions = PrintTaskOptionDetails.GetFromPrintTaskOptions(printTask.Options);
                IList<string> displayedOptions = printDetailedOptions.DisplayedOptions;

                // Choose the printer options to be shown.
                // The order in which the options are appended determines the order in which they appear in the UI
                displayedOptions.Clear();

                displayedOptions.Add(StandardPrintTaskOptions.Copies);
                displayedOptions.Add(StandardPrintTaskOptions.Orientation);

                // Create a new list option
                PrintCustomItemListOptionDetails pageFormat = printDetailedOptions.CreateItemListOption("PageContent", loader.GetString("Contents"));
                pageFormat.AddItem("TitleBody", loader.GetString("TitleBody"));
                pageFormat.AddItem("BodyOnly", loader.GetString("BodyOnly"));

                // Add the custom option to the option list
                displayedOptions.Add("PageContent");

                printDetailedOptions.OptionChanged += printDetailedOptions_OptionChanged;

                // Print Task event handler is invoked when the print job is completed.
                printTask.Completed += async (s, args) =>
                {
                    // Notify the user when the print operation fails.
                    if (args.Completion == PrintTaskCompletion.Failed)
                    {
                        await scenarioUc.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                        {
                            var msg = new MessageDialog(loader.GetString("FailedPrint"));
                            await msg.ShowAsync();
                        });
                    }
                };

                sourceRequestedArgs.SetSource(printDocumentSource);
            });
        }

        async void printDetailedOptions_OptionChanged(PrintTaskOptionDetails sender, PrintTaskOptionChangedEventArgs args)
        {
            // Listen for PageContent changes
            string optionId = args.OptionId as string;
            if (string.IsNullOrEmpty(optionId))
            {
                return;
            }

            if (optionId == "PageContent")
            {
                await scenarioUc.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    printDocument.InvalidatePreview();
                });
            }
        }

        #region PrintPreview

        protected override RichTextBlockOverflow AddOnePrintPreviewPage(RichTextBlockOverflow lastRTBOAdded, PrintPageDescription printPageDescription)
        {
            // Check if we need to hide/show title and body for this scenario
            // Since all is rulled by the first page (page flow), here is where we must start
            if (lastRTBOAdded == null)
            {
                // Get a refference to page objects
                RichTextBlock title = (RichTextBlock)firstPage.FindName("Title");

                // Hide(collapse) and move elements in different grid cells depending by the viewable content(only body, title and body)

                title.Visibility = ShowTitle ? Visibility.Visible : Visibility.Collapsed;
            }

            //Continue with the rest of the base printing layout processing (paper size, printable page size)
            return base.AddOnePrintPreviewPage(lastRTBOAdded, printPageDescription);
        }

        protected override void CreatePrintPreviewPages(object sender, PaginateEventArgs e)
        {
            // Get PageContent property
            PrintTaskOptionDetails printDetailedOptions = PrintTaskOptionDetails.GetFromPrintTaskOptions(e.PrintTaskOptions);
            string pageContent = (printDetailedOptions.Options["PageContent"].Value as string).ToLowerInvariant();

            // Set the title & body display flag
            titleBody = (DisplayContent)(Convert.ToInt32(pageContent.Contains("title")) + Convert.ToInt32(pageContent.Contains("body")));

            base.CreatePrintPreviewPages(sender, e);
        }

        #endregion
    }
}
