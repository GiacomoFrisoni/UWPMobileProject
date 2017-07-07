using MyPoetry.Model;
using MyPoetry.Utilities;
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


        #region LoadingMasterDetail
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

            }
            else
            {
                MasterDetailView.ItemsSource = null;
                MasterDetailView.ItemsSource = UserHandler.Instance.GetPoetries();
            }

            ProgressBarVisible(false);
        }
        #endregion


        #region ButtonClick events
        private void BtnShare_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDetails_Click(object sender, RoutedEventArgs e)
        {
            
        }
        #endregion

        private void CmbOrderby_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProgressBarVisible(true);
            FlyOrderby.Hide();
            MasterDetailView.ItemsSource = null;
            MasterDetailView.ItemsSource = GetPoetriesWithCriteria(GetCriteriaFromOption(CmbOrderby.SelectedIndex), SearchBox.Text);
            ProgressBarVisible(false);
        }


        private enum Criteria
        {
            Date, DateDesc, Length, LengthDesc, Rating, RatingDesc
        }

        private Criteria GetCriteriaFromOption(int option)
        {
            switch (option)
            {
                case 0: return Criteria.DateDesc;
                case 1: return Criteria.Date;
                case 2: return Criteria.LengthDesc;
                case 3: return Criteria.Length;
                case 4: return Criteria.RatingDesc;
                case 5: return Criteria.Rating;
                default: return Criteria.Date;
            }
        }

        private async Task<List<Poetry>> GetPoetriesWithCriteria (Criteria criteria, string text)
        {
            switch(criteria)
            {
                case Criteria.Date:
                    {
                        return await App.MobileService.GetTable<Poetry>()
                        .Where(p => p.UserId == UserHandler.Instance.GetUser().Id && (p.Title.ToLower().Contains(text) || p.Body.ToLower().Contains(text)))
                        .OrderBy(poetry => poetry.CreationDate)
                        .ToListAsync();
                    }

                case Criteria.DateDesc:
                    {
                        return await App.MobileService.GetTable<Poetry>()
                        .Where(p => p.UserId == UserHandler.Instance.GetUser().Id && (p.Title.ToLower().Contains(text) || p.Body.ToLower().Contains(text)))
                        .OrderByDescending(poetry => poetry.CreationDate)
                        .ToListAsync();
                    }

                case Criteria.Length:
                    {
                        return await App.MobileService.GetTable<Poetry>()
                        .Where(p => p.UserId == UserHandler.Instance.GetUser().Id && (p.Title.ToLower().Contains(text) || p.Body.ToLower().Contains(text)))
                        .OrderBy(poetry => poetry.CharactersNumber)
                        .ToListAsync();
                    }

                case Criteria.LengthDesc:
                    {
                        return await App.MobileService.GetTable<Poetry>()
                        .Where(p => p.UserId == UserHandler.Instance.GetUser().Id && (p.Title.ToLower().Contains(text) || p.Body.ToLower().Contains(text)))
                        .OrderByDescending(poetry => poetry.CharactersNumber)
                        .ToListAsync();
                    }
                case Criteria.Rating:
                    {
                        return await App.MobileService.GetTable<Poetry>()
                        .Where(p => p.UserId == UserHandler.Instance.GetUser().Id && (p.Title.ToLower().Contains(text) || p.Body.ToLower().Contains(text)))
                        .OrderBy(poetry => poetry.Rating)
                        .ToListAsync();
                    }

                case Criteria.RatingDesc:
                    {
                        return await App.MobileService.GetTable<Poetry>()
                        .Where(p => p.UserId == UserHandler.Instance.GetUser().Id && (p.Title.ToLower().Contains(text) || p.Body.ToLower().Contains(text)))
                        .OrderByDescending(poetry => poetry.Rating)
                        .ToListAsync();
                    }

                default: return null;
            }
        }
    }

}
