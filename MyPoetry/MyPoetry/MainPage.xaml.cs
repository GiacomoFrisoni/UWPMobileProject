using MyPoetry.UserControls;
using MyPoetry.UserControls.Menu;
using MyPoetry.UserControls.Pages;
using MyPoetry.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace MyPoetry
{
    public sealed partial class MainPage : Page
    {
        private AppLocalSettings settings;

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
            ib.ImageSource = await ImageFromBytes(UserHandler.Instance.GetUser().Photo);
            string user = UserHandler.Instance.GetUser().Name + " " + UserHandler.Instance.GetUser().Surname;

            // Creating menu groups
            var loader = new ResourceLoader();
            menu.Add(new MenuItem() { ItemText = user, ItemImage = ib.ImageSource, Group = MenuItem.Groups.User, ItemPage = new Profile().GetPage });

            menu.Add(new MenuItem() { ItemText = loader.GetString("Home"), ItemIcon = Symbol.Home, Group = MenuItem.Groups.Home, ItemPage = new Home().GetPage  });
            menu.Add(new MenuItem() { ItemText = loader.GetString("MyPoetries"), ItemIcon = Symbol.Folder, Group = MenuItem.Groups.Home, ItemPage = new PoetryDetail().GetPage });

            menu.Add(new MenuItem() { ItemText = loader.GetString("NewPoetry"), ItemIcon = Symbol.Add, Group = MenuItem.Groups.Create, ItemPage = new Editor().GetPage });

            menu.Add(new MenuItem() { ItemText = loader.GetString("Settings"), ItemIcon = Symbol.Setting, Group = MenuItem.Groups.Settings, ItemPage = new Settings().GetPage });

            // Settings groups
            var groups = from c in menu group c by c.Group;
            this.cvs.Source = groups;

            // Selecting Home
            MenuList.SelectedIndex = 1;
        }

        private async static Task<BitmapImage> ImageFromBytes(Byte[] bytes)
        {
            BitmapImage image = new BitmapImage();
            using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
            {
                await stream.WriteAsync(bytes.AsBuffer());
                stream.Seek(0);
                await image.SetSourceAsync(stream);
            }
            return image;
        }
        
        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationPane.IsPaneOpen = !NavigationPane.IsPaneOpen;
        }

        private void MenuList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MenuList.SelectedItem.GetType().Equals(typeof(MenuItem)))
            {
                CurrentPage = ((MenuItem)MenuList.SelectedItem).ItemPage;
                this.Bindings.Update();
                NavigationPane.IsPaneOpen = false;
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
