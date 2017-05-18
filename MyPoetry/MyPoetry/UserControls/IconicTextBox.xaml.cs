using System;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace MyPoetry.UserControls
{
    public sealed partial class IconicTextBox : UserControl
    {
        public IconicTextBox()
        {
            this.InitializeComponent();
        }


        public String Text
        {
            get { return text.Text; }
            set { text.Text = value; }
        }

        public Symbol Icon
        {
            get { return icon.Symbol; }
            set { icon.Symbol = value; }
        }
    }
}