using MyPoetry.Model;
using MyPoetry.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.System.Threading;
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

        private ThreadPoolTimer timer;
        private Task checkLoginTask;
        private bool autoLogin = false;

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
            }

            // Create a Frame to act as the navigation context
            rootFrame = new Frame();
            VisualStateManager.GoToState(this, "Extended", true);
        }
        
        /// <summary>
        /// Code executed when the system has completed the default splash screen loading
        /// and is ready for the transition to the extended one.
        /// </summary>
        internal void DismissedEventHandler(SplashScreen sender, object e)
        {
            dismissed = true;

            // Thread synchronization
            timer = ThreadPoolTimer.CreateTimer(TimerElapsedHandler, new TimeSpan(0, 0, 0, 2, 500));
            checkLoginTask = Task.Run(() => CheckAutoLogin());
        }

        /// <summary>
        /// This method is called when the timer is elapsed.
        /// It checks if the auto login task is completed.
        /// If true, calls the dismiss extended splash; otherwise waits for it.
        /// </summary>
        /// <param name="timer">Timer</param>
        private async void TimerElapsedHandler(ThreadPoolTimer timer)
        {
            timer.Cancel();
            if (!checkLoginTask.IsCompleted)
            {
                VisualStateManager.GoToState(this, "Loading", true);
                checkLoginTask.Wait();
            }
            await this.m_oCoreDispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(DismissExtendedSplash));
        }

        /// <summary>
        /// Check if auto login is used.
        /// If yes it gets the user from the server and loads it in memory.
        /// </summary>
        private async void CheckAutoLogin()
        {
            AppLocalSettings settings = new AppLocalSettings();
            if (settings.GetUserLoggedId() != string.Empty)
            {
                autoLogin = true;
                List<User> users = await App.MobileService.GetTable<User>().Where(user => user.Id == settings.GetUserLoggedId()).ToListAsync();
                UserHandler.Instance.SetUser(users.First());
            }
        }

        private void DismissExtendedSplash()
        {
            var rootFrame = new Frame();
            
            // Check auto login
            if (autoLogin)
            {
                if (!UserHandler.Instance.GetUser().IsActivated)
                    rootFrame.Navigate(typeof(ActivationPage));
                else
                    rootFrame.Navigate(typeof(MainPage));
            }
            else
            {
                // Navigate to mainpage
                rootFrame.Navigate(typeof(LoginPage));
            }

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
            }
        }
    }
}