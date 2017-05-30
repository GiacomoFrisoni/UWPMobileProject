using MyPoetry.Common;
using System.Diagnostics;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace MyPoetry.Utilities
{
    public class ResizeContentOnKeyboardShowingBehavior : Behavior<FrameworkElement>
    {
        private Thickness originalMargin;

        public ResizeContentOnKeyboardShowingBehavior() { }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += AssociatedObjectLoaded;
            originalMargin = AssociatedObject.Margin;
        }

        private void AssociatedObjectLoaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Loaded -= AssociatedObjectLoaded;
            InputPane.GetForCurrentView().Showing += OnKeyboardShowing;
            InputPane.GetForCurrentView().Hiding += OnKeyboardHiding;
        }

        protected override void OnDetaching()
        {
            InputPane.GetForCurrentView().Showing -= OnKeyboardShowing;
            InputPane.GetForCurrentView().Hiding -= OnKeyboardHiding;
        }

        private void OnKeyboardShowing(InputPane sender, InputPaneVisibilityEventArgs args)
        {
            //double keyboardHeight = sender.OccludedRect.Height;
            AssociatedObject.Margin = new Thickness(originalMargin.Left, originalMargin.Top,
                originalMargin.Right, originalMargin.Bottom + 256);
            Debug.WriteLine(sender.OccludedRect.Height);
        }

        private void OnKeyboardHiding(InputPane sender, InputPaneVisibilityEventArgs args)
        {
            AssociatedObject.Margin = originalMargin;
        }
    }
}
