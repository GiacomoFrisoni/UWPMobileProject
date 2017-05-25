using Windows.UI.Xaml.Controls;

namespace MyPoetry
{
    class CustomPage2
    {
        public string Title { get; set; }
        public StackPanel TopButtons { get; set; }
        public Grid Content { get; set; }

        /// <summary>
        /// Initialize a CustomPage object, with no title, empty content and buttons
        /// </summary>
        public CustomPage2()
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
        public CustomPage2(string title, Grid content)
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
        public CustomPage2 (string title, StackPanel topButtons, Grid content)
        {
            Title = title;
            TopButtons = topButtons;
            Content = content;
        }
    }
}
