using MyPoetry.Common;
using MyPoetry.Model;
using MyPoetry.UserControls;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.ApplicationModel.Resources;

namespace MyPoetry.ViewModel
{
    class WelcomePageViewModel : BindableBase
    {
        private ObservableCollection<WelcomeItem> _WelcomePages = new ObservableCollection<WelcomeItem>();

        public ObservableCollection<WelcomeItem> WelcomePages { get { return _WelcomePages; } }

        WelcomeItem _SelectedItem = default(WelcomeItem);
        public WelcomeItem SelectedItem
        {
            get { return _SelectedItem; }
            set
            {
                base.SetProperty(ref _SelectedItem, value);
                foreach (var item in this.WelcomePages.Where(x => x.Selected))
                    item.Selected = false;
                if (value != null)
                {
                    value.Selected = true;
                    ((IWelcomeControl)value.Control).StartAnimation();
                }
                foreach (var item in this.WelcomePages.Where(x => !x.Equals(SelectedItem)))
                    ((IWelcomeControl)item.Control).Reset();
            }
        }

        /// <summary>
        /// Loads the data collection to use in the WelcomePage,
        /// with the first item selected.
        /// </summary>
        public void LoadDesignTimeData()
        {
            var loader = new ResourceLoader();
            _WelcomePages.Clear();
            _WelcomePages.Add(new WelcomeItem() { Control = new Welcome_WritePoetry(), Description = loader.GetString("Write") });
            _WelcomePages.Add(new WelcomeItem() { Control = new Welcome_ReadPoetries(), Description = loader.GetString("Read") });
            _WelcomePages.Add(new WelcomeItem() { Control = new Welcome_SharePoetries(), Description = loader.GetString("Share") });
            _WelcomePages.Add(new WelcomeItem() { Control = new Welcome_Start(), Description = (String)App.Current.Resources["AppName"] });
            _SelectedItem = _WelcomePages.First();
            _WelcomePages.First().Selected = true;
            ((IWelcomeControl)_WelcomePages.First().Control).StartAnimation();
        }


        // Relay Commands to handle click on indicators.
        #region RelayCommands
        RelayCommand _LoadDataCommand = null;
        public RelayCommand LoadDataCommand
        {
            get
            {
                if (_LoadDataCommand == null)
                {
                    _LoadDataCommand = new RelayCommand
                    (
                        () => { this.LoadDesignTimeData(); },
                        () => { return true; }
                    );
                    this.PropertyChanged += (s, e) => _LoadDataCommand.RaiseCanExecuteChanged();
                }
                return _LoadDataCommand;
            }
        }

        RelayCommand<WelcomeItem> _SelectCommand = null;
        public RelayCommand<WelcomeItem> SelectCommand
        {
            get
            {
                if (_SelectCommand == null)
                {
                    _SelectCommand = new RelayCommand<WelcomeItem>
                    (
                        (o) => { this.SelectedItem = o; },
                        (o) => { return true; }
                    );
                    this.PropertyChanged += (s, e) => _SelectCommand.RaiseCanExecuteChanged();
                }
                return _SelectCommand;
            }
        }
        #endregion
    }
}
