using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeModel
{
    internal static class StackExtensions
    {
        public static T[] PopMany<T>(this Stack<T> @this, int count)
        {
            var result = new T[count];

            for (int i = 0; i < count; i++)
            {
                result[count - i - 1] = @this.Pop();
            }

            return result;
        }
    }
}
