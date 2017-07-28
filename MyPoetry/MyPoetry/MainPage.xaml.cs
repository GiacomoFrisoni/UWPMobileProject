using MyPoetry.UserControls;
using MyPoetry.UserControls.Menu;
using MyPoetry.UserControls.Pages;
using MyPoetry.Utilities;
using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace MyPoetry
{
    /// <summary>
    /// This class handles the main navigation structure of the application.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private AppLocalSettings settings;
        private int menuSelectedIndex;

        public MainPage()
        {
            this.InitializeComponent();
            settings = new AppLocalSettings();
            GenerateMenu();
        }

        CustomPage CurrentPage { get; set; }
        
        private async void GenerateMenu()
        {
            // Retriving user data
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = await ImageHelper.ImageFromBytes(UserHandler.Instance.GetUser().Photo);
            ib.Stretch = Stretch.UniformToFill;
            string username = UserHandler.Instance.GetUser().Name + " " + UserHandler.Instance.GetUser().Surname;

            // Settings reference to menu
            MenuHandler.Instance.SetMenu(MenuList);
            MenuHandler.Instance.SetCollectionViewSource(this.cvs);

            //Creating menu with updated profile
            MenuHandler.Instance.CreateMenu(new MenuItem() { ItemText = username, ItemImage = ib.ImageSource, Group = MenuItem.Groups.User, ItemPage = new Profile().GetPage });

            // Selecting Home as default screen
            MenuList.SelectedIndex = 1;
            menuSelectedIndex = 1;
        }
        
        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationPane.IsPaneOpen = !NavigationPane.IsPaneOpen;
        }

        private async void MenuList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MenuList.SelectedItem != null)
            {
                if (MenuList.SelectedItem.GetType().Equals(typeof(MenuItem)))
                {
                    var oldIndex = menuSelectedIndex;
                    menuSelectedIndex = MenuList.SelectedIndex;
                    if (oldIndex == 2 && UserHandler.Instance.IsPoetryInEditing())
                    {
                        var loader = new ResourceLoader();

                        // Create the message dialog
                        MessageDialog messageDialog = new MessageDialog(loader.GetString("ExitFromEditorConfirm"), loader.GetString("Confirm"));

                        // YES command
                        messageDialog.Commands.Add(new UICommand(loader.GetString("Yes"), (command) =>
                        {
                            UserHandler.Instance.SetPoetryInEditing(false);
                            if (UserHandler.Instance.GetPoetryToEdit() != null)
                                UserHandler.Instance.SetPoetryToEdit(null);
                            CurrentPage = ((MenuItem)MenuList.SelectedItem).ItemPage;
                            this.Bindings.Update();
                            NavigationPane.IsPaneOpen = false;
                        }));

                        // NO command
                        messageDialog.Commands.Add(new UICommand(loader.GetString("No"), (command) =>
                        {
                            MenuList.SelectedIndex = oldIndex;
                        }));

                        // Set the command that will be invoked by default
                        messageDialog.DefaultCommandIndex = 1;

                        // Show the message dialog
                        await messageDialog.ShowAsync();
                    }
                    else
                    {
                        CurrentPage = ((MenuItem)MenuList.SelectedItem).ItemPage;
                        this.Bindings.Update();
                        NavigationPane.IsPaneOpen = false;
                    }
                }
            }
        }

        private void BtnLogout_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            // Reset user data
            UserHandler.Instance.SetUser(null);
            // Reset keep login
            settings.SetUserLoggedId(String.Empty);
            // Go to login page
            this.Frame.Navigate(typeof(LoginPage));
        }
    }
}
