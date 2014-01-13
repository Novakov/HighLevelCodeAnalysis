using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeModel
{
    static class ReflectionExtensions
    {
        public static IEnumerable<Type> GetGenericImplementationsOfInterface(this Type @this, Type openGenericInterface)
        {
            return @this.GetInterfaces().Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == openGenericInterface);
        }
    }
}
