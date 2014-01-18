using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
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

        public static bool IsCompilerGenerated(this ICustomAttributeProvider @this)
        {
            return @this.GetCustomAttributes(typeof (CompilerGeneratedAttribute), false).Any();
        }

        public static BindingFlags GetBindingFlags(this MethodBase @this)
        {
            return (@this.IsPublic ? BindingFlags.Public : BindingFlags.NonPublic)
                   | (@this.IsStatic ? BindingFlags.Static : BindingFlags.Instance);
        }

        public static bool IsInherited(this MemberInfo @this)
        {
            return @this.DeclaringType != @this.ReflectedType;
        }
    }
}
