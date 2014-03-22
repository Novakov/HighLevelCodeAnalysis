using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using CodeModel.FlowAnalysis;
using CodeModel.Graphs;
using NUnit.Framework;
using TestTarget;

namespace Tests.FlowAnalysisTests
{
    [TestFixture]
    public class CallParameterTypesStackWalkerTest : IHaveGraph
    {
        public Graph Result { get; private set; }

        [Test]
        public void CheckMethodWithOpcodeInitobj()
        {
            var method = Get.MethodOf<CallParametersTarget>(x => CallParametersTarget.Get<int>()).GetGenericMethodDefinition();

            var flowGraph = new ControlFlow().AnalyzeMethod(method);            
            
            var types = new DetermineCallParameterTypes();
            types.Walk(method, flowGraph);
        }

        [Test]
        [TestCaseSource("GetTestCases")]
        public void CheckDeterminedTypes(string methodName, PotentialType[] expected)
        {
            // arrange            
            var method = typeof(CallParametersTarget).GetMethod(methodName);
            var flowGraph = new ControlFlow().AnalyzeMethod(method);            

            var walker = new DetermineCallParameterTypes();

            // act
            walker.Walk(method, flowGraph);           

            // assert
            Assert.That(walker.Calls, Has.Count.AtLeast(1));
            Assert.That(walker.Calls.Last().Value.Single(), Is.EqualTo(expected));
        }

        public IEnumerable<TestCaseData> GetTestCases()
        {
            yield return TestCase(x => x.InlineParameter(), new[] { PotentialType.String });
            yield return TestCase(x => x.CallToStaticMethod(6), new[] { PotentialType.Numeric });
            yield return TestCase(x => x.UseManyParameters(6, "a", 4f, 5m, false),
                PotentialType.Numeric,
                PotentialType.String,
                typeof(float),
                typeof(decimal),
                typeof(Boolean)
            );
            yield return TestCase(x => x.UseInlineValues(),
                PotentialType.Numeric,
                PotentialType.String,
                typeof(float),
                typeof(decimal),
                typeof(Boolean)
            );

            yield return TestCase(x => x.UseVariables(), PotentialType.Numeric);
            yield return TestCase(x => x.OverrideArgument(null), PotentialType.String);
        }

        private static TestCaseData TestCase(Expression<Action<CallParametersTarget>> method, params PotentialType[] types)
        {
            return new TestCaseData(Get.MethodOf(method).Name, types);
        }

        [Test, Category("Perf"), Explicit]
        public void AnalyzeMethodWith27Ifs()
        {
            var method = Get.MethodOf<NastyMethods>(x => NastyMethods.MethodWith27Ifs());
            var cfg = new ControlFlow().AnalyzeMethod(method);

            this.Result = cfg;                        

            new DetermineCallParameterTypes().Walk(method, cfg);
        }
    }
}
