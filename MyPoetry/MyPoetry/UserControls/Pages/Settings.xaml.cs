using Microsoft.WindowsAzure.MobileServices;
using MyPoetry.Model;
using MyPoetry.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using Windows.ApplicationModel.Resources;
using Windows.Globalization;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace MyPoetry.UserControls.Pages
{
    /// <summary>
    /// This class handles Settings page.
    /// </summary>
    public sealed partial class Settings : UserControl
    {
        private readonly Dictionary<string, string> languages = new Dictionary<string, string>()
        {
            { "Italiano [ITA]", "it" },
            { "English [ENG]", "en" },
            { "Polski [POL]", "pl" }
        };

        public Settings()
        {
            this.InitializeComponent();

            // Clears combobox
            CmbLanguageSelector.Items.Clear();

            // Loads languages
            CmbLanguageSelector.ItemsSource = languages;
            CmbLanguageSelector.DisplayMemberPath = "Key";
            CmbLanguageSelector.SelectedValuePath = "Value";

            // Handles selected language
            CmbLanguageSelector.SelectedValue = UserHandler.Instance.GetUser().LanguagePref != null ?
                UserHandler.Instance.GetUser().LanguagePref : CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
        }

        public CustomPage GetPage { get { return MainContent; } }

        private async void CmbLanguageSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Gets ISO 2-digits code
            string langCode = (string)CmbLanguageSelector.SelectedValue;
            // Sets application language
            ApplicationLanguages.PrimaryLanguageOverride = langCode;

            if (UserHandler.Instance.GetUser().LanguagePref != langCode)
            {
                if (Connection.HasInternetAccess)
                {
                    Exception exception = null;
                    HalfPageMessage hpm = new HalfPageMessage(GrdParent);
                    try
                    {
                        // Shows loading message
                        var loader = new ResourceLoader();
                        hpm.ShowMessage(loader.GetString("UpdatingLangPref"), loader.GetString("ServerConnection"), true, false, false, null, null);

                        // Updates user preference on server
                        UserHandler.Instance.GetUser().LanguagePref = langCode;
                        await App.MobileService.GetTable<User>().UpdateAsync(UserHandler.Instance.GetUser());
                    }
                    catch (MobileServiceInvalidOperationException ex)
                    {
                        exception = ex;
                    }
                    finally
                    {
                        hpm.Dismiss();
                        if (exception != null)
                        {
                            var msg = new MessageDialog(ServerErrorInfo.Instance.GetInfo(exception.Message));
                            await msg.ShowAsync();
                        }
                    }
                }
            }
        }

    }
}
