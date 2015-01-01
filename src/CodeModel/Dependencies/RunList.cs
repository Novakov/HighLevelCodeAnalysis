using System.Collections.Generic;
using System.Linq;

namespace CodeModel.Dependencies
{   
    public class RunList<TElement>
    {
        public IEnumerable<TElement> Elements { get; private set; }

        public RunList(IEnumerable<TElement> elements)
        {
            this.Elements = elements;                
        }
    }
}