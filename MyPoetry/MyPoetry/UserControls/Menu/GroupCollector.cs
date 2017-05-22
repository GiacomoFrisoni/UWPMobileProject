using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
