using MyPoetry.Common;

namespace MyPoetry.ViewModel
{
    /// <summary>
    /// This class handles the view model used for WelcomePage.
    /// </summary>
    partial class WelcomeLocator : BindableBase
    {
        WelcomePageViewModel sWelcomePageViewModel = default(WelcomePageViewModel);
        public WelcomePageViewModel WelcomePageViewModel
        {
            get
            {
                if (sWelcomePageViewModel == null)
                {
                    sWelcomePageViewModel = new WelcomePageViewModel();
                    if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                        sWelcomePageViewModel.LoadDesignTimeData();
                    else
                        sWelcomePageViewModel.LoadDataCommand.Execute();
                }
                return sWelcomePageViewModel;
            }
        }
    }
}
