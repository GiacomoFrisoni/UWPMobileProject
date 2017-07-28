using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MyPoetry.UserControls.Pages
{
    /// <summary>
    /// This class handles Credits page.
    /// </summary>
    public sealed partial class Credits : UserControl
    {
        public Credits()
        {
            this.InitializeComponent();
        }

        public CustomPage GetPage { get { return MainContent; } }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            //Gridview credits
            CreditsGridView.ItemsSource = null;
            CreditsGridView.ItemsSource = GenerateCreditsInfo();
        }

        private List<object> GenerateCreditsInfo()
        {
            List<object> info = new List<object>();

            info.Add(new { Title = "Giacomo Frisoni", Details = "Developer", Email = "giacomo.frisoni@studio.unibo.it", EmailLink = new Uri("mailto:giacomo.frisoni@studio.unibo.it"), Photo = new Uri("ms-appx:///Assets/Credits/giacomo_frisoni.jpg") });
            info.Add(new { Title = "Marcin Pabich", Details = "Developer", Email = "marcintomasz.pabich@studio.unibo.it", EmailLink = new Uri("mailto:marcintomasz.pabich@studio.unibo.it"), Photo = new Uri("ms-appx:///Assets/Credits/marcin_pabich.jpg") });

            return info;
        }
    }
}
