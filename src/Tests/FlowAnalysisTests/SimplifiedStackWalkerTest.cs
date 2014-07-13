using System.Linq;
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

            var stackAtEnd = walker.Walk(cfg);

            Assert.That(stackAtEnd, Is.EqualTo(0));
        }

        private class Walker : BaseCfgWalker<int>
        {
            public int Walk(ControlFlowGraph cfg)
            {
                var results = base.WalkCore(cfg);

                return results.Single();
            }

            protected override int VisitBlock(int inputState, BlockNode block)
            {
                return inputState + block.StackDiff;
            }

            protected override int GetInitialState()
            {
                return 0;
            }
        }
    }
}
