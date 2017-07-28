using MyPoetry.UserControls.Menu;
using MyPoetry.UserControls.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace MyPoetry.Utilities
{
    /// <summary>
    /// This class uses Singleton pattern to handle hamburger menu inside main page.
    /// </summary>
    public sealed class MenuHandler
    {
        private static volatile MenuHandler instance;
        private static object syncRoot = new Object();
        private ListView menu;
        private CollectionViewSource source;

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

        public ListView GetMenu()
        {
            return this.menu;
        }

        public void SetMenu(ListView menu)
        {
            this.menu = menu;
        }

        public void SetCollectionViewSource(CollectionViewSource source)
        {
            this.source = source;
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

        public void CreateMenu(MenuItem user)
        {
            // Temp list for binding
            List<MenuItem> menu = new List<MenuItem>();

            var loader = new ResourceLoader();
            menu.Add(user);
            menu.Add(new MenuItem() { ItemText = loader.GetString("Home"), ItemIcon = Symbol.Home, Group = MenuItem.Groups.Home, ItemPage = new Homepage().GetPage });
            menu.Add(new MenuItem() { ItemText = loader.GetString("NewPoetry"), ItemIcon = Symbol.Add, Group = MenuItem.Groups.Create, ItemPage = new Editor().GetPage });
            menu.Add(new MenuItem() { ItemText = loader.GetString("Settings"), ItemIcon = Symbol.Setting, Group = MenuItem.Groups.Settings, ItemPage = new Settings().GetPage });
            menu.Add(new MenuItem() { ItemText = loader.GetString("Credits"), ItemIcon = Symbol.Emoji2, Group = MenuItem.Groups.Settings, ItemPage = new Credits().GetPage });

            var groups = from c in menu group c by c.Group;
            this.source.Source = null;
            this.source.Source = groups;
        }
    }
}

