using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeModel.Extensions.DomainModel
{
    internal static class Extension
    {
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key, TValue defaultValue = default(TValue))
        {
            TValue value;
            if (@this.TryGetValue(key, out value))
            {
                return value;
            }

            return defaultValue;
        }
    }
}
