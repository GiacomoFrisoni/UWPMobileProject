using System;
using Windows.UI.Xaml.Controls;

namespace MyPoetry.Utilities
{
    public sealed class MenuHandler
    {
        private static volatile MenuHandler instance;
        private static object syncRoot = new Object();
        private ListView menu;

        private MenuHandler() { }

        /// <summary>
        /// Gets the Singleton instance.
        /// If the instance does not exist, it creates a new one.
        /// </summary>
        public static MenuHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new MenuHandler();
                    }
                }

                return instance;
            }
        }

        public void SetMenu(ListView menu)
        {
            this.menu = menu;
        }

        public void SetMenuIndex(int index)
        {
            if (menu != null && index >= 0 && index < this.menu.Items.Count)
                this.menu.SelectedIndex = index;
        }

        public int? GetMenuIndex()
        {
            if (menu == null)
                return null;
            else
                return this.menu.SelectedIndex;
        }
    }
}

