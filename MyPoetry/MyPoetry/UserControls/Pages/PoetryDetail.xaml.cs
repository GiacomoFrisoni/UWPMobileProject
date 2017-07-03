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
    public sealed partial class PoetryDetail : UserControl
    {
        public PoetryDetail()
        {
            this.InitializeComponent();

            RatingControl.FilledImage = new Uri("ms-appx:///Assets/Rating/staron.png");
            RatingControl.EmptyImage = new Uri("ms-appx:///Assets/Rating/staroff.png");
        }

        public CustomPage GetPage { get { return MainContent; } }

        private void BtnDetails_Click(object sender, RoutedEventArgs e)
        {
            SplitView.IsPaneOpen = !SplitView.IsPaneOpen;
        }

        private void FlpPoetries_Loaded(object sender, RoutedEventArgs e)
        {
            FlpPoetries.Items.Add(new PoetryViewer() { Title = "Titolo a caso", Text = "Minacciata da cose strane\nStavo a nascondere i cadaveri delle persone\nche mi stavano\nsul cazzo\nmi ammazzo\ntantissimo poi, che non riesco\npiù a sopportare\nnessuno\nma proprio nesusno" });
            FlpPoetries.Items.Add(new PoetryViewer() { Title = "Titolo a caso", Text = "Minacciata da cose strane\nStavo a nascondere i cadaveri delle persone\nche mi stavano\nsul cazzo\nmi ammazzo\ntantissimo poi, che non riesco\npiù a sopportare\nnessuno\nma proprio nesusno" });
            FlpPoetries.Items.Add(new PoetryViewer() { Title = "Titolo a caso", Text = "Minacciata da cose strane\nStavo a nascondere i cadaveri delle persone\nche mi stavano\nsul cazzo\nmi ammazzo\ntantissimo poi, che non riesco\npiù a sopportare\nnessuno\nma proprio nesusno" });
            FlpPoetries.Items.Add(new PoetryViewer() { Title = "Titolo a caso", Text = "Minacciata da cose strane\nStavo a nascondere i cadaveri delle persone\nche mi stavano\nsul cazzo\nmi ammazzo\ntantissimo poi, che non riesco\npiù a sopportare\nnessuno\nma proprio nesusno" });
            FlpPoetries.Items.Add(new PoetryViewer() { Title = "Titolo a caso", Text = "Minacciata da cose strane\nStavo a nascondere i cadaveri delle persone\nche mi stavano\nsul cazzo\nmi ammazzo\ntantissimo poi, che non riesco\npiù a sopportare\nnessuno\nma proprio nesusno" });
            FlpPoetries.Items.Add(new PoetryViewer() { Title = "Titolo a caso", Text = "Minacciata da cose strane\nStavo a nascondere i cadaveri delle persone\nche mi stavano\nsul cazzo\nmi ammazzo\ntantissimo poi, che non riesco\npiù a sopportare\nnessuno\nma proprio nesusno" });
            FlpPoetries.Items.Add(new PoetryViewer() { Title = "Titolo a caso", Text = "Minacciata da cose strane\nStavo a nascondere i cadaveri delle persone\nche mi stavano\nsul cazzo\nmi ammazzo\ntantissimo poi, che non riesco\npiù a sopportare\nnessuno\nma proprio nesusno" });
            FlpPoetries.Items.Add(new PoetryViewer() { Title = "Titolo a caso", Text = "Minacciata da cose strane\nStavo a nascondere i cadaveri delle persone\nche mi stavano\nsul cazzo\nmi ammazzo\ntantissimo poi, che non riesco\npiù a sopportare\nnessuno\nma proprio nesusno" });
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnEdit_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void BtnShare_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
