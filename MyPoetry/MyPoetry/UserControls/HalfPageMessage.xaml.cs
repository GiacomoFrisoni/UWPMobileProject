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

namespace MyPoetry.UserControls
{
    public sealed partial class HalfPageMessage : UserControl
    {
        private Grid Parent;

        public HalfPageMessage()
        {
            this.InitializeComponent();
        }


        public string Title
        {
            get { return txbTitle.Text; }
            set { txbTitle.Text = value; }
        }

        public string Message
        {
            get { return txbMessage.Text; }
            set { txbMessage.Text = value; }
        }

        public bool IsProgressRingEnabled
        {
            get { return prgRing.Visibility == Visibility.Visible; }
            set {
                if (value)
                {
                    prgRing.Visibility = Visibility.Visible;
                    prgRing.IsActive = true;
                }
                else
                {
                    prgRing.Visibility = Visibility.Collapsed;
                    prgRing.IsActive = false;
                }
                //Sta cazzo di cosa non va
                //(value) ? prgRing.Visibility = Visibility.Visible : prgRing.Visibility = Visibility.Collapsed;
            }
        }

        public StackPanel ButtonContainer
        {
            get { return stpButtons; }
            set { stpButtons = value; }
        }


        public void ShowMessage(Grid parent, string title, string message, bool isProgressRingEnabled, StackPanel buttons)
        {
            this.Title = title;
            this.Message = message;
            this.ButtonContainer = buttons;
            this.IsProgressRingEnabled = isProgressRingEnabled;

            this.Parent = parent;
            parent.Children.Add(this);
        }

        public void Dismiss()
        {
            this.Parent.Children.Remove(this);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Parent.Children.Remove(this);
        }
    }
}
