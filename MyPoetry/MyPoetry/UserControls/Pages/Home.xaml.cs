using MyPoetry.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace MyPoetry.UserControls.Pages
{
    public sealed partial class Home : UserControl
    {
        public Home()
        {
            this.InitializeComponent();
        }

        public CustomPage GetPage { get { return MainContent; } }


        public class Poetry
        {
            public string Title { get; set; }
            public string ShortText { get; set; }
        }
        

        private void itemListView_Loaded(object sender, RoutedEventArgs e)
        {
            itemListView.Items.Clear();

            List<Poetry> poetries = new List<Poetry>();
            poetries.Add(new Poetry() { Title = "Ciao", ShortText = "Minacciata da cose strane\nStavo a nascondere i cadaveri delle persone\nche mi stavano\nsul cazzo" });
            poetries.Add(new Poetry() { Title = "Buonaseeeeeeeraaaaaaa", ShortText = "Minacciata da Alice Gori\nStavo scrivendo righe immensamente lunghe\nche mi stavano sul cazzo in una maniera impressionante\nmi ammazzo\ntantissimo poi, che non riesco\npiù a sopportare\nnessuno" });
            poetries.Add(new Poetry() { Title = "Magari fossi in ritardo estremo!", ShortText = "Minacciata da cose strane\nStavo a nascondere i cadaveri delle persone\nche mi stavano\nsul cazzo" });
            poetries.Add(new Poetry() { Title = "Titolo completamente lungo a casissimo", ShortText = "Minacciata da cose strane\nStavo a nascondere i cadaveri delle persone\nche mi stavano\nsul cazzo\nmi ammazzo\ntantissimo poi, che non riesco\npiù a sopportare\nnessuno\nma proprio nesusno" });
            poetries.Add(new Poetry() { Title = "Ancora un altro titolo completamente lungo completamente a caso", ShortText = "Minacciata da cose strane\nStavo a nascondere i cadaveri delle persone\nche mi stavano\nsul cazzo" });

            itemListView.ItemsSource = poetries;

        }
    }
}
