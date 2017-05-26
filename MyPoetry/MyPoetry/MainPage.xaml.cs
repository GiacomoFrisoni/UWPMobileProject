
using MyPoetry.UserControls;
using MyPoetry.UserControls.Menu;
using MyPoetry.UserControls.Pages;
using MyPoetry.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// Il modello di elemento Pagina vuota è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x410

namespace MyPoetry
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            this.InitializeComponent();

            GenerateMenu();       
        }

        CustomPage CurrentPage { get; set; }


        private async void GenerateMenu()
        {
            //Temp list for binding
            List<MenuItem> menu = new List<MenuItem>();

            //Retriving user data
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = await ImageFromBytes(UserHandler.Instance.GetUser().Photo);
            string user = UserHandler.Instance.GetUser().Name + " " + UserHandler.Instance.GetUser().Surname;

            //Creating menu groups
            menu.Add(new MenuItem() { ItemText = user, ItemImage = ib.ImageSource, Group = MenuItem.Groups.User, ItemPage = new Home().GetPage });

            menu.Add(new MenuItem() { ItemText = "Home", ItemIcon = Symbol.Home, Group = MenuItem.Groups.Home, ItemPage = new Home().GetPage  });
            menu.Add(new MenuItem() { ItemText = "Mie poesie", ItemIcon = Symbol.FontSize, Group = MenuItem.Groups.Home, ItemPage = new Home().GetPage });
            menu.Add(new MenuItem() { ItemText = "Mie plaquette", ItemIcon = Symbol.Folder, Group = MenuItem.Groups.Home, ItemPage = new Home().GetPage });

            menu.Add(new MenuItem() { ItemText = "Nuova poesia", ItemIcon = Symbol.Add, Group = MenuItem.Groups.Create, ItemPage = new Editor().GetPage });
            menu.Add(new MenuItem() { ItemText = "Nuova plaquette", ItemIcon = Symbol.Crop, Group = MenuItem.Groups.Create, ItemPage = new Home().GetPage });

            menu.Add(new MenuItem() { ItemText = "Dizionari", ItemIcon = Symbol.Font, Group = MenuItem.Groups.Explore, ItemPage = new Home().GetPage });
            menu.Add(new MenuItem() { ItemText = "Cerca rime", ItemIcon = Symbol.AllApps, Group = MenuItem.Groups.Explore, ItemPage = new Home().GetPage });

            menu.Add(new MenuItem() { ItemText = "Impostazini", ItemIcon = Symbol.Setting, Group = MenuItem.Groups.Settings, ItemPage = new Home().GetPage });

            //Settings groups
            var groups = from c in menu group c by c.Group;
            this.cvs.Source = groups;

            //Selecting HOME
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
    }
}
