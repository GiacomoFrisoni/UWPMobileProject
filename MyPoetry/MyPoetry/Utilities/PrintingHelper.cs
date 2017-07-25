using MyPoetry.UserControls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Printing;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Printing;

namespace MyPoetry.Utilities
{
    class PrintingHelper
    {
        ///  <summary> 
        /// use PrintManage.GetForCurrentView () Gets a PrintManager objects
        /// PrintManage Printer Manager is responsible for the arrangements for the print stream Windows applications
        /// You must call PrintManager.GetForCurrentView when using () method to return specific to the current window PrintManager
        ///  </ summary> 
        PrintManager printmgr = PrintManager.GetForCurrentView();

        ///  <summary> 
        /// PrintDocument print stream sent to the printer in a reusable objects
        ///  </ summary> 
        PrintDocument PrintDoc = null;

        ///  <summary> 
        /// RotateTransform is to rotate the print element, if the device is sideways of the need to rotate 90 °
        ///  </ summary> 
        RotateTransform rottrf = null;

        ///  <summary> 
        /// representation includes content to be printed as well as provide access to the description of how to print the contents of the information The printing operation
        ///  </ summary> 
        PrintTask Task = null;

        Grid ToPrint;

        public PrintingHelper(Grid toPrint)
        {
            // When requested printing occurs through programmatically by calling ShowPrintUIAsync method to print or by user action may trigger this event 
            printmgr.PrintTaskRequested += Printmgr_PrintTaskRequested;
            ToPrint = toPrint;
        }

        private void Printmgr_PrintTaskRequested(PrintManager Sender, PrintTaskRequestedEventArgs args)
        {
            // Get PrintTaskRequest tasks associated with property from the Request Parameter in
            // After creating the print content and tasks calling the Complete method for printing 
            var deferral = args.Request.GetDeferral();
            // create a print task 

            Task = args.Request.CreatePrintTask(" shopping information - Print single ", OnPrintTaskSourceRequrested);
            Task.Completed += PrintTask_Completed;
            deferral.Complete();
        }

        private void PrintTask_Completed(PrintTask Sender, PrintTaskCompletedEventArgs args)
        {
            // print completed 
        }

        private async void OnPrintTaskSourceRequrested(PrintTaskSourceRequestedArgs args)
        {
            var def = args.GetDeferral();
            await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
             () =>
             {
                 // Set the print source 
                 args.SetSource(PrintDoc?.DocumentSource);
             });
            def.Complete();
        }

        public async void Print()
        {
            if (PrintDoc != null)
            {
                PrintDoc.GetPreviewPage -= OnGetPreviewPage;
                PrintDoc.Paginate -= PrintDic_Paginate;
                PrintDoc.AddPages -= PrintDic_AddPages;
            }

            this.PrintDoc = new PrintDocument();

            // subscribe preview event 
            PrintDoc.GetPreviewPage += OnGetPreviewPage;
            // print parameters occur Subscribe preview change the direction of events such as the document 
            PrintDoc.Paginate += PrintDic_Paginate;
            // add a page to handle events 
            PrintDoc.AddPages += PrintDic_AddPages;

            // display the Print dialog box 
            bool showPrint = await PrintManager.ShowPrintUIAsync();
        }

        // add the contents of the printed page 
        private void PrintDic_AddPages(Object Sender, AddPagesEventArgs e)
        {
            // add elements of a page to be printed 
            PrintDoc.AddPage(ToPrint);

            // completed to increase the printed page 
            PrintDoc.AddPagesComplete();
        }

        private void PrintDic_Paginate(Object Sender, PaginateEventArgs e)
        {
            PrintTaskOptions opt = Task.Options;
            // to adjust according to the direction of the page print direction of rotation 
            switch (opt.Orientation)
            {
                case PrintOrientation.Default:
                    rottrf.Angle = 0d;
                    break;
                case PrintOrientation.Portrait:
                    rottrf.Angle = 0d;
                    break;
                case PrintOrientation.Landscape:
                    rottrf.Angle = 90d;
                    break;
            }

            // set the preview page of the total number of pages 
            PrintDoc.SetPreviewPageCount(1, PreviewPageCountType.Final);
        }

        private void OnGetPreviewPage(Object Sender, GetPreviewPageEventArgs e)
        {
            // set to preview page 
            PrintDoc.SetPreviewPage(e.PageNumber, ToPrint);
        }
    }
}
