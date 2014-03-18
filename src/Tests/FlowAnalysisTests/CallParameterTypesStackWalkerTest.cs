using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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

    public class Reducer
    {
        private readonly MethodInfo method;
        private readonly ControlFlowGraph cfg;

        public static void Reduce(MethodInfo method,ControlFlowGraph cfg)
        {
            Reducer tempQualifier = new Reducer(method, cfg);
            Reduce(new Reducer(method, cfg).cfg, tempQualifier.method);
        }

        private Reducer(MethodInfo method, ControlFlowGraph cfg)
        {
            this.method = method;
            this.cfg = cfg;
        }

        private static void Reduce(ControlFlowGraph controlFlowGraph, MethodInfo containingMethod)
        {
            foreach (var blockNode in controlFlowGraph.Blocks.OfType<InstructionBlockNode>())
            {
                blockNode.CalculateStackProperties(containingMethod);
            }

            foreach (var block in controlFlowGraph.Blocks.OfType<InstructionBlockNode>())
            {
                if (!block.IsPassthrough)
                {
                    continue;                    
                }

                if (block.GoesBelowInitialStack || block.SetsLocalVariable)
                {
                    continue;                    
                }

                var branchBlock = block.TransitedFrom.Single();
                var joinBlock = block.TransitTo.Single();

                var bypassingTransition = branchBlock.OutboundLinks.Where(x => x.Target.Equals(joinBlock));

                if (bypassingTransition.Count() == 1)
                {
                    controlFlowGraph.RemoveLink(bypassingTransition.Single());
                }
            }
        }
    }
}
