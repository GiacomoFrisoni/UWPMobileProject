using System;

namespace MyPoetry.Utilities
{
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
