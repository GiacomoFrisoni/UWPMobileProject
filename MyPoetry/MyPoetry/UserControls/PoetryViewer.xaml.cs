using Microsoft.Toolkit.Uwp;
using System;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace MyPoetry.UserControls
{
    public sealed partial class PoetryViewer : UserControl
    {
        public PoetryViewer()
        {
            this.InitializeComponent();

            RatingControl.FilledImage = new Uri("ms-appx:///Assets/Rating/staron.png");
            RatingControl.EmptyImage = new Uri("ms-appx:///Assets/Rating/staroff.png");
        }


        public event EventHandler BackEvent;
        public event EventHandler ForwardEvent;


        private void BtnShare_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDetails_Click(object sender, RoutedEventArgs e)
        {
            PoetrySplitView.IsPaneOpen = !PoetrySplitView.IsPaneOpen;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {

        }
        
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            BackEvent?.Invoke(sender, null);
        }

        private void BtnForward_Click(object sender, RoutedEventArgs e)
        {
            ForwardEvent?.Invoke(sender, null);
        }

        async private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            StackPanel stp = new StackPanel();
            Grid g = new Grid();
            TextBlock txb = new TextBlock();
            Rectangle rect = new Rectangle();
            RichEditBox rtb = new RichEditBox();

            txb.Text = TxbTitle.Text;
            txb.FontSize = 22;

            rect.Height = 1;
            rect.VerticalAlignment = VerticalAlignment.Bottom;
            rect.Fill = new SolidColorBrush(Colors.LightGray);
            rect.Margin = new Thickness(0, 5, 0, 0);

            g.Children.Add(txb);
            g.Children.Add(rect);

            ITextRange text = RtbView.Document.GetRange(0, TextConstants.MaxUnitCount);
            string s = text.Text;

            rtb.Document.SetText(TextSetOptions.None, s);

            stp.Children.Add(g);
            stp.Children.Add(rtb);
            
            PrintHelper pHelp = new PrintHelper(stp);
            await pHelp.ShowPrintUIAsync("Testo della poesia", true);
        }
    }
}
