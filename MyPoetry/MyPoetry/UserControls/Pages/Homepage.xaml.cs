using MyPoetry.Model;
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

// Il modello di elemento Pagina vuota è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=234238

namespace MyPoetry.UserControls.Pages
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class Homepage : Page
    {
        public Homepage()
        {
            this.InitializeComponent();
        }

        public CustomPage GetPage { get { return MainContent; } }



        private void ProgressBarVisible(bool visible)
        {
            ProgressRingPoetries.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            ProgressRingPoetries.IsActive = visible;
        }

        private async void MasterDetailView_Loaded(object sender, RoutedEventArgs e)
        {
            ProgressBarVisible(true);

            if (UserHandler.Instance.GetPoetries() == null)
            {
                List<Poetry> poetries = await App.MobileService.GetTable<Poetry>()
                    .Where(p => p.UserId == UserHandler.Instance.GetUser().Id)
                    .OrderByDescending(poetry => poetry.CreationDate)
                    .ToListAsync();
                UserHandler.Instance.SetPoetries(poetries);

                MasterDetailView.ItemsSource = poetries;
                //PoetriesListView.ItemsSource = poetries;

                //var groups = from c in UserHandler.Instance.GetPoetries() group c by c.CreationDate.Month;
                //PoetriesListView.ItemsSource = groups;
            }
            else
            {
                MasterDetailView.ItemsSource = null;
                MasterDetailView.ItemsSource = UserHandler.Instance.GetPoetries();
                //PoetriesListView.ItemsSource = null;
                //var groups = from c in UserHandler.Instance.GetPoetries() group c by c.CreationDate.Month;
                //PoetriesListView.ItemsSource = groups;
                //PoetriesListView.ItemsSource = UserHandler.Instance.GetPoetries();
            }

            ProgressBarVisible(false);
        }
    }

}
