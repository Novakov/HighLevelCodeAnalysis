using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using CodeModel.FlowAnalysis;
using CodeModel.Graphs;
using NUnit.Framework;
using TestTarget;

namespace Tests.FlowAnalysisTests
{
    [TestFixture]
    public class CalculateStackHeightsTest : IHaveGraph<BlockNode, ControlTransition>
    {
        private ControlFlowGraph cfg;

        public Graph<BlockNode, ControlTransition> Result
        {
            get { return this.cfg; }
        }

        [Test]
        public void ShouldCalculateInAndOutStackHeights()
        {
            // arrange
            var method = Get.MethodOf<TestTarget.IL.WalkCfg>(x => TestTarget.IL.WalkCfg.MethodWithIf());            

            // act
            this.cfg = ControlFlowGraphFactory.BuildForMethod(method);

            // assert
            var block1 = FindMethodCallInstructionBlock("Mark1");
            Assert.That(block1.InStackHeight, Is.EqualTo(0), "[block1] InStack mismatch");
            Assert.That(block1.OutStackHeight, Is.EqualTo(1), "[block1] OutStack mismatch");
        }

        private InstructionBlockNode FindMethodCallInstructionBlock(string name)
        {           
            return this.Result.Nodes.OfType<InstructionBlockNode>().Single(x => x.Instructions.Any(y => y.OpCode == OpCodes.Call && ((MethodInfo)y.Operand).Name == name));
        } 
    }
}
