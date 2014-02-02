using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeModel
{
    public class ArrayComparer<TElement> : IEqualityComparer<TElement[]>
        where TElement : IEquatable<TElement>
    {
        public bool Equals(TElement[] x, TElement[] y)
        {
            if (x.Length != y.Length)
            {
                return false;
            }

            for (int i = 0; i < x.Length; i++)
            {
                if (!x[i].Equals(y[i]))
                {
                    return false;
                }                
            }

            return true;
        }

        public int GetHashCode(TElement[] obj)
        {
            return 42;
        }
    }
}
