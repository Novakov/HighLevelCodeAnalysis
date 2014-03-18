using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTarget
{
    public class NastyMethods
    {
        public static void MethodWith27Ifs()
        {
            string s = "aaa";
            int i = 8;
            
            if (Condition()) Call(s);
            if (Condition()) Call(s);
            if (Condition()) Call(s);
            if (Condition()) Call(s);
            if (Condition()) Call(s);
            if (Condition()) Call(s);
            if (Condition()) Call(s);
            if (Condition()) Call(s);
            if (Condition()) Call(s);
            if (Condition()) Call(s);
            if (Condition()) Call(s);
            if (Condition()) Call(s);
            if (Condition()) Call(s);
            if (Condition()) Call(s);
            if (Condition()) Call(s);
            if (Condition()) Call(s);
            if (Condition()) Call(s);
            
            if (Condition()) 
            {
                Call(s);
                i = 10;
            }
            
            if (Condition()) Call(s);
            if (Condition()) Call(s);
            if (Condition()) Call(s);
            if (Condition()) Call(s);
            if (Condition()) Call(s);
            if (Condition()) Call(s);
            if (Condition()) Call(s);
            if (Condition()) Call(s);
            if (Condition()) Call(s);            
        }

        private static bool Condition()
        {
            return true;
        }

        private static void Call(string a)
        {
            
        }
    }
}
