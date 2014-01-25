using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TestTarget
{
    public class ControlFlowAnalysisTarget
    {
        public void MethodWithOneBranch()
        {
            int x = 10;
            Keep(x);
        }

        public void MethodWithOneIf()
        {
            Nop();

            if (DateTime.Now.Day != 10)
            {
                Nop();
            }

            Nop();
        }

        public void MethodWithTwoIfs()
        {
            Nop();

            if (DateTime.Now.Day != 10)
            {
                Nop();
            }

            Nop();

            if (DateTime.Now.Day != 11)
            {
                Nop();
            }

            Nop();
        }

        public void MethodWithOneIfAndElse()
        {
            Nop();

            if (DateTime.Now.Day != 10)
            {
                Nop();
            }
            else
            {
                Nop();
            }

            Nop();
        }

        public void MethodWithWhile()
        {
            Nop();

            for (int i = 0; i < 100; i++)
            {
                Nop();
            }

            Nop();
        }

        public void MethodWithThrow()
        {
            Nop();

            if (DateTime.Now.Day == 10)
            {
                throw new Exception();
            }

            Nop();
        }

        public void MethodWithFinally()
        {
            Nop();
            try
            {
                Marker1();
            }
            finally
            {
                Marker2();
            }

            Marker3();
        }

        public void MethodWithThrowAndSingleCatchClause()
        {
            Nop();
            try
            {
                throw new Exception("aaaa");
            }
            catch (ArgumentException)
            {
                Marker1();
            }           
        }


        public void MethodWithThrowAndMultipleCatchClauses()
        {
            Nop();
            try
            {
                throw new Exception("aaaa");
            }
            catch (ArgumentException)
            {
                Marker1();
            }
            catch (InvalidOperationException)
            {
                Marker2();
            }
            catch (Exception)
            {
                Marker3();
            }

            Marker4();
        }

        private static T Keep<T>(T value)
        {
            return value;
        }

        private static void Nop()
        {
        }

        public static void Marker1() { }
        public static void Marker2() { }
        public static void Marker3() { }
        public static void Marker4() { }
        public static void Marker5() { }
    }
}
