using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace MyPoetry.UserControls.Menu
{
    class MenuItem
    {
        public enum Groups
        {
            Home,
            Create,
            Explore
        }

        public string ItemText { get; set; }

        public Symbol ItemIcon { get; set; }

        public Groups Group { get; set; }
    }
}
