using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MyPoetry.UserControls
{
    public sealed partial class HalfPageMessage : UserControl
    {
        /// <summary>
        /// Save the reference for the main grid where the dialog is open
        /// </summary>
        private Grid gridParent = null;
        private Func<bool> cancelAction, okAction;

        /// <summary>
        /// Result of the dialog | TRUE: pressed ok | FALSE: pressed cancel
        /// </summary>
        public bool Result = false;

        public HalfPageMessage(Grid parent)
        {
            this.InitializeComponent();
            this.gridParent = parent;
        }
        
        /// <summary>
        /// Get or set the title of the dialog
        /// </summary>
        public string Title
        {
            get { return txbTitle.Text; }
            set { txbTitle.Text = value; }
        }

        /// <summary>
        /// Get or set the message of the dialog
        /// </summary>
        public string Message
        {
            get { return txbMessage.Text; }
            set { txbMessage.Text = value; }
        }

        /// <summary>
        /// Get or set if the progress ring is enabled
        /// </summary>
        public bool IsProgressRingEnabled
        {
            get { return prgRing.Visibility == Visibility.Visible; }
            set
            {
                prgRing.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                prgRing.IsActive = value;
            }
        }

        /// <summary>
        /// Get or set if the cancel button is enabled
        /// </summary>
        public bool IsCancelButtonEnabled
        {
            get { return btnCancel.IsEnabled; }
            set
            {
                btnCancel.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                btnCancel.IsEnabled = value;
            }
        }

        /// <summary>
        /// Get or set if the ok button is enabled
        /// </summary>
        public bool IsOkButtonEnabled
        {
            get { return btnOk.IsEnabled; }
            set
            {
                btnOk.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                btnOk.IsEnabled = value;
            }
        }

        /// <summary>
        /// Set the action for the cancel button
        /// </summary>
        /// <param name="action">Action for the button</param>
        public void SetCancelAction(Func<bool> action)
        {
            this.cancelAction = action;
        }

        /// <summary>
        /// Set the action for the ok button
        /// </summary>
        /// <param name="action">Action for the button</param>
        public void SetOkAction(Func<bool> action)
        {
            this.okAction = action;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent">Parent grid where the dialog will be opened</param>
        /// <param name="title">Title of the dialog</param>
        /// <param name="message">Message of the dialog</param>
        /// <param name="isProgressRingEnabled">TRUE = progress ring spinning and visible | FALSE = no ring</param>
        /// <param name="isCancelButtonEnabled">TRUE = cancel button visible | FALSE = invisible</param>
        /// <param name="isOkButtonEnabled">TRUE = ok button visible | FALSE = invisible</param>
        public void ShowMessage(string title, string message, bool isProgressRingEnabled, bool isCancelButtonEnabled, bool isOkButtonEnabled, Func<bool> cancelAction, Func<bool> okAction)
        {
            this.Title = title;
            this.Message = message;
            this.IsProgressRingEnabled = isProgressRingEnabled;
            this.IsCancelButtonEnabled = isCancelButtonEnabled;
            this.IsOkButtonEnabled = isCancelButtonEnabled;

            this.cancelAction = cancelAction;
            this.okAction = okAction;

            this.gridParent.Children.Add(this);
        }

        /// <summary>
        /// Remove the dialog from the grid
        /// </summary>
        public void Dismiss()
        {
            if (this.gridParent != null && this.gridParent.Children.Contains(this))
                this.gridParent.Children.Remove(this);
        }


        private void btn_Click(object sender, RoutedEventArgs e)
        {
            this.Dismiss();

            if (((Button)sender).Name == "btnOk")
            {
                this.okAction?.Invoke();
            }
            else
            {
                this.cancelAction?.Invoke();
            }
        }
    }
}
