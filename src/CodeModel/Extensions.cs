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
    }
}