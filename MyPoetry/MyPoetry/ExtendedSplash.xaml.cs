using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MyPoetry
{
    /// <summary>
    /// This class handles an extended customized splash screen.
    /// </summary>
    public sealed partial class ExtendedSplash : Page
    {
        internal Rect splashImageRect;                  // Rect to store splash screen image coordinates.
        private SplashScreen splash;                    // Variable to hold the splash screen object.
        internal bool dismissed = false;                // Variable to track splash screen dismissal status.
        internal Frame rootFrame;
        public delegate void SplashEventHandler();
        private CoreDispatcher m_oCoreDispatcher;
        private Double scaleFactor;

        public ExtendedSplash(SplashScreen splashScreen, bool loadState)
        {
            this.InitializeComponent();
            this.m_oCoreDispatcher = Window.Current.Dispatcher;

            // Variable to hold the device scale factor (use to determine phone screen resolution)
            scaleFactor = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;

            // Listen for window resize events to reposition the extended splash screen image accordingly.
            // This ensures that the extended splash screen formats properly in response to window resizing.
            Window.Current.SizeChanged += new WindowSizeChangedEventHandler(ExtendedSplashScreen_OnResize);

            splash = splashScreen;
            if (splash != null)
            {
                // Register an event handler to be executed when the splash screen has been dismissed.
                splash.Dismissed += new TypedEventHandler<SplashScreen, object>(DismissedEventHandler);

                // Retrieve the window coordinates of the splash screen image.
                splashImageRect = splash.ImageLocation;
                AdjustImage();
            }

            // Create a Frame to act as the navigation context
            rootFrame = new Frame();
            VisualStateManager.GoToState(this, "Extended", true);
        }

        /// <summary>
        /// This method simulates loading data.
        /// </summary>
        /// <returns></returns>
        private async Task SimulateLoadingData()
        {
            await Task.Delay(2000);
        }

        /// <summary>
        /// This method adjustes Splash Screen image according to dimension factors.
        /// </summary>
        void AdjustImage()
        {/*
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                extendedSplashImage.Height = splashImageRect.Height / scaleFactor;
                extendedSplashImage.Width = splashImageRect.Width / scaleFactor;
            }*/
        }

        /// <summary>
        /// Code executed when the system has completed the default splash screen loading
        /// and is ready for the transition to the extended one.
        /// </summary>
        internal async void DismissedEventHandler(SplashScreen sender, object e)
        {
            dismissed = true;

            // Thread synchronization
            await SimulateLoadingData();
            await this.m_oCoreDispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(DismissExtendedSplash));
        }

        private void DismissExtendedSplash()
        {
            var rootFrame = new Frame();

            // Navigate to mainpage
            rootFrame.Navigate(typeof(MainPage));

            // Place the frame in the current Window
            Window.Current.Content = rootFrame;
            Window.Current.Activate();
        }

        void ExtendedSplashScreen_OnResize(Object sender, WindowSizeChangedEventArgs e)
        {
            // Safely update the extended splash screen image coordinates. This function will be executed when a user resizes the window.
            if (splash != null)
            {
                // Update the coordinates of the splash screen image.
                splashImageRect = splash.ImageLocation;
                AdjustImage();
            }
        }
    }
}