using MyPoetry.Model;
using MyPoetry.Utilities;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
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
        public Editor()
        {
            this.InitializeComponent();
        }

        public CustomPage GetPage { get { return MainContent; } }

        #region CharacterFormat
        private void BtnBold_Click(object sender, RoutedEventArgs e) //Grassetto
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

        private void BtnItalic_Click(object sender, RoutedEventArgs e) //Corsivo
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

        private void BtnUnderline_Click(object sender, RoutedEventArgs e) //Sottolineato
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
        #endregion

        #region Tools
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Window.Current.Bounds.Width <= 700)
                SpvContent.IsPaneOpen = !SpvContent.IsPaneOpen;
            else
                SpvContent.IsPaneOpen = true;
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

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var loader = new ResourceLoader();

            // Checks input
            if (TxbTitle.Text != string.Empty &&
                RebText.Document.GetRange(0, TextConstants.MaxUnitCount).Text != "\n")
            {
                // Create the message dialog
                MessageDialog messageDialog;

                // Checks if a poetry with the same title already exists for the current user
                List<Poetry> poetries = await App.MobileService.GetTable<Poetry>()
                    .Where(p => p.UserId == UserHandler.Instance.GetUser().Id && p.Title.Trim() == TxbTitle.Text.Trim())
                    .ToListAsync();

                // Sets content and title for the message dialog
                if (poetries.Count == 0)
                    messageDialog = new MessageDialog(loader.GetString("NewPoetryConfirm"), loader.GetString("Save") + " \"" + TxbTitle.Text + "\"");
                else
                    messageDialog = new MessageDialog(loader.GetString("UpdatePoetryConfirm"), loader.GetString("Update") + " \"" + TxbTitle.Text + "\"");

                // YES command
                messageDialog.Commands.Add(new UICommand(loader.GetString("Yes"), async (command) =>
                {
                    // Creates a new poetry
                    Poetry poetry = new Poetry();

                    // Sets id
                    poetry.Id = Guid.NewGuid().ToString();

                    // Sets user id
                    poetry.UserId = UserHandler.Instance.GetUser().Id;

                    // Sets title
                    poetry.Title = TxbTitle.Text;

                    // Sets text
                    ITextRange text = RebText.Document.GetRange(0, TextConstants.MaxUnitCount);
                    string rtfText = text.Text;
                    text.GetText(TextGetOptions.FormatRtf, out rtfText);
                    poetry.Body = rtfText;

                    // Sets dates
                    if (poetries.Count == 0)
                        poetry.CreationDate = DateTime.Now;
                    poetry.RevisionDate = DateTime.Now;

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

                    // Sets poetry rating
                    poetry.Rating = 0;

                    // Sets bookmark
                    poetry.BookmarkYN = false;
                    
                    // Shows loading message
                    HalfPageMessage hpm = new HalfPageMessage(GrdParent);
                    hpm.ShowMessage(loader.GetString("SavingPoetry"), loader.GetString("SavingPoetryMessage"), true, false, false, null, null);

                    // Updates cloud db
                    if (poetries.Count == 0)
                        await App.MobileService.GetTable<Poetry>().InsertAsync(poetry);
                    else
                        await App.MobileService.GetTable<Poetry>().UpdateAsync(poetry);
                    
                    // Shows confirm message
                    hpm.IsProgressRingEnabled = false;
                    hpm.Title = loader.GetString("Confirm");
                    if (poetries.Count == 0)
                        hpm.Message = loader.GetString("PoetryAdded");
                    else
                        hpm.Message = loader.GetString("PoetryUpdated");
                    hpm.SetOkAction(null, loader.GetString("Ok"));
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
        }
    }
}
