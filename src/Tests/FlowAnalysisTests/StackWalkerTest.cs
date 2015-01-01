using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CodeModel.FlowAnalysis;
using NUnit.Framework;
using TestTarget;

namespace Tests.FlowAnalysisTests
{
    [TestFixture]
    public class StackWalkerTest
    {
        [Test]
        public void StackLengthAtTheEndOfMethodShouldBeZero()
        {
            IEnumerable<MethodInfo> methods = typeof(ControlFlowAnalysisTarget).GetMethods().Where(x => x.Name.StartsWith("Method"));

            foreach (var method in methods)
            {
                var graph = ControlFlowGraphFactory.BuildForMethod(method);

                var branches = graph.FindPaths().ToList();

                foreach (var branch in branches)
                {
                    var stackLength = branch.SelectMany(x => x.Instructions).Aggregate(0, (a, i) => a + i.PushedValuesCount(method, method.GetMethodBody()) - i.PopedValuesCount(method));

                    Assert.That(stackLength, Is.EqualTo(0));
                }
            }
        }       

        [Test]
        [TestCaseSource(typeof(AllMscorlibTypes), "AllTypes")]
        public void CheckStackLength(Type type)
        {
            IEnumerable<MethodInfo> methods = type.GetMethods()
                .Where(x => x.GetMethodBody() != null)
                .Where(x => x.DeclaringType == type);

            foreach (var method in methods)
            {
                var graph = ControlFlowGraphFactory.BuildForMethod(method);

                var walker = new ControlFlowGraphWalker<int>()
                {
                    InitialState = 0,
                    VisitingBlock = (stack, block) => stack + block.StackDiff
                };

                try
                {
                    var result = walker.WalkCore(method, graph);
                    Assert.That(result.Single(), Is.EqualTo(0), string.Format("Stack not 0 for method (static:{1}) {0}", method, method.IsStatic));
                }
                catch (Exception e)
                {
                    Assert.Fail("Type: {0} Method:{1} {2}", method, method.DeclaringType, e);
                }
            }
        }        
    }

    internal class AllMscorlibTypes
    {
        public IEnumerable<Type> AllTypes()
        {
            return typeof(string).Assembly.GetTypes()                
                .Where(x => !x.IsInterface)               
                .OrderBy(x => x.FullName);
        }
    }
}
