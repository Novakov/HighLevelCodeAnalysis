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
            var block0 = FindMethodCallInstructionBlock("Mark1");
            Assert.That(block0.InStackHeight, Is.EqualTo(0), "[block0] InStack mismatch");
            Assert.That(block0.OutStackHeight, Is.EqualTo(1), "[block0] OutStack mismatch");

            var block1 = FindMethodCallInstructionBlock("Mark2");
            Assert.That(block1.InStackHeight, Is.EqualTo(1), "[block1] InStack mismatch");
            Assert.That(block1.OutStackHeight, Is.EqualTo(2), "[block1] OutStack mismatch");

            var block2 = FindMethodCallInstructionBlock("Mark3");
            Assert.That(block2.InStackHeight, Is.EqualTo(1), "[block2] InStack mismatch");
            Assert.That(block2.OutStackHeight, Is.EqualTo(2), "[block2] OutStack mismatch");

            var block3 = FindMethodCallInstructionBlock("Mark4");
            Assert.That(block3.InStackHeight, Is.EqualTo(2), "[block3] InStack mismatch");
            Assert.That(block3.OutStackHeight, Is.EqualTo(0), "[block3] OutStack mismatch");
        }

        [Test]
        public void ShouldCalculateInAndOutStackHeightsInMethodWhitBlocksExecutedOutOfOrder()
        {
            // arrange

            // act

            // assert
        }

        private InstructionBlockNode FindMethodCallInstructionBlock(string name)
        {           
            return this.Result.Nodes.OfType<InstructionBlockNode>().Single(x => x.Instructions.Any(y => y.OpCode == OpCodes.Call && ((MethodInfo)y.Operand).Name == name));
        } 
    }
}
