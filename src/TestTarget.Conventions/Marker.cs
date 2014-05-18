using System.Reflection;

namespace TestTarget.Conventions
{
    public class Marker
    {
        public static Assembly Assembly
        {
            get { return typeof (Marker).Assembly; }
        }
    }
}
