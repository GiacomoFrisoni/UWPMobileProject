using MyPoetry.Common;

namespace MyPoetry.ViewModel
{
    partial class Locator : BindableBase
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
