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
    public sealed partial class Editor : UserControl
    {
        public Editor()
        {
            this.InitializeComponent();
        }

        public CustomPage GetPage { get { return MainContent; } }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Window.Current.Bounds.Width <= 700)
                SpvContent.IsPaneOpen = !SpvContent.IsPaneOpen;
            else
                SpvContent.IsPaneOpen = true;
        }
    }
}
