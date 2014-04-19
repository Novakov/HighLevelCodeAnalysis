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

        public void MethodWithNestedTryAndNoThrow()
        {
            Nop();
            try
            {
                Marker1();
                try
                {
                    Marker2();
                }
                catch (Exception)
                {
                    Marker3();
                }
                Marker4();
            }
            catch (Exception)
            {
                Marker5();
            }
        }

        public void MethodWithNestedTryAndThrow()
        {
            Nop();
            try
            {
                Marker1();
                try
                {
                    Marker2();
                    throw new Exception();
                }
                catch (ArgumentException e)
                {
                    Marker3();
                }
                catch (NullReferenceException e)
                {
                    Marker4();
                }

                Marker5();
            }
            catch (ArrayTypeMismatchException e)
            {
                Marker6();
            }
            catch (Exception)
            {
                Marker7();
            }
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
        public static void Marker6() { }
        public static void Marker7() { }

        public void MethodWithSwitch()
        {
            switch (DateTime.Now.Day)
            {
                case 0:
                    Marker1();
                    break;
                case 1:
                    Marker2();
                    break;
                case 3:
                    Marker3();
                    break;
            }

            Marker4();
        }
    }
}
