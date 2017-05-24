using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace MyPoetry
{
    class CustomPage
    {
        public string Title { get; set; }
        public StackPanel TopButtons { get; set; }
        public Grid Content { get; set; }

        /// <summary>
        /// Initialize a CustomPage object, with no title, empty content and buttons
        /// </summary>
        public CustomPage()
        {
            Title = "";
            TopButtons = new StackPanel();
            Content = new Grid();
        }

        /// <summary>
        /// Initialize a CustomPage object, with no buttons
        /// </summary>
        /// <param name="title">Title to show on the top of the page</param>
        /// <param name="content">The content of the page</param>
        public CustomPage(string title, Grid content)
        {
            Title = title;
            TopButtons = new StackPanel(); ;
            Content = content;
        }

        /// <summary>
        /// Initialize a CustomPage object
        /// </summary>
        /// <param name="title">Title to show on the top of the page</param>
        /// <param name="topButtons">Eventual buttons to show at the top of the page</param>
        /// <param name="content">The content of the page</param>
        public CustomPage (string title, StackPanel topButtons, Grid content)
        {
            Title = title;
            TopButtons = topButtons;
            Content = content;
        }


    }
}
