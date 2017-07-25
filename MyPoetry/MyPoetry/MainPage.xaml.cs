using MyPoetry.UserControls;
using MyPoetry.UserControls.Menu;
using MyPoetry.UserControls.Pages;
using MyPoetry.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace MyPoetry
{
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
            // Temp list for binding
            List<MenuItem> menu = new List<MenuItem>();

            // Retriving user data
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = await ImageHelper.ImageFromBytes(UserHandler.Instance.GetUser().Photo);
            string user = UserHandler.Instance.GetUser().Name + " " + UserHandler.Instance.GetUser().Surname;

            // Creating menu groups
            var loader = new ResourceLoader();
            menu.Add(new MenuItem() { ItemText = user, ItemImage = ib.ImageSource, Group = MenuItem.Groups.User, ItemPage = new Profile().GetPage });
            menu.Add(new MenuItem() { ItemText = loader.GetString("Home"), ItemIcon = Symbol.Home, Group = MenuItem.Groups.Home, ItemPage = new Homepage().GetPage  });
            menu.Add(new MenuItem() { ItemText = loader.GetString("NewPoetry"), ItemIcon = Symbol.Add, Group = MenuItem.Groups.Create, ItemPage = new Editor().GetPage });
            menu.Add(new MenuItem() { ItemText = loader.GetString("Settings"), ItemIcon = Symbol.Setting, Group = MenuItem.Groups.Settings, ItemPage = new Settings().GetPage });
            menu.Add(new MenuItem() { ItemText = loader.GetString("Credits"), ItemIcon = Symbol.Emoji2, Group = MenuItem.Groups.Settings, ItemPage = new Credits().GetPage });

            // Settings groups
            var groups = from c in menu group c by c.Group;
            this.cvs.Source = groups;

            // Selecting Home
            MenuList.SelectedIndex = 1;
            menuSelectedIndex = 1;

            // Sets menu inside singleton
            MenuHandler.Instance.SetMenu(MenuList);
        }
        
        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationPane.IsPaneOpen = !NavigationPane.IsPaneOpen;
        }

        private async void MenuList_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
