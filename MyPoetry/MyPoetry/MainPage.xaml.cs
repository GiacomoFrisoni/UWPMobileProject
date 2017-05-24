
using MyPoetry.UserControls;
using MyPoetry.UserControls.Menu;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
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

            MenuList.SelectedIndex = 0;
        }

        CustomPage CurrentPage { get; set; }

        private void GenerateMenu()
        {
            List<MenuItem> menu = new List<MenuItem>();

            menu.Add(new MenuItem() { ItemText = "Home", ItemIcon = Symbol.Home, Group = MenuItem.Groups.Home, ItemPage = Create("Home") });
            menu.Add(new MenuItem() { ItemText = "Mie poesie", ItemIcon = Symbol.FontSize, Group = MenuItem.Groups.Home, ItemPage = Create("Mie poesie") });
            menu.Add(new MenuItem() { ItemText = "Mie plaquette", ItemIcon = Symbol.Folder, Group = MenuItem.Groups.Home, ItemPage = Create("Mie plaquette") });

            menu.Add(new MenuItem() { ItemText = "Nuova poesia", ItemIcon = Symbol.Add, Group = MenuItem.Groups.Create, ItemPage = Create("Test 4") });
            menu.Add(new MenuItem() { ItemText = "Nuova plaquette", ItemIcon = Symbol.Crop, Group = MenuItem.Groups.Create, ItemPage = Create("Test 5") });

            menu.Add(new MenuItem() { ItemText = "Dizionari", ItemIcon = Symbol.Font, Group = MenuItem.Groups.Explore, ItemPage = Create("Test 6") });
            menu.Add(new MenuItem() { ItemText = "Cerca rime", ItemIcon = Symbol.AllApps, Group = MenuItem.Groups.Explore, ItemPage = Create("Test 7") });

            var groups = from c in menu group c by c.Group;

            this.cvs.Source = groups;
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

        private CustomPage Create(string textone)
        {
            Grid grd = new Grid();
            TextBlock text = new TextBlock();
            text.Text = textone;
            grd.Children.Add(text);

            return new CustomPage(textone, grd);
        }

        
    }
}
