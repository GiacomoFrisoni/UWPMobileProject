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

// Il modello di elemento Pagina vuota è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=234238

namespace MyPoetry
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class LostConnectionPage : Page
    {
        public LostConnectionPage()
        {
            this.InitializeComponent();
        }

        private void BtnReconnect_Click(object sender, RoutedEventArgs e)
        {
            BtnReconnect.Visibility = Visibility.Collapsed;
            PrgRing.Visibility = Visibility.Visible;

            if (CheckInternet())
            {

            }
            else
            {
                BtnReconnect.Visibility = Visibility.Visible;
                PrgRing.Visibility = Visibility.Collapsed;
                TxbError.Visibility = Visibility.Visible;
            }
        }

        private bool CheckInternet()
        {
            return true;
        }
    }
}
