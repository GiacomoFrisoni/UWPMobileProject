using MyPoetry.Common;

namespace MyPoetry.ViewModel
{
    class EditorViewModel : BindableBase
    {
        private bool _IsBoldEnabled = default(bool);
        private bool _IsItalicEnabled = default(bool);
        private bool _IsUnderlineEnabled = default(bool);

        public bool IsBoldEnabled
        {
            get { return _IsBoldEnabled; }
            set { base.SetProperty(ref _IsBoldEnabled, value); }
        }

        public bool IsItalicEnabled
        {
            get { return _IsItalicEnabled; }
            set { base.SetProperty(ref _IsItalicEnabled, value); }
        }

        public bool IsUnderlineEnabled
        {
            get { return _IsUnderlineEnabled; }
            set { base.SetProperty(ref _IsUnderlineEnabled, value); }
        }
    }
}
