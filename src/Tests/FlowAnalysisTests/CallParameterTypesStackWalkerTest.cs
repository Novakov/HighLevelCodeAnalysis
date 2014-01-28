using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CodeModel.FlowAnalysis;
using NUnit.Framework;
using TestTarget;

namespace Tests.FlowAnalysisTests
{
    [TestFixture]
    public class CallParameterTypesStackWalkerTest
    {     
        [Test]
        [TestCaseSource("GetTestCases")]
        public void CheckDeterminedTypes(string methodName, PotentialType[] expected)
        {
            // arrange            
            var method = typeof (CallParametersTarget).GetMethod(methodName);
            var flowGraph = new ControlFlow().AnalyzeMethod(method);
            var path = flowGraph.FindPaths().Single();

            var walker = new DetermineCallParameterTypes();

            // act
            walker.Walk(method, path);

            // assert
            Assert.That(walker.Calls, Has.Count.AtLeast(1));
            Assert.That(walker.Calls.Last().Item2, Is.EqualTo(expected));
        }

        public IEnumerable<TestCaseData> GetTestCases()
        {
            yield return TestCase(x => x.InlineParameter(), new[] {PotentialType.String});
            yield return TestCase(x => x.CallToStaticMethod(6), new[] {PotentialType.Integer});
            yield return TestCase(x => x.UseManyParameters(6, "a", 4f, 5m, false), 
                PotentialType.Integer,
                PotentialType.String,
                typeof(float),
                typeof(decimal),
                typeof(Boolean)
            );
            yield return TestCase(x => x.UseInlineValues(), 
            
                PotentialType.Integer,
                PotentialType.String,
                typeof(float),
                typeof(decimal),
                typeof(Boolean)
            );

            yield return TestCase(x => x.UseVariables(), PotentialType.Integer);
        }

        private static TestCaseData TestCase(Expression<Action<CallParametersTarget>> method, params PotentialType[] types)
        {
            return new TestCaseData(Get.MethodOf(method).Name, types);
        }
    }
}
