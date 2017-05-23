
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
        }

        private void GenerateMenu()
        {
            List<MenuItem> menu = new List<MenuItem>();

            menu.Add(new MenuItem() { ItemText = "Home", ItemIcon = Symbol.Home, Group = MenuItem.Groups.Home });
            menu.Add(new MenuItem() { ItemText = "Mie poesie", ItemIcon = Symbol.FontSize, Group = MenuItem.Groups.Home });
            menu.Add(new MenuItem() { ItemText = "Mie plaquette", ItemIcon = Symbol.Folder, Group = MenuItem.Groups.Home });

            menu.Add(new MenuItem() { ItemText = "Nuova poesia", ItemIcon = Symbol.Add, Group = MenuItem.Groups.Create });
            menu.Add(new MenuItem() { ItemText = "Nuova plaquette", ItemIcon = Symbol.Crop, Group = MenuItem.Groups.Create });

            menu.Add(new MenuItem() { ItemText = "Dizionari", ItemIcon = Symbol.Font, Group = MenuItem.Groups.Explore });
            menu.Add(new MenuItem() { ItemText = "Cerca rime", ItemIcon = Symbol.AllApps, Group = MenuItem.Groups.Explore });

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
                TxblTitle.Text = ((MenuItem)MenuList.SelectedItem).ItemText;
        }
    }
}
