using System;

namespace MyPoetry.Utilities
{
    /// <summary>
    /// This enum represents the two options for the custom setting
    /// inside printing menu, dedicated to the structure of the page.
    /// </summary>
    [Flags]
    internal enum DisplayContent : int
    {
        /// <summary>
        /// Show only body
        /// </summary>
        Body = 1,

        /// <summary>
        /// Show title and body
        /// </summary>
        TitleAndBody = 2
    }
}
