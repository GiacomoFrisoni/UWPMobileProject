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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MyPoetry.UserControls.Pages
{
    public sealed partial class Profile : UserControl
    {
        public Profile()
        {
            this.InitializeComponent();
        }

        public CustomPage GetPage { get { return MainContent; } }


        private async void LoadData()
        {
            ProgressBarVisible(true);

            if (UserHandler.Instance.GetPoetries() == null)
            {
                List<Poetry> poetries = await App.MobileService.GetTable<Poetry>()
                    .Where(p => p.UserId == UserHandler.Instance.GetUser().Id)
                    .ToListAsync();
                UserHandler.Instance.SetPoetries(poetries);
            }


            //Immagine
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = await ImageHelper.ImageFromBytes(UserHandler.Instance.GetUser().Photo);
            ImgImage.Source = ib.ImageSource;
            ImgProfile.Source = ib.ImageSource;

            //Nome e cognome
            TxbUser.Text = UserHandler.Instance.GetUser().Name + " " + UserHandler.Instance.GetUser().Surname;
            TxbName.Text = UserHandler.Instance.GetUser().Name;
            TxbSurname.Text = UserHandler.Instance.GetUser().Surname;

            //Gender
            CmbGender.SelectedIndex = UserHandler.Instance.GetUser().Gender == "M" ? 0 : 1;

            //Mail
            TxbMail.Text = UserHandler.Instance.GetUser().Email;

            //Data registrazione
            TxbRegisterDate.Text = UserHandler.Instance.GetUser().RegistrationDate.ToString("dd/MM/yyyy");




            //Poesie scritte
            TxbPoetriesNumber.Text = UserHandler.Instance.GetPoetries().Count.ToString();

            //Lunghezza complessiva (char)
            TxbCharsNumber.Text = UserHandler.Instance.GetPoetries().Sum(poetry => poetry.CharactersNumber).ToString();

            //Parole utilizzate
            TxbWordsNumber.Text = UserHandler.Instance.GetPoetries().Sum(poetry => poetry.WordsNumber).ToString();

            //Numero versi
            TxbVersesNumber.Text = UserHandler.Instance.GetPoetries().Sum(poetry => poetry.VersesNumber).ToString();

            //Quella più lunga
            TxbLongest.Text = UserHandler.Instance.GetPoetries().OrderByDescending(poetry => poetry.CharactersNumber).First().Title;

            //Quella più corta
            TxbShortest.Text = UserHandler.Instance.GetPoetries().OrderBy(poetry => poetry.CharactersNumber).First().Title;

            //Giorno di ispirazione
            TxbInspiration.Text = "Smetti di scrivere...";

            ProgressBarVisible(false);
        }


        private void ProgressBarVisible(bool visible)
        {
            ProgressRingProfile.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            ProgressRingProfile.IsActive = visible;
        }

        private void Pivot_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
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
    }
}
