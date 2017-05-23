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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace MyPoetry.UserControls.Menu
{
    public sealed partial class MenuItemViewer : UserControl
    {
        public MenuItemViewer()
        {
            this.InitializeComponent();
        }

        public string ItemText
        {
            get
            {
                return Text.Text;
            }
            set
            {
                Text.Text = value;
            }
        }

        public Symbol ItemIcon
        {
            get
            {
                return Icon.Symbol;
            }
            set
            {
                Icon.Symbol = value;
            }
        }

        public Style ItemStyle
        {
            get
            {
                return Container.Style;
            }
            set
            {
                Container.Style = value;
            }
        }
    }
}
