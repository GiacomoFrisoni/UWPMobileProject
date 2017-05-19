using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MyPoetry.UserControls
{
    public sealed partial class HalfPageMessage : UserControl
    {
        private Grid GridParent = null;

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
                // Non va
                // (value) ? prgRing.Visibility = Visibility.Visible : prgRing.Visibility = Visibility.Collapsed;
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

            this.GridParent = parent;
            parent.Children.Add(this);
        }

        public void Dismiss()
        {
            if (this.GridParent != null && this.GridParent.Children.Contains(this))
                this.GridParent.Children.Remove(this);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.GridParent != null && this.GridParent.Children.Contains(this))
                this.GridParent.Children.Remove(this);
        }
    }
}
