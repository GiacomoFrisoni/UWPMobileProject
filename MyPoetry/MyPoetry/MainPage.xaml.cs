
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
            List<GroupCollector<object>> menu = new List<GroupCollector<object>>();

            GroupCollector<object> main = new GroupCollector<object>();
            main.Key = "Principale";
            main.Add(new MenuItem("Home", Symbol.Home));
            main.Add(new MenuItem("Le mie creazioni", Symbol.Highlight));
            main.Add(new MenuItem("Le mie plaquette", Symbol.Import));
            menu.Add(main);

            GroupCollector<object> create = new GroupCollector<object>();
            create.Key = "Crea";
            create.Add(new MenuItem("Nuova poesia", Symbol.Add));
            create.Add(new MenuItem("Nuova plaquette", Symbol.Add));
            menu.Add(create);

            GroupCollector<object> explore = new GroupCollector<object>();
            explore.Key = "Esplora";
            explore.Add(new MenuItem("Dizionari", Symbol.Directions));
            explore.Add(new MenuItem("Cerca rime", Symbol.DockRight));
            menu.Add(explore);

            var cvs = (CollectionViewSource)Resources["itemsViewSource"];
            cvs.Source = menu;
        }

        /*
        public List<GroupInfoList<object>> GetFriendsGrouped()
        {
            if (_friends == null)
            {
                _friends = await NetworkManager.GetFriends();
            }

            List<GroupInfoList<object>> friendsGrouped = new List<GroupInfoList<object>>();

            var query = from friend in _friends
                        orderby ((Friend)friend).first_name
                        group friend by ((Friend)friend).first_name[0] into g
                        select new { GroupName = g.Key, Items = g };
            foreach (var g in query)
            {
                GroupInfoList<object> info = new GroupInfoList<object>();
                info.Key = g.GroupName;
                foreach (var friend in g.Items)
                {
                    info.Add(friend);
                }
                friendsGrouped.Add(info);
            }
            return friendsGrouped;
        }*/

        private void HamburgerMenuControl_ItemClick(object sender, ItemClickEventArgs e)
        {
            /*var menuItem = e.ClickedItem as MenuItem;
            ContentFrame.Navigate(menuItem.Page);
            TxblTitle.Text = menuItem.Text;*/
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           /* HamburgerMenuControl.IsPaneOpen = !HamburgerMenuControl.IsPaneOpen;*/
        }
    }
}
