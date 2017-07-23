using Microsoft.Toolkit.Uwp;
using MyPoetry.Model;
using MyPoetry.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace MyPoetry.UserControls.Pages
{
    public sealed partial class Profile : UserControl
    {
        public Profile()
        {
            this.InitializeComponent();
        }

        public CustomPage GetPage { get { return MainContent; } }
        
        public class InfoViewer
        {
            public string Description { get; set; }
            public Symbol Icon { get; set; }
            public string Value { get; set; }

            public InfoViewer(string title, Symbol icon, string value)
            {
                Description = title;
                Icon = icon;
                Value = value;
            }
        }

        private async void LoadData()
        {
            // Loads profile image
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = await ImageHelper.ImageFromBytes(UserHandler.Instance.GetUser().Photo);
            UsrViewer.ImageSource = ib;
            UsrViewer.Title = UserHandler.Instance.GetUser().Name + " " + UserHandler.Instance.GetUser().Surname;
            UsrViewer.Details = UserHandler.Instance.GetUser().Email;    

            // Generates statistic elements inside the gridview
            ProfileGridView.ItemsSource = null;
            ProfileGridView.ItemsSource = GenerateAdvancedInfo();
        }
        
        private List<DataViewer> GenerateAdvancedInfo()
        {
            List<DataViewer> info = new List<DataViewer>();
            info.Add(new DataViewer("L'anima d'ispirazione", "Scrivi dal " + UserHandler.Instance.GetUser().RegistrationDate.ToString("dd/MM/yyyy"), Symbol.Calendar, new SolidColorBrush(ColorHelper.ToColor("#1BBC9B"))));
            info.Add(new DataViewer("Scrittore nato", "Hai scritto " + UserHandler.Instance.GetPoetries().Count.ToString() + " poesie", Symbol.Edit, new SolidColorBrush(ColorHelper.ToColor("#2DCC70"))));
            info.Add(new DataViewer("Distruttore della tastiera", "Hai immesso " + UserHandler.Instance.GetPoetries().Sum(poetry => poetry.CharactersNumber).ToString() + " caratteri", Symbol.Font, new SolidColorBrush(ColorHelper.ToColor("#3598DB"))));
            info.Add(new DataViewer("Sagge parole", "Hai utilizzato " + UserHandler.Instance.GetPoetries().Sum(poetry => poetry.WordsNumber).ToString(), Symbol.FontColor, new SolidColorBrush(ColorHelper.ToColor("#9B58B5"))));
            info.Add(new DataViewer("Io vado a capo", "Il numero dei tuoi versi: " + UserHandler.Instance.GetPoetries().Sum(poetry => poetry.VersesNumber).ToString(), Symbol.ShowResults, new SolidColorBrush(ColorHelper.ToColor("#34495E"))));

            info.Add(new DataViewer("La poesia più corta", UserHandler.Instance.GetPoetries().OrderBy(poetry => poetry.CharactersNumber).First().Title, Symbol.Remove, new SolidColorBrush(ColorHelper.ToColor("#F1C40F"))));
            info.Add(new DataViewer("La poesia più lunga", UserHandler.Instance.GetPoetries().OrderByDescending(poetry => poetry.CharactersNumber).First().Title, Symbol.List, new SolidColorBrush(ColorHelper.ToColor("#E77E23"))));

            return info;
        }


        private void ProgressBarVisible(bool visible)
        {
            ProgressRingProfile.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            ProgressRingProfile.IsActive = visible;
        }


        private void BtnDeletePhoto_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnFile_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnPhoto_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (GrdProfile.Visibility == Visibility.Visible)
            {
                GrdProfile.Visibility = Visibility.Collapsed;
                StpModify.Visibility = Visibility.Visible;
                BtnEdit.Content = new SymbolIcon() { Symbol = Symbol.Save };
            }
            else
            {
                GrdProfile.Visibility = Visibility.Visible;
                StpModify.Visibility = Visibility.Collapsed;
                BtnEdit.Content = new SymbolIcon() { Symbol = Symbol.Edit };
            }
        }
    }
}
