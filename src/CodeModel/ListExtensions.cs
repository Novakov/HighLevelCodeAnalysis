using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeModel
{
    static class ListExtensions
    {
        public static IEnumerable<T> Range<T>(this IList<T> list, int fromInclusive, int toInclusive)
        {
            for (int i = fromInclusive; i <= toInclusive; i++)
            {
                yield return list[i];
            }
        }

        public static void RemoveRange<T>(this IList<T> list, int fromInclusive, int toInclusive)
        {
            for (int i = toInclusive; i >= fromInclusive; i--)
            {
                list.RemoveAt(i);
            }
        }
    }
}
