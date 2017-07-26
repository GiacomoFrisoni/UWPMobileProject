using MyPoetry.Model;
using MyPoetry.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp;
using Windows.UI.Xaml.Media;

namespace MyPoetry.UserControls.Pages
{
    public sealed partial class Profile : UserControl
    {
        public Profile()
        {
            this.InitializeComponent();
        }

        public CustomPage GetPage { get { return MainContent; } }

        RegistrationControl rc;
      
        private async void LoadData()
        {
            // Loads profile image
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = await ImageHelper.ImageFromBytes(UserHandler.Instance.GetUser().Photo);
            UsrViewer.ImageSource = ib;
            UsrViewer.Title = UserHandler.Instance.GetUser().Name + " " + UserHandler.Instance.GetUser().Surname;
            UsrViewer.Details = UserHandler.Instance.GetUser().Email;    

            // Generates statistic elements inside the gridview
            ProfileGridView.ItemsSource = null;
            ProfileGridView.ItemsSource = GenerateAdvancedInfo();
            
            //Now loads the editor part
            rc = new RegistrationControl(UserHandler.Instance.GetUser());
            rc.EnableToModify("Salva", SaveUser);

            StpModify.Child = rc;
        }
        
        private List<DataViewer> GenerateAdvancedInfo()
        {
            var loader = new ResourceLoader();

            List<DataViewer> info = new List<DataViewer>();
            info.Add(new DataViewer(loader.GetString("ProfileInspiration"),       loader.GetString("ProfileWriteFrom") + UserHandler.Instance.GetUser().RegistrationDate.ToString("dd/MM/yyyy"), Symbol.Calendar, new SolidColorBrush(ColorHelper.ToColor("#1BBC9B"))));
            info.Add(new DataViewer(loader.GetString("ProfileBornWriter"),        loader.GetString("ProfileYouWrite") + UserHandler.Instance.GetPoetries().Count.ToString() + loader.GetString("ProfilePoetries"), Symbol.Edit, new SolidColorBrush(ColorHelper.ToColor("#2DCC70"))));
            info.Add(new DataViewer(loader.GetString("ProfileKeyboardDestroyer"), loader.GetString("ProfileYouType") + UserHandler.Instance.GetPoetries().Sum(poetry => poetry.CharactersNumber).ToString() + loader.GetString("ProfileCharacters"), Symbol.Font, new SolidColorBrush(ColorHelper.ToColor("#3598DB"))));
            info.Add(new DataViewer(loader.GetString("ProfileWiseWords"),         loader.GetString("ProfileYouUsed") + UserHandler.Instance.GetPoetries().Sum(poetry => poetry.WordsNumber).ToString() + loader.GetString("ProfileWords"), Symbol.FontColor, new SolidColorBrush(ColorHelper.ToColor("#9B58B5"))));
            info.Add(new DataViewer(loader.GetString("ProfileWrappingMaster"),    loader.GetString("ProfileLineNumbers") + UserHandler.Instance.GetPoetries().Sum(poetry => poetry.VersesNumber).ToString(), Symbol.ShowResults, new SolidColorBrush(ColorHelper.ToColor("#34495E"))));

            if (UserHandler.Instance.GetPoetries().Count > 0)
            {
                info.Add(new DataViewer(loader.GetString("ProfileLongest"), UserHandler.Instance.GetPoetries().OrderBy(poetry => poetry.CharactersNumber).First().Title, Symbol.Remove, new SolidColorBrush(ColorHelper.ToColor("#F1C40F"))));
                info.Add(new DataViewer(loader.GetString("ProfileShortest"), UserHandler.Instance.GetPoetries().OrderByDescending(poetry => poetry.CharactersNumber).First().Title, Symbol.List, new SolidColorBrush(ColorHelper.ToColor("#E77E23"))));
            }

            return info;
        }

        private async void SaveUser()
        {
            if (rc != null)
            {
                if (Connection.HasInternetAccess)
                {
                    var loader = new ResourceLoader();

                    // Create the message dialog
                    MessageDialog messageDialog = new MessageDialog(loader.GetString("UpdateUserConfirm"), loader.GetString("Update"));

                    // YES command
                    messageDialog.Commands.Add(new UICommand(loader.GetString("Yes"), async (command) =>
                    {
                        // Updates user data
                        UserHandler.Instance.GetUser().Name = rc.GetName;
                        UserHandler.Instance.GetUser().Surname = rc.GetSurname;
                        UserHandler.Instance.GetUser().Photo = rc.GetPhoto;
                        UserHandler.Instance.GetUser().Gender = rc.GetGender;

                        HalfPageMessage hpm = new HalfPageMessage(GrdParent);
                        hpm.ShowMessage(loader.GetString("UpdatingUser"), loader.GetString("UpdatingUserMessage"), true, false, false, null, null);

                        await App.MobileService.GetTable<Model.User>().UpdateAsync(UserHandler.Instance.GetUser());

                        // Refresh user in cache
                        List<Model.User> users = await App.MobileService.GetTable<Model.User>().Where(user => user.Id == UserHandler.Instance.GetUser().Id).ToListAsync();
                        UserHandler.Instance.SetUser(users.First());

                        // Shows confirm message
                        hpm.IsProgressRingEnabled = false;
                        hpm.Title = loader.GetString("Confirm");
                        hpm.Message = loader.GetString("UserUpdated");
                        hpm.SetOkAction(() => {
                            ScrProfile.Visibility = Visibility.Visible;
                            StpModify.Visibility = Visibility.Collapsed;
                        }, "OK");
                        hpm.IsOkButtonEnabled = true;
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
                else
                {
                    ((Frame)Window.Current.Content).Navigate(typeof(NoConnectionPage));
                }
            }
        }

        private void ProgressBarVisible(bool visible)
        {
            ProgressRingProfile.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            ProgressRingProfile.IsActive = visible;
        }
        
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (ScrProfile.Visibility == Visibility.Visible)
            {
                ScrProfile.Visibility = Visibility.Collapsed;
                StpModify.Visibility = Visibility.Visible;
                BtnEdit.Content = new SymbolIcon() { Symbol = Symbol.Contact };
            }
            else
            {
                ScrProfile.Visibility = Visibility.Visible;
                StpModify.Visibility = Visibility.Collapsed;
                BtnEdit.Content = new SymbolIcon() { Symbol = Symbol.Edit };
            }
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {     
            if (e.NewSize.Width < 520)
            {
                UsrViewer.SetSmaller();
            }
            else
            {
                UsrViewer.SetBigger();
            }
        }
    }
}
