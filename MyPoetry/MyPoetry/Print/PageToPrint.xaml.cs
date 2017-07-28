using MyPoetry.Model;
using Windows.ApplicationModel.Resources;
using Windows.UI.Text;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

namespace MyPoetry
{
    /// <summary>
    /// First page for poetry printing.
    /// </summary>
    public sealed partial class PageToPrint : Page
    {
        /// <summary>
        /// Variable for localized string resources
        /// </summary>
        ResourceLoader loader = new ResourceLoader();

        public PageToPrint(Poetry poetry)
        {
            this.InitializeComponent();
            
            // Clears paragraphs
            Title.Blocks.Clear();
            Body.Blocks.Clear();
            FooterDescription.Blocks.Clear();

            // Sets title
            Paragraph paragraphTitle = new Paragraph();
            Run runTitle = new Run();
            runTitle.Text = poetry.Title;
            paragraphTitle.Inlines.Add(runTitle);
            Title.Blocks.Add(paragraphTitle);
            
            // Sets body
            RichEditBox reb = new RichEditBox();
            reb.Document.SetText(TextSetOptions.FormatRtf, poetry.Body);
            ITextRange poetryText = reb.Document.GetRange(0, TextConstants.MaxUnitCount);
            string[] verses = poetryText.Text.Split('\r');
            foreach (string v in verses)
            {
                Paragraph paragraph = new Paragraph();
                Run run = new Run();
                run.Text = v;
                paragraph.Inlines.Add(run);
                Body.Blocks.Add(paragraph);
            }

            // Sets localized footer
            Paragraph paragraphFooter = new Paragraph();
            Run runFooter = new Run();
            runFooter.Text = loader.GetString("PrintFooter");
            paragraphFooter.Inlines.Add(runFooter);
            FooterDescription.Blocks.Add(paragraphFooter);
        }
    }
}
