using MyPoetry.Model;
using MyPoetry.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace MyPoetry.UserControls.Pages
{
    public sealed partial class Editor : UserControl
    {
        public CustomPage GetPage { get { return MainContent; } }

        // Tools variables
        private const string words_db_path = @"Assets\data\words.txt";
        static private ResourceLoader loader = new ResourceLoader();
        private Dictionary<String, int> rhymesOptions = new Dictionary<string, int>
        {
            { loader.GetString("EditorRhymesTwo"), 2 },
            { loader.GetString("EditorRhymesThree"), 3 },
            { loader.GetString("EditorRhymesFour"), 4 }
        };
        private List<String> foundedRhymes = new List<string>();

        public Editor()
        {
            this.InitializeComponent();

            LoadRhymesOptions();
        }

        private void LoadRhymesOptions()
        {
            CmbRhymesLength.Items.Clear();
            foreach (KeyValuePair<String, int> option in rhymesOptions)
                CmbRhymesLength.Items.Add(option.Key);
            CmbRhymesLength.SelectedIndex = 0;
        }

        private void GrdParent_Loaded(object sender, RoutedEventArgs e)
        {
            // Cleans data
            Clear();

            // Checks for data to be preloaded
            var poetryData = UserHandler.Instance.GetPoetryToEdit();
            if (poetryData != null)
            {
                TxbTitle.Text = poetryData.Title;
                RebText.Document.SetText(TextSetOptions.FormatRtf, poetryData.Body);
            }
        }

        #region CharacterFormat
        private void BtnBold_Click(object sender, RoutedEventArgs e)
        {
            ITextSelection selectedText = RebText.Document.Selection;
            if (selectedText != null)
            {
                ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                charFormatting.Bold = FormatEffect.Toggle;
                selectedText.CharacterFormat = charFormatting;
            }
            RebText.Focus(FocusState.Programmatic);
        }

        private void BtnItalic_Click(object sender, RoutedEventArgs e)
        {
            ITextSelection selectedText = RebText.Document.Selection;
            if (selectedText != null)
            {
                ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                charFormatting.Italic = FormatEffect.Toggle;
                selectedText.CharacterFormat = charFormatting;
            }
            RebText.Focus(FocusState.Programmatic);
        }

        private void BtnUnderline_Click(object sender, RoutedEventArgs e)
        {
            ITextSelection selectedText = RebText.Document.Selection;
            if (selectedText != null)
            {
                ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                if (charFormatting.Underline != UnderlineType.Single)
                    charFormatting.Underline = UnderlineType.Single;
                else
                    charFormatting.Underline = UnderlineType.None;
                selectedText.CharacterFormat = charFormatting;
            }
            RebText.Focus(FocusState.Programmatic);
        }

        private void RebText_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ITextSelection selectedText = RebText.Document.Selection;
            ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
            BtnBold.IsChecked = charFormatting.Bold.Equals(FormatEffect.On);
            BtnItalic.IsChecked = charFormatting.Italic.Equals(FormatEffect.On);
            BtnUnderline.IsChecked = charFormatting.Underline.Equals(UnderlineType.Single);
        }
        #endregion

        #region Undo & Redo
        private void BtnUndo_Click(object sender, RoutedEventArgs e)
        {
            if (RebText.Document.CanUndo())
                RebText.Document.Undo();
        }

        private void BtnRedo_Click(object sender, RoutedEventArgs e)
        {
            if (RebText.Document.CanRedo())
                RebText.Document.Redo();
        }
        #endregion

        #region Tools
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SpvContent.IsPaneOpen = !SpvContent.IsPaneOpen;

            BtnMenu.Content = SpvContent.IsPaneOpen ?
                new SymbolIcon() { Symbol = Symbol.ClosePane }:
                new SymbolIcon() { Symbol = Symbol.OpenPane };
        }

        private async void SearchRhymes(string query, int rhymeLength)
        {
            ProgressBarVisible(true);

            try
            {
                var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                var file = await folder.GetFileAsync(words_db_path);
                var lines = await FileIO.ReadLinesAsync(file);

                foundedRhymes.Clear();
                foreach (String line in lines)
                {
                    if (line.Length >= rhymeLength)
                    {
                        if (line.Substring(line.Length - rhymeLength, rhymeLength).Equals(
                            query.Substring(query.Length - rhymeLength, rhymeLength)))
                        {
                            foundedRhymes.Add(line);
                        }
                    }
                }
                LsvResults.ItemsSource = null;
                LsvResults.ItemsSource = foundedRhymes;
            }
            catch { }
            
            ProgressBarVisible(false);
        }

        private async void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            var loader = new ResourceLoader();
            int rhymeLength = rhymesOptions[CmbRhymesLength.SelectedItem.ToString()];
            if (!TxbQuery.Text.Equals(String.Empty))
            {
                if (TxbQuery.Text.Length >= rhymeLength)
                {
                    SearchRhymes(TxbQuery.Text, rhymeLength);
                }
                else
                {
                    var msg = new MessageDialog(loader.GetString("RhymesToolInvalidQuery"));
                    await msg.ShowAsync();
                }
            }
            else
            {
                var msg = new MessageDialog(loader.GetString("RhymesToolEmptyQuery"));
                await msg.ShowAsync();
            }
        }

        private void ProgressBarVisible(bool visible)
        {
            ProgressRingRhymes.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            ProgressRingRhymes.IsActive = visible;
        }
        #endregion

        #region Focus
        private void TxbTitle_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                this.Focus(FocusState.Programmatic);
            }
        }
        #endregion

        #region TextChanged
        private void RebText_TextChanged(object sender, RoutedEventArgs e)
        {
            // Update statistics
            ITextRange text = RebText.Document.GetRange(0, TextConstants.MaxUnitCount);
            string s = text.Text;

            int n_words = 0, n_lines = 0, n_chars = 0;
            string[] tmp = s.Replace("\r", " ").Split(' ');
            foreach (string st in tmp)
                if (!st.Equals(String.Empty))
                    n_words++;

            if (s != "\r")
            {
                tmp = s.Replace("\r", "§").Split('§');
                foreach (string st in tmp)
                {
                    if (st != "")
                        n_lines++;
                    
                }
            }
            else
            {
                n_lines = 0;
            }

            n_chars = s.Replace("\r", "").Length;

            TxbCharsNumber.Text = n_chars.ToString();
            TxbWordsNumber.Text = n_words.ToString();
            TxbLinesNumber.Text = n_lines.ToString();

            CheckEditing();
        }

        private void TxbTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckEditing();
        }

        private void CheckEditing()
        {
            if (TxbTitle.Text.Length > 0 || RebText.Document.GetRange(0, TextConstants.MaxUnitCount).Text.Replace("\r", "").Length > 0)
                UserHandler.Instance.SetPoetryInEditing(true);
            else
                UserHandler.Instance.SetPoetryInEditing(false);
        }
        #endregion

        #region Cleaning
        private void Clear()
        {
            TxbTitle.Text = String.Empty;
            RebText.Document.SetText(TextSetOptions.FormatRtf, String.Empty);
        }
        #endregion

        #region Saving
        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (Connection.HasInternetAccess)
            {
                var loader = new ResourceLoader();

                // Create the message dialog
                MessageDialog messageDialog;

                // Checks input
                if (TxbTitle.Text != string.Empty &&
                    RebText.Document.GetRange(0, TextConstants.MaxUnitCount).Text != "\n")
                {
                    // Checks if a poetry with the same title already exists for the current user
                    List<Poetry> poetries = await App.MobileService.GetTable<Poetry>()
                        .Where(p => p.UserId == UserHandler.Instance.GetUser().Id && p.Title.Trim() == TxbTitle.Text.Trim())
                        .ToListAsync();

                    Poetry poetry = null;

                    // Sets content and title for the message dialog
                    if (poetries.Count == 0)
                        messageDialog = new MessageDialog(loader.GetString("NewPoetryConfirm"), loader.GetString("Save") + " \"" + TxbTitle.Text + "\"");
                    else
                        messageDialog = new MessageDialog(loader.GetString("UpdatePoetryConfirm"), loader.GetString("Update") + " \"" + TxbTitle.Text + "\"");

                    // YES command
                    messageDialog.Commands.Add(new UICommand(loader.GetString("Yes"), async (command) =>
                    {
                        if (poetries.Count == 0)
                        {
                            // Creates a new poetry
                            poetry = new Poetry();
                            // Sets id
                            poetry.Id = Guid.NewGuid().ToString();
                            // Sets user id
                            poetry.UserId = UserHandler.Instance.GetUser().Id;
                            // Set creation date
                            poetry.CreationDate = DateTime.Now;
                            // Sets poetry rating
                            poetry.Rating = 0;
                            // Sets bookmark
                            poetry.BookmarkYN = false;
                        }
                        else
                        {
                            // Update existing poetry
                            poetry = poetries.First();
                        }

                        // Set revision date
                        poetry.RevisionDate = DateTime.Now;

                        // Sets title
                        poetry.Title = TxbTitle.Text;

                        // Sets text
                        ITextRange text = RebText.Document.GetRange(0, TextConstants.MaxUnitCount);
                        string rtfText = text.Text;
                        text.GetText(TextGetOptions.FormatRtf, out rtfText);
                        poetry.Body = rtfText;

                        // Sets characters number
                        poetry.CharactersNumber = text.Text.Replace("\r", "").Length;

                        // Sets words number
                        int wordsNumber = 0;
                        string[] tmp = text.Text.Replace("\r", " ").Split(' ');
                        foreach (string s in tmp)
                            if (s != string.Empty)
                                wordsNumber++;
                        poetry.WordsNumber = wordsNumber;

                        // Sets verses number
                        poetry.VersesNumber = text.Text.Split('\r', '\n').Length - 1;

                        // Shows loading message
                        HalfPageMessage hpm = new HalfPageMessage(GrdParent);
                        hpm.ShowMessage(loader.GetString("SavingPoetry"), loader.GetString("SavingPoetryMessage"), true, false, false, null, null);

                        // Updates cloud db
                        if (poetries.Count == 0)
                            await App.MobileService.GetTable<Poetry>().InsertAsync(poetry);
                        else
                            await App.MobileService.GetTable<Poetry>().UpdateAsync(poetry);

                        // Updates local poetries
                        List<Poetry> new_poetries = await App.MobileService.GetTable<Poetry>()
                            .Where(p => p.UserId == UserHandler.Instance.GetUser().Id)
                            .ToListAsync();
                        UserHandler.Instance.SetPoetries(new_poetries);

                        // Shows confirm message
                        hpm.IsProgressRingEnabled = false;
                        hpm.Title = loader.GetString("Confirm");
                        if (poetries.Count == 0)
                            hpm.Message = loader.GetString("PoetryAdded");
                        else
                            hpm.Message = loader.GetString("PoetryUpdated");
                        hpm.SetOkAction(() => {
                            // Reset inputs
                            Clear();
                            // Navigates to home
                            MenuHandler.Instance.SetMenuIndex(1);
                        }, loader.GetString("Ok"));
                        hpm.IsOkButtonEnabled = true;
                    }));

                    // NO command
                    messageDialog.Commands.Add(new UICommand(loader.GetString("No"), (command) =>
                    {
                    }));

                    // Set the command that will be invoked by default
                    messageDialog.DefaultCommandIndex = 1;

                    // Show the message dialog
                    await messageDialog.ShowAsync();
                }
                else
                {
                    messageDialog = new MessageDialog(loader.GetString("Err_MissingData"), loader.GetString("Warning"));
                }
            }
            else
            {
                ((Frame)Window.Current.Content).Navigate(typeof(NoConnectionPage));
            }
        }
        #endregion
    }
}
