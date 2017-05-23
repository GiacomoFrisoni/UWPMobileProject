using MyPoetry.Common;
using System;
using Windows.UI.Xaml.Controls;

namespace MyPoetry.Model
{
    public class WelcomeItem : BindableBase
    {
        public UserControl Control { get; set; }
        public String Description { get; set; }

        private bool _Selected = default(bool);
        public bool Selected { get { return _Selected; } set { base.SetProperty(ref _Selected, value);  } }
    }
}
