using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using CodeModel.Extensions.DgmlExport;
using CodeModel.FlowAnalysis;
using Mono.Reflection;
using NUnit.Framework;

namespace Tests.FlowAnalysisTests
{
    [TestFixture]
    public class ControlFlowGraphTest
    {        
        private static Instruction NewInstruction(int offset)
        {
            var ctor = typeof (Instruction).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance).First();
            var instruction = (Instruction) ctor.Invoke(new object[] {offset, OpCodes.Nop});
            return instruction;
        }

        [Test]
        public void ShouldReduceNodesWithSingleTransitionsToOneBlock()
        {
            // arrange
            var graph = new ControlFlowGraph(new InstructionBlockNode(NewInstruction(0)));

            var i1 = graph.AddNode(new InstructionBlockNode(NewInstruction(1)));
            var i2 = graph.AddNode(new InstructionBlockNode(NewInstruction(2)));
            var i3 = graph.AddNode(new InstructionBlockNode(NewInstruction(3)));

            graph.AddLink(graph.EntryPoint, i1, new ControlTransition(TransitionKind.Forward));
            graph.AddLink(i1, i2, new ControlTransition(TransitionKind.Forward));
            graph.AddLink(i2, i3, new ControlTransition(TransitionKind.Forward));
            graph.AddLink(i3, graph.ExitPoint, new ControlTransition(TransitionKind.Forward));

            // act
            graph.MergePassthroughBlocks();

            // assert               
            Assert.That(graph.Nodes.ToList(), Has.Count.EqualTo(2));              
        }

        [Test]
        public void ShouldReduceNodesWithSingleTransitionsToThreeBlocks()
        {
            // arrange
            var graph = new ControlFlowGraph(new InstructionBlockNode(NewInstruction(0)));

            var i1 = graph.AddNode(new InstructionBlockNode(NewInstruction(1)));
            var i2 = graph.AddNode(new InstructionBlockNode(NewInstruction(2)));
            var i3 = graph.AddNode(new InstructionBlockNode(NewInstruction(3)));

            var i4 = graph.AddNode(new InstructionBlockNode(NewInstruction(4)));
            var i5 = graph.AddNode(new InstructionBlockNode(NewInstruction(5)));
            var i6 = graph.AddNode(new InstructionBlockNode(NewInstruction(6)));

            var i7 = graph.AddNode(new InstructionBlockNode(NewInstruction(7)));
            var i8 = graph.AddNode(new InstructionBlockNode(NewInstruction(8)));
            var i9 = graph.AddNode(new InstructionBlockNode(NewInstruction(9)));

            var i10 = graph.AddNode(new InstructionBlockNode(NewInstruction(10)));
            var i11 = graph.AddNode(new InstructionBlockNode(NewInstruction(11)));
            var i12 = graph.AddNode(new InstructionBlockNode(NewInstruction(12)));

            graph.AddLink(graph.EntryPoint, i1, new ControlTransition(TransitionKind.Forward));
            graph.AddLink(i1, i2, new ControlTransition(TransitionKind.Forward));
            graph.AddLink(i2, i3, new ControlTransition(TransitionKind.Forward));
            graph.AddLink(i3, i4, new ControlTransition(TransitionKind.Forward));
            graph.AddLink(i3, i7, new ControlTransition(TransitionKind.Forward));

            graph.AddLink(i4, i5, new ControlTransition(TransitionKind.Forward));
            graph.AddLink(i5, i6, new ControlTransition(TransitionKind.Forward));
            graph.AddLink(i6, i10, new ControlTransition(TransitionKind.Forward));

            graph.AddLink(i7, i8, new ControlTransition(TransitionKind.Forward));
            graph.AddLink(i8, i9, new ControlTransition(TransitionKind.Forward));
            graph.AddLink(i9, i10, new ControlTransition(TransitionKind.Forward));

            graph.AddLink(i10, i11, new ControlTransition(TransitionKind.Forward));
            graph.AddLink(i11, i12, new ControlTransition(TransitionKind.Forward));
            graph.AddLink(i12, graph.ExitPoint, new ControlTransition(TransitionKind.Forward));            

            // act
            graph.MergePassthroughBlocks();

            // assert           
            Assert.That(graph.Blocks.ToList(), Has.Count.EqualTo(4));
        }
    }
}
