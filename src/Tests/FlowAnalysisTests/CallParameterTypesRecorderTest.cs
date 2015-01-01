using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using CodeModel.FlowAnalysis;
using Mono.Reflection;
using NUnit.Framework;

namespace Tests.FlowAnalysisTests
{
    [TestFixture]
    public class CallParameterTypesRecorderTest
    {
        [Test]
        [TestCaseSource("ILTargets")]
        public void Test(string methodName, PotentialType expectedType)
        {
            var method = typeof (TestTarget.IL.CallParameterTypesRecorderTarget).GetMethod(methodName);

            var instructions = method.GetInstructions();

            var recorder = new CallParameterTypesRecorder();

            recorder.Initialize(method);

            var takeWhile = instructions.TakeWhile(x => x.OpCode != OpCodes.Nop);
            var state = recorder.Visit(TypeAnalysisState.Empty,  takeWhile);

            Assert.That(state.StackTop, Is.EqualTo(expectedType));
        }

        private static IEnumerable<object[]> ILTargets()
        {
            yield return new object[] {"Ldtoken", PotentialType.Token};
            yield return new object[] {"Ldlen", PotentialType.Numeric};
            yield return new object[] {"Newarr", PotentialType.FromType(typeof(object[]))};
            yield return new object[] {"Ldftn", PotentialType.MethodHandle};
            yield return new object[] {"LdelemRef", PotentialType.FromType(typeof(object))};
            yield return new object[] {"Ldelema", PotentialType.FromType(typeof(object))};
            yield return new object[] {"ConvI", PotentialType.Numeric};
            yield return new object[] {"ConvI1", PotentialType.Numeric};
            yield return new object[] {"ConvI2", PotentialType.Numeric};            
            yield return new object[] {"ConvI4", PotentialType.Numeric};
            yield return new object[] {"ConvI8", PotentialType.Numeric};
            yield return new object[] {"Ldsfld", PotentialType.String};
            yield return new object[] {"Ldvirtftn", PotentialType.MethodHandle};
            yield return new object[] {"Neg", PotentialType.Numeric};
            yield return new object[] {"Not", PotentialType.Numeric};
            yield return new object[] {"Isinst", PotentialType.FromType(typeof(object))};
            yield return new object[] {"Ldflda", PotentialType.FromType(typeof(string))};
            yield return new object[] {"UnboxAny", PotentialType.FromType(typeof(int))};
            yield return new object[] {"Unbox", PotentialType.FromType(typeof(int))};
        }        
    }
}
