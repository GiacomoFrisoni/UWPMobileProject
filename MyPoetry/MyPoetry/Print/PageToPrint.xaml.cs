using MyPoetry.Model;
using Windows.UI.Text;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

namespace MyPoetry
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class PageToPrint : Page
    {
        public PageToPrint(Poetry poetry)
        {
            this.InitializeComponent();
            
            Title.Blocks.Clear();
            Body.Blocks.Clear();

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
        }
    }
}
