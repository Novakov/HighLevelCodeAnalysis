using System.Linq;
using System.Reflection;
using CodeModel.FlowAnalysis;
using NUnit.Framework;
using TestTarget;

namespace Tests.FlowAnalysisTests
{
    [TestFixture]
    public class SimplifiedStackWalkerTest
    {
        [Test]
        public void Test()
        {
            var method = Get.Method(() => NastyMethods.MethodWith27Ifs());

            var cfg = ControlFlowGraphFactory.BuildForMethod(method);

            var walker = new Walker();

            var stackAtEnd = walker.Walk(method, cfg);

            Assert.That(stackAtEnd, Is.EqualTo(0));
        }

        private class Walker : BaseCfgWalker<int>
        {
            public int Walk(MethodInfo method, ControlFlowGraph cfg)
            {
                var results = base.WalkCore(method, cfg);

                return results.Single();
            }

            protected override int VisitBlock(int inputState, BlockNode block)
            {
                return inputState + block.StackDiff;
            }

            protected override int GetInitialState(MethodInfo method, ControlFlowGraph graph)
            {
                return 0;
            }
        }
    }
}
