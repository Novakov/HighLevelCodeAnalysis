using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CodeModel
{
    public static class ReflectionExtensions
    {
        public static IEnumerable<Type> GetGenericImplementationsOfInterface(this Type @this, Type openGenericInterface)
        {
            return @this.GetInterfaces().Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == openGenericInterface);
        }

        public static bool IsCompilerGenerated(this ICustomAttributeProvider @this)
        {
            return @this.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Any();
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

        public static MethodInfo GetImplementedMethod(this MethodInfo @this, Type interfaceType)
        {
            var map = @this.DeclaringType.GetInterfaceMap(interfaceType);
            for (int i = 0; i < map.TargetMethods.Length; i++)
            {
                if (map.TargetMethods[i] == @this)
                {
                    return map.InterfaceMethods[i];
                }
            }

            return null;
        }

        public static bool HasBody(this MethodBase @this)
        {
            return @this.GetMethodBody() != null;
        }

        public static string DisplayLabel(this MethodInfo method)
        {
            var sb = new StringBuilder();

            sb
                .Append(method.ReturnType.FullName)
                .Append(" ")
                .Append(method.DeclaringType.FullName)
                .Append(".")
                .Append(method.Name);

            if (method.IsGenericMethod)
            {
                sb.Append("<");
                sb.AppendJoin(", ", method.GetGenericArguments().Select(x => x.Name));
                sb.Append(">");
            }

            sb.Append("(")
              .AppendJoin(", ", method.GetParameters().Select(x => (x.ParameterType.FullName ?? x.ParameterType.Name) + " " + x.Name))
              .Append(")");

            return sb.ToString();
        }
    }
}
