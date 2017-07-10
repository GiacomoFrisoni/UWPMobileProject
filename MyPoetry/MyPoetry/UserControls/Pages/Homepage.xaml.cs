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

        private List<Poetry> GetPoetriesWithCriteria (Criteria criteria, string text)
        {

            switch(criteria)
            {
                case Criteria.Date:
                    {
                        return UserHandler.Instance.GetPoetries()
                        .Where(p => p.UserId == UserHandler.Instance.GetUser().Id && (p.Title.ToLower().Contains(text) || p.Body.ToLower().Contains(text)))
                        .OrderBy(poetry => poetry.CreationDate)
                        .ToList();
                    }

                case Criteria.DateDesc:
                    {
                        return UserHandler.Instance.GetPoetries()
                        .Where(p => p.UserId == UserHandler.Instance.GetUser().Id && (p.Title.ToLower().Contains(text) || p.Body.ToLower().Contains(text)))
                        .OrderByDescending(poetry => poetry.CreationDate)
                        .ToList();
                    }

                case Criteria.Length:
                    {
                        return UserHandler.Instance.GetPoetries()
                        .Where(p => p.UserId == UserHandler.Instance.GetUser().Id && (p.Title.ToLower().Contains(text) || p.Body.ToLower().Contains(text)))
                        .OrderBy(poetry => poetry.CharactersNumber)
                        .ToList();
                    }

                case Criteria.LengthDesc:
                    {
                        return UserHandler.Instance.GetPoetries()
                        .Where(p => p.UserId == UserHandler.Instance.GetUser().Id && (p.Title.ToLower().Contains(text) || p.Body.ToLower().Contains(text)))
                        .OrderByDescending(poetry => poetry.CharactersNumber)
                        .ToList();
                    }
                case Criteria.Rating:
                    {
                        return UserHandler.Instance.GetPoetries()
                        .Where(p => p.UserId == UserHandler.Instance.GetUser().Id && (p.Title.ToLower().Contains(text) || p.Body.ToLower().Contains(text)))
                        .OrderBy(poetry => poetry.Rating)
                        .ToList();
                    }

                case Criteria.RatingDesc:
                    {
                        return UserHandler.Instance.GetPoetries()
                        .Where(p => p.UserId == UserHandler.Instance.GetUser().Id && (p.Title.ToLower().Contains(text) || p.Body.ToLower().Contains(text)))
                        .OrderByDescending(poetry => poetry.Rating)
                        .ToList();
                    }

                default: return null;
            }
        }

        private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            ProgressBarVisible(true);
            MasterDetailView.ItemsSource = null;
            MasterDetailView.ItemsSource = GetPoetriesWithCriteria(GetCriteriaFromOption(CmbOrderby.SelectedIndex), SearchBox.Text);
            ProgressBarVisible(false);
        }


        public void GoForward()
        {
            if (MasterDetailView.Items.Count > 0)
            {
                bool isNextGood = false;
                Poetry selectedPoetry = MasterDetailView.Items[MasterDetailView.Items.Count - 1] as Poetry;

                foreach (var v in MasterDetailView.Items)
                {
                    if (isNextGood)
                    {
                        selectedPoetry = v as Poetry;
                        break;
                    }

                    if (v == MasterDetailView.SelectedItem)
                        isNextGood = true;
                }

                MasterDetailView.SelectedItem = selectedPoetry;
                
            }
        }

        public void GoBack()
        {
            if (MasterDetailView.Items.Count > 0)
            {
                bool isPrevGood = false;
                Poetry selectedPoetry = MasterDetailView.Items[0] as Poetry;

                foreach (var v in MasterDetailView.Items)
                {
                    if (v == MasterDetailView.SelectedItem)
                        isPrevGood = true;

                    if (isPrevGood == false)
                        selectedPoetry = v as Poetry;
                    else
                        break;
                }

                MasterDetailView.SelectedItem = selectedPoetry;
            }
        }


        private void PoetryViewer_BackEvent(object sender, EventArgs e)
        {
            GoBack();
        }

        private void PoetryViewer_ForwardEvent(object sender, EventArgs e)
        {
            GoForward();
        }

        private void MasterDetailView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }

}
