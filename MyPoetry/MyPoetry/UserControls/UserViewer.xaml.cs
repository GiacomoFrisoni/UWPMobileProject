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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MyPoetry.UserControls
{
    public sealed partial class UserViewer : UserControl
    {
        public UserViewer()
        {
            this.InitializeComponent();
        }

        public string Title
        {
            get { return TxblTitle.Text; }
            set { TxblTitle.Text = value; }
        }

        public string Details
        {
            get { return TxblDetails.Text; }
            set { TxblDetails.Text = value; }
        }
       
        public Brush ImageSource
        {
            get { return UserImage.Fill; }
            set { UserImage.Fill = value; }
        }

        public void SetSize(int size)
        {
            UserBorder.Width = size;
            UserBorder.Height = size;

            UserImage.Width = size - 4;
            UserImage.Height = size - 4;
        }

        public void SetSmaller()
        {
            SetSize(100);
        }

        public void SetBigger()
        {
            SetSize(170);
        }
    }
}
