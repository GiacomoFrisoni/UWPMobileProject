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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MyPoetry.UserControls.Pages
{
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

            info.Add(new { Title = "Giacomo Frisoni", Details = "Developer" });
            info.Add(new { Title = "Marcin Pabich", Details = "Developer" });

            return info;
        }
    }
}
