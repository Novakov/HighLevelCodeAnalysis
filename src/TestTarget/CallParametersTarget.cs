using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTarget
{
    public class CallParametersTarget
    {
        public void InlineParameter()
        {
            TargetWithObject("test_string");
        }

        public void CallToStaticMethod(int i)
        {
            StaticTarget(i);
        }

        public void UseManyParameters(int arg1, string arg2, float arg3, decimal arg4, bool arg5)
        {
            ManyParameters(arg1, arg2, arg3, arg4, arg5);
        }

        public void UseInlineValues()
        {
            ManyParameters(1, "aaaa", 3f, 4m, false);
        }

        public void UseVariables()
        {
            object i = Get<int>();

            TargetWithObject(i);
        }

        public void OverrideArgument(object test)
        {
            test = "aaaaa";

            TargetWithObject(test);
        }

        public void TwoBranches()
        {
            object value;

            if (Get<int>() == 2)
            {
                value = "Test";
            }
            else
            {
                value = new Dictionary<int, int>();
            }

            try
            {
                TargetWithObject(value);
            }
            finally
            {
                Keep();
            }
        }

        public static void ShouldBe<T>(T i)
        {
        }

        public void ManyParameters(object arg1, object arg2, object arg3, object arg4, object arg5)
        {
        }

        public void TargetWithObject(object value)
        { }

        public void StaticTarget(object value)
        {
        }

        public static T Get<T>()
        {
            return default(T);
        }

        public void Keep()
        {
        }
    }
}
