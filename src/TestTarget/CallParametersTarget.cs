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

        public void Operators()
        {
            var decimal1 = Get<decimal>();
            var decimal2 = Get<decimal>();

            var double1 = Get<double>();
            var double2 = Get<double>();

            var float1 = Get<float>();
            var float2 = Get<float>();

            var uint64A = Get<UInt64>();
            var uint64B = Get<UInt64>();

            var int64A = Get<Int64>();
            var int64B = Get<Int64>();

            var uint32A = Get<UInt32>();
            var uint32B = Get<UInt32>();
            
            var int32A = Get<Int32>();
            var int32B = Get<Int32>();

            var uint16A = Get<UInt16>();
            var uint16B = Get<UInt16>();
            
            var int16A = Get<Int16>();
            var int16B = Get<Int16>();

            var byteA = Get<byte>();
            var byteB = Get<byte>();

            var sbyteA = Get<sbyte>();
            var sbyteB = Get<sbyte>();

            ShouldBe(decimal1 + decimal2);
            ShouldBe(decimal1 + uint64A);
            ShouldBe(decimal1 + int64A);
            ShouldBe(decimal1 + uint32A);
            ShouldBe(decimal1 + int32A);
            ShouldBe(decimal1 + uint16A);
            ShouldBe(decimal1 + int16A);
            ShouldBe(decimal1 + byteA);
            ShouldBe(decimal1 + sbyteA);
            
            ShouldBe(double1 + double2);
            ShouldBe(double1 + float1);
            ShouldBe(double1 + uint64A);
            ShouldBe(double1 + int64A);
            ShouldBe(double1 + uint32A);
            ShouldBe(double1 + int32A);
            ShouldBe(double1 + uint16A);
            ShouldBe(double1 + int16A);
            ShouldBe(double1 + byteA);
            ShouldBe(double1 + sbyteA);

            ShouldBe(float1 + float2);
            ShouldBe(float1 + uint64A);
            ShouldBe(float1 + int64A);
            ShouldBe(float1 + uint32A);
            ShouldBe(float1 + int32A);
            ShouldBe(float1 + uint16A);
            ShouldBe(float1 + int16A);
            ShouldBe(float1 + byteA);
            ShouldBe(float1 + sbyteA);

            ShouldBe(uint64A + uint64A);            
            ShouldBe(uint64A + uint32A);            
            ShouldBe(uint64A + uint16A);            
            ShouldBe(uint64A + byteA);            

            ShouldBe(int64A + int64B);
            ShouldBe(int64A + uint32A);
            ShouldBe(int64A + int32A);
            ShouldBe(int64A + uint16A);
            ShouldBe(int64A + int16A);
            ShouldBe(int64A + byteA);
            ShouldBe(int64A + sbyteA);
            
            ShouldBe(uint32A + uint32B);
            ShouldBe(uint32A + int32A);
            ShouldBe(uint32A + uint16A);
            ShouldBe(uint32A + int16A);
            ShouldBe(uint32A + byteA);
            ShouldBe(uint32A + sbyteA);

            ShouldBe(int32A + int32B);
            ShouldBe(int32A + uint16A);
            ShouldBe(int32A + int16A);
            ShouldBe(int32A + byteA);
            ShouldBe(int32A + sbyteA);

            ShouldBe(uint16A + uint16A);
            ShouldBe(uint16A + int16A);
            ShouldBe(uint16A + byteA);
            ShouldBe(uint16A + sbyteA);

            ShouldBe(int16A + int16B);
            ShouldBe(int16A + byteA);
            ShouldBe(int16A + sbyteA);

            ShouldBe(byteA + byteB);
            ShouldBe(byteA + sbyteB);

            ShouldBe(sbyteA + sbyteB);
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
    }
}
