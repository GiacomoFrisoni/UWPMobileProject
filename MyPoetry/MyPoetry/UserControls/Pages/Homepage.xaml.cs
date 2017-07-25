using MyPoetry.Model;
using MyPoetry.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

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

        private void MasterDetailView_Loaded(object sender, RoutedEventArgs e)
        {
            // Loads poetries from server at the first loading...
            if (UserHandler.Instance.GetPoetries() == null)
            {
                RefreshMasterDetailItemsFromServer();
            }
            else
            {
                // Uses stored data otherwise
                MasterDetailView.ItemsSource = null;
                MasterDetailView.ItemsSource = UserHandler.Instance.GetPoetries();
            }
        }

        private async void RefreshMasterDetailItemsFromServer()
        {
            if (Connection.HasInternetAccess)
            {
                ProgressBarVisible(true);
                List<Poetry> poetries = await App.MobileService.GetTable<Poetry>()
                    .Where(p => p.UserId == UserHandler.Instance.GetUser().Id)
                    .OrderByDescending(poetry => poetry.CreationDate)
                    .ToListAsync();
                UserHandler.Instance.SetPoetries(poetries);
                MasterDetailView.ItemsSource = null;
                MasterDetailView.ItemsSource = poetries;
                ProgressBarVisible(false);
            }
            else
            {
                ((Frame)Window.Current.Content).Navigate(typeof(NoConnectionPage));
            }
        }
        #endregion

        #region Filters and Searching
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

        private List<Poetry> GetPoetriesWithCriteria(Criteria criteria, string text)
        {
            switch(criteria)
            {
                case Criteria.Date:
                    {
                        return UserHandler.Instance.GetPoetries()
                        .Where(p => p.UserId == UserHandler.Instance.GetUser().Id && (p.Title.ToLower().Contains(text.ToLower()) || p.Body.ToLower().Contains(text.ToLower())))
                        .OrderBy(poetry => poetry.CreationDate)
                        .ToList();
                    }

                case Criteria.DateDesc:
                    {
                        return UserHandler.Instance.GetPoetries()
                        .Where(p => p.UserId == UserHandler.Instance.GetUser().Id && (p.Title.ToLower().Contains(text.ToLower()) || p.Body.ToLower().Contains(text.ToLower())))
                        .OrderByDescending(poetry => poetry.CreationDate)
                        .ToList();
                    }

                case Criteria.Length:
                    {
                        return UserHandler.Instance.GetPoetries()
                        .Where(p => p.UserId == UserHandler.Instance.GetUser().Id && (p.Title.ToLower().Contains(text.ToLower()) || p.Body.ToLower().Contains(text.ToLower())))
                        .OrderBy(poetry => poetry.CharactersNumber)
                        .ToList();
                    }

                case Criteria.LengthDesc:
                    {
                        return UserHandler.Instance.GetPoetries()
                        .Where(p => p.UserId == UserHandler.Instance.GetUser().Id && (p.Title.ToLower().Contains(text.ToLower()) || p.Body.ToLower().Contains(text.ToLower())))
                        .OrderByDescending(poetry => poetry.CharactersNumber)
                        .ToList();
                    }
                case Criteria.Rating:
                    {
                        return UserHandler.Instance.GetPoetries()
                        .Where(p => p.UserId == UserHandler.Instance.GetUser().Id && (p.Title.ToLower().Contains(text.ToLower()) || p.Body.ToLower().Contains(text.ToLower())))
                        .OrderBy(poetry => poetry.Rating)
                        .ToList();
                    }

                case Criteria.RatingDesc:
                    {
                        return UserHandler.Instance.GetPoetries()
                        .Where(p => p.UserId == UserHandler.Instance.GetUser().Id && (p.Title.ToLower().Contains(text.ToLower()) || p.Body.ToLower().Contains(text.ToLower())))
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

        /*
         * Reset enabling is the actual solution for the softkeyboard closing on Windows 10 Mobile.
         * Link: https://stackoverflow.com/questions/18322559/how-to-programmatically-hide-the-keyboard
         */
        private void SearchBox_KeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                SearchBox.IsEnabled = false;
                SearchBox.IsEnabled = true;
            }
        }
        #endregion
        
        #region NewPoetry Shortcut
        private void BtnNewPoetry_Click(object sender, RoutedEventArgs e)
        {
            MenuHandler.Instance.SetMenuIndex(2);
        }
        #endregion

        #region Forward and Back Navigation
        public bool CanGoForward()
        {
            if (MasterDetailView.Items.Count > 0)
            {
                Poetry lastPoetry = MasterDetailView.Items[MasterDetailView.Items.Count - 1] as Poetry;
                return MasterDetailView.SelectedItem.Equals(lastPoetry) ? false : true;
            }
            return false;
        }

        public bool CanGoBack()
        {
            if (MasterDetailView.Items.Count > 0)
            {
                Poetry firstPoetry = MasterDetailView.Items[0] as Poetry;
                return MasterDetailView.SelectedItem.Equals(firstPoetry) ? false : true;
            }
            return false;
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

        /*
         * Utility method used to obtain the ListView contained inside of the MasterDetail
         * component. By doing so, we are able to make an automatic scroll even in case of
         * indirect navigation (with back and forward buttons).
         * See: https://stackoverflow.com/questions/45009500/masterdetailview-uwp-access-the-listview-or-make-it-scroll/45022012#45022012
         */
        public static T FindChildOfType<T>(DependencyObject root) where T : class
        {
            var queue = new Queue<DependencyObject>();
            queue.Enqueue(root);
            while (queue.Count > 0)
            {
                DependencyObject current = queue.Dequeue();
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(current); i++)
                {
                    var child = VisualTreeHelper.GetChild(current, i);
                    var typedChild = child as T;
                    if (typedChild != null)
                    {
                        return typedChild;
                    }
                    queue.Enqueue(child);
                }
            }
            return null;
        }

        private void MasterDetailView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var v = FindChildOfType<ListView>(MasterDetailView);
            v.ScrollIntoView(MasterDetailView.SelectedItem);
        }
        #endregion

        #region Refresh event
        private void PoetryViewer_RefreshEvent(object sender, EventArgs e)
        {
            RefreshMasterDetailItemsFromServer();
        }
        #endregion
    }

}
