using System.Reflection;

namespace CodeModel.Extensions.AspNetMvc
{
    internal static class ReflectionHelper
    {
        public static Assembly Load(this AssemblyName name)
        {
            return Assembly.Load(name);
        }
    }
}