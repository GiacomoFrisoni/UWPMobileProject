using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace MyPoetry.UserControls
{
    /// <summary>
    /// This class handles the graphic appearance of a user
    /// </summary>
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
