using MyPoetry.Model;
using MyPoetry.UserControls;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MyPoetry
{
    public sealed partial class WelcomePage : Page
    {
        private Welcome_WritePoetry writePoetry;
        private Welcome_ReadPoetries readPoetries;
        private Welcome_SharePoetries sharePoetries;
        private Welcome_Start startApp;

        public WelcomePage()
        {
            this.InitializeComponent();
            this.Loaded += WelcomePage_Loaded;
        }

        private void WelcomePage_Loaded(object sender, RoutedEventArgs e)
        {
            writePoetry = new Welcome_WritePoetry();
            readPoetries = new Welcome_ReadPoetries();
            sharePoetries = new Welcome_SharePoetries();
            startApp = new Welcome_Start();

            var loader = new ResourceLoader();
            List<WelcomeFlipViewItem> flipViewData = new List<WelcomeFlipViewItem>();
            flipViewData.Add(new WelcomeFlipViewItem() { Control = writePoetry, Description = loader.GetString("Write") });
            flipViewData.Add(new WelcomeFlipViewItem() { Control = readPoetries, Description = loader.GetString("Read") });
            flipViewData.Add(new WelcomeFlipViewItem() { Control = sharePoetries, Description = loader.GetString("Share") });
            flipViewData.Add(new WelcomeFlipViewItem() { Control = startApp, Description =  (String)App.Current.Resources["AppName"]});
            FlipView.ItemsSource = null;
            FlipView.ItemsSource = flipViewData;
        }

        private void FlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((FlipView)sender).SelectedIndex)
            {
                case 0:
                    writePoetry.StartAnimation();
                    readPoetries.Reset();
                    sharePoetries.Reset();
                    startApp.Reset();
                    break;
                case 1:
                    readPoetries.StartAnimation();
                    writePoetry.Reset();
                    sharePoetries.Reset();
                    startApp.Reset();
                    break;
                case 2:
                    sharePoetries.StartAnimation();
                    writePoetry.Reset();
                    readPoetries.Reset();
                    startApp.Reset();
                    break;
                case 3:
                    startApp.StartAnimation();
                    writePoetry.Reset();
                    readPoetries.Reset();
                    sharePoetries.Reset();
                    break;
                default:
                    break;
            }
        }
    }
}
