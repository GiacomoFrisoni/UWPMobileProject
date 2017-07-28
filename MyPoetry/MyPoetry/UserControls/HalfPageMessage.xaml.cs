using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MyPoetry.UserControls
{
    /// <summary>
    /// This class handles an half page message that can be showed inside
    /// a grid control.
    /// </summary>
    public sealed partial class HalfPageMessage : UserControl
    {
        /// <summary>
        /// Save the reference for the main grid where the dialog is open
        /// </summary>
        private Grid GridParent = null;

        /// <summary>
        /// Save the reference for the action after pressing button
        /// </summary>
        private Action CancelAction, OkAction;

        /// <summary>
        /// Result of the dialog | TRUE: pressed ok | FALSE: pressed cancel
        /// </summary>
        public bool Result = false;

        /// <summary>
        /// Initialize the logic for halfpagemessage
        /// </summary>
        /// <param name="parent">Parent grid where message will pop</param>
        public HalfPageMessage(Grid parent)
        {
            this.InitializeComponent();
            this.GridParent = parent;
        }
        
        /// <summary>
        /// Get or set the title of the dialog
        /// </summary>
        public string Title
        {
            get { return TxbTitle.Text; }
            set { TxbTitle.Text = value; }
        }

        /// <summary>
        /// Get or set the message of the dialog
        /// </summary>
        public string Message
        {
            get { return TxbMessage.Text; }
            set { TxbMessage.Text = value; }
        }

        /// <summary>
        /// Get or set if the progress ring is enabled
        /// </summary>
        public bool IsProgressRingEnabled
        {
            get { return PrgRing.Visibility == Visibility.Visible; }
            set
            {
                PrgRing.Visibility = (value) ? Visibility.Visible : Visibility.Collapsed;
                PrgRing.IsActive = value;
            }
        }

        /// <summary>
        /// Get or set if the cancel button is enabled
        /// </summary>
        public bool IsCancelButtonEnabled
        {
            get { return BtnCancel.IsEnabled; }
            set
            {
                BtnCancel.Visibility = (value) ? Visibility.Visible : Visibility.Collapsed;
                BtnCancel.IsEnabled = value;
            }
        }

        /// <summary>
        /// Get or set if the ok button is enabled
        /// </summary>
        public bool IsOkButtonEnabled
        {
            get { return BtnOk.IsEnabled; }
            set
            {
                BtnOk.Visibility = (value) ? Visibility.Visible : Visibility.Collapsed;
                BtnOk.IsEnabled = value;
            }
        }

        /// <summary>
        /// Set the action for the cancel button
        /// </summary>
        /// <param name="action">Action for the button</param>
        /// <param name="text">Text shown on the button</param>
        public void SetCancelAction(Action action, string text)
        {
            this.CancelAction = action;
            BtnCancel.Content = text;
        }

        /// <summary>
        /// Set the action for the ok button
        /// </summary>
        /// <param name="action">Action for the button</param>
        /// <param name="text">Text shown on the button</param>
        public void SetOkAction(Action action, string text)
        {
            this.OkAction = action;
            BtnOk.Content = text;
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
        public void ShowMessage(string title, string message, bool isProgressRingEnabled, bool isCancelButtonEnabled, bool isOkButtonEnabled, Action cancelAction, Action okAction)
        {
            this.Title = title;
            this.Message = message;
            this.IsProgressRingEnabled = isProgressRingEnabled;
            this.IsCancelButtonEnabled = isCancelButtonEnabled;
            this.IsOkButtonEnabled = isOkButtonEnabled;

            this.CancelAction = cancelAction;
            this.OkAction = okAction;

            this.GridParent.Children.Add(this);
        }

        /// <summary>
        /// Remove the dialog from the grid
        /// </summary>
        public void Dismiss()
        {
            if (this.GridParent != null && this.GridParent.Children.Contains(this))
                this.GridParent.Children.Remove(this);
        }


        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            this.Dismiss();

            if (((Button)sender).Name == "BtnOk")
            {
                this.OkAction?.Invoke();
            }
            else
            {
                this.CancelAction?.Invoke();
            }
        }
    }
}
