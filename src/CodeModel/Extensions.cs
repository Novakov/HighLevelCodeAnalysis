using System;
using System.Collections.Generic;
using System.Text;

namespace CodeModel
{
    internal static class Extensions
    {
        public static StringBuilder AppendJoin(this StringBuilder @this, string separator, IEnumerable<string> items)
        {
            bool firstItem = true;

            foreach (var item in items)
            {
                if (!firstItem)
                {
                    @this.Append(separator);                    
                }
                else
                {
                    firstItem = false;
                }

                @this.Append(item);
            }

            return @this;
        }

        public static void Call<TEventArgs>(this EventHandler<TEventArgs> @delegate, object @this, TEventArgs eventArgs)
        {
            if (@delegate != null)
            {
                @delegate(@this, eventArgs);
            }
        }

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