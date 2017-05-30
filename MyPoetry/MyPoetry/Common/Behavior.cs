using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;

namespace MyPoetry.Common
{
    public abstract class Behavior : DependencyObject, IBehavior
    {
        public DependencyObject AssociatedObject { get; set; }

        public virtual void Attach(DependencyObject associatedObject)
        {
            AssociatedObject = associatedObject;
        }

        public virtual void Detach()
        {
        }
    }

    public abstract class Behavior<T> : Behavior
    where T : DependencyObject
    {
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public new T AssociatedObject { get; set; }

        public override void Attach(DependencyObject associatedObject)
        {
            base.Attach(associatedObject);
            this.AssociatedObject = (T)associatedObject;
            OnAttached();
        }

        public override void Detach()
        {
            base.Detach();
            OnDetaching();
        }

        protected virtual void OnAttached()
        {
        }

        protected virtual void OnDetaching()
        {
        }
    }
}
