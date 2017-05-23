using System.Collections.Generic;

namespace MyPoetry.UserControls.Menu
{
    class GroupCollector<T> : List<object>
    {   
        public object Key { get; set; }
        public new IEnumerator<object> GetEnumerator()
        {
            return base.GetEnumerator();
        }
        
    }
}
