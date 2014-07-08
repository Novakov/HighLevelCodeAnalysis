using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using CodeModel.Extensions.DgmlExport;
using CodeModel.FlowAnalysis;
using NUnit.Framework;
using Tests.Constraints;
using TestTarget;

namespace Tests.FlowAnalysisTests
{
    [TestFixture]
    public class ControlFlowTest : IHaveGraph<BlockNode, ControlTransition>
    {
        public ControlFlowGraph Result { get; private set; }

        CodeModel.Graphs.Graph<BlockNode, ControlTransition> IHaveGraph<BlockNode, ControlTransition>.Result
        {
            get { return this.Result; }
        }

        [Test]
        public void MethodWithOneBranch()
        {
            // arrage
            var method = Get.MethodOf<ControlFlowAnalysisTarget>(x => x.MethodWithOneBranch());

            // act
            this.Result = ControlFlowGraphFactory.BuildForMethod(method);

            // assert           
            var paths = this.Result.FindPaths().ToList();
            Assert.That(paths, Has.Count.EqualTo(1));
        }

        [Test]
        public void MethodWithOneIf()
        {
            // arrage
            var method = Get.MethodOf<ControlFlowAnalysisTarget>(x => x.MethodWithOneIf());

            // act
            this.Result = ControlFlowGraphFactory.BuildForMethod(method);

            // assert                                  
            var paths = this.Result.FindPaths().ToList();
            Assert.That(paths, Has.Count.EqualTo(2));
        }

        [Test]
        public void MethodWithTwoIfs()
        {
            // arrage
            var method = Get.MethodOf<ControlFlowAnalysisTarget>(x => x.MethodWithTwoIfs());

            // act
            this.Result = ControlFlowGraphFactory.BuildForMethod(method);

            // assert    
            var paths = this.Result.FindPaths().ToList();
            Assert.That(paths, Has.Count.EqualTo(4));
        }

        [Test]
        public void MethodWithOneIfAndElse()
        {
            // arrage            
            var method = Get.MethodOf<ControlFlowAnalysisTarget>(x => x.MethodWithOneIfAndElse());

            // act
            this.Result = ControlFlowGraphFactory.BuildForMethod(method);

            // assert          
            var paths = this.Result.FindPaths().ToList();
            Assert.That(paths, Has.Count.EqualTo(2));
        }

        [Test]
        public void MethodWithWhile()
        {
            // arrage
            var method = Get.MethodOf<ControlFlowAnalysisTarget>(x => x.MethodWithWhile());

            // act
            this.Result = ControlFlowGraphFactory.BuildForMethod(method);

            // assert          
            var paths = this.Result.FindPaths().ToList();

            Assert.That(this.Result, Graph.Has
                .Nodes<InstructionBlockNode>(exactly: 1, matches: x => x.OutboundLinks.Count() == 2)
                .Nodes<InstructionBlockNode>(exactly: 1, matches: x => x.InboundLinks.Count() == 2)
                );
        }

        [Test]
        public void MethodWithThrow()
        {
            // arrage
            var method = Get.MethodOf<ControlFlowAnalysisTarget>(x => x.MethodWithThrow());

            // act
            this.Result = ControlFlowGraphFactory.BuildForMethod(method);

            // assert          
            var throwNode = this.Result.Nodes.OfType<InstructionBlockNode>().Single(x => x.Instructions.Any(y => y.OpCode == OpCodes.Throw));

            Assert.That(throwNode.OutboundLinks, Has.Exactly(1)
                .Matches<ControlTransition>(x => x.Target.Equals(this.Result.ExitPoint)));
        }

        [Test]
        public void MethodWithFinally()
        {
            // arrage
            var method = Get.MethodOf<ControlFlowAnalysisTarget>(x => x.MethodWithFinally());

            // act
            this.Result = ControlFlowGraphFactory.BuildForMethod(method);

            // assert
            var marker2Call = FindMethodCallInstructionBlock(x => ControlFlowAnalysisTarget.Marker2());

            var marker3Call = FindMethodCallInstructionBlock(x => ControlFlowAnalysisTarget.Marker3());

            var paths = this.Result.FindPaths().ToList();
            Assert.That(paths, Has.Count.EqualTo(2)
                .And.Exactly(1).Contains(marker3Call)
                .And.Exactly(2).Contains(marker2Call));
        }

        [Test]
        public void MethodWithThrowAndSingleCatchClause()
        {
            // arrage
            var method = Get.MethodOf<ControlFlowAnalysisTarget>(x => x.MethodWithThrowAndSingleCatchClause());

            // act
            this.Result = ControlFlowGraphFactory.BuildForMethod(method);

            // assert   
            var marker1Call = FindMethodCallInstructionBlock(x => ControlFlowAnalysisTarget.Marker1());

            var paths = this.Result.FindPaths().ToList();
            Assert.That(paths, Has.Count.EqualTo(2)
                .And.Exactly(1).Contains(marker1Call));
        }

        [Test]
        public void MethodWithThrowAndMultipleCatchClause()
        {
            // arrage
            var method = Get.MethodOf<ControlFlowAnalysisTarget>(x => x.MethodWithThrowAndMultipleCatchClauses());

            // act
            this.Result = ControlFlowGraphFactory.BuildForMethod(method);

            // assert
            var paths = this.Result.FindPaths().ToList();
            Assert.That(paths, Has.Count.EqualTo(4));

            var marker1Call = FindMethodCallInstructionBlock(x => ControlFlowAnalysisTarget.Marker1());

            Assert.That(marker1Call.InboundLinks, Has.Count.EqualTo(1), "Catch block not reached");

            var marker2Call = FindMethodCallInstructionBlock(x => ControlFlowAnalysisTarget.Marker2());

            Assert.That(marker2Call.InboundLinks, Has.Count.EqualTo(1), "Catch block not reached");

            var marker3Call = FindMethodCallInstructionBlock(x => ControlFlowAnalysisTarget.Marker3());

            Assert.That(marker3Call.InboundLinks, Has.Count.EqualTo(1), "Catch block not reached");

            var marker4Call = FindMethodCallInstructionBlock(x => ControlFlowAnalysisTarget.Marker4());

            Assert.That(marker4Call.InboundLinks, Has.Count.AtLeast(1), "Block after try-catch not reached");

            Assert.That(this.Result.ExitPoint.InboundLinks, Has.Count.EqualTo(2), "Exit point can be reached from throw or catched exception");
        }

        [Test]
        public void MethodWithSwitch()
        {
            // arrage
            var method = Get.MethodOf<ControlFlowAnalysisTarget>(x => x.MethodWithSwitch());

            // act
            this.Result = ControlFlowGraphFactory.BuildForMethod(method);

            // assert
            var paths = this.Result.FindPaths().ToList();
            Assert.That(paths, Has.Count.EqualTo(5));
        }

        [Test]
        public void MethodWithNestedTryAndNoThrow()
        {
            // arrage
            var method = Get.MethodOf<ControlFlowAnalysisTarget>(x => x.MethodWithNestedTryAndNoThrow());

            // act
            this.Result = ControlFlowGraphFactory.BuildForMethod(method);

            // assert
            var paths = this.Result.FindPaths().ToList();
            Assert.That(paths, Has.Count.EqualTo(1));
        }

        [Test]
        public void MethodWithNestedTryAndThrow()
        {
            // arrage
            var method = Get.MethodOf<ControlFlowAnalysisTarget>(x => x.MethodWithNestedTryAndThrow());

            // act
            this.Result = ControlFlowGraphFactory.BuildForMethod(method);

            // assert
            var block2 = FindMethodCallInstructionBlock(x => ControlFlowAnalysisTarget.Marker2());
            var block3 = FindMethodCallInstructionBlock(x => ControlFlowAnalysisTarget.Marker3());
            var block4 = FindMethodCallInstructionBlock(x => ControlFlowAnalysisTarget.Marker4());
            var block5 = FindMethodCallInstructionBlock(x => ControlFlowAnalysisTarget.Marker5());
            var block6 = FindMethodCallInstructionBlock(x => ControlFlowAnalysisTarget.Marker6());
            var block7 = FindMethodCallInstructionBlock(x => ControlFlowAnalysisTarget.Marker7());

            Assert.That(this.Result, Graph.Has
                .Links<ControlTransition>(from: block2, to: block3));

            Assert.That(this.Result, Graph.Has
                .Links<ControlTransition>(from: block2, to: block4));

            Assert.That(this.Result, Graph.Has
                .Links<ControlTransition>(from: block2, to: block5));

            Assert.That(this.Result, Graph.Has
                .Links<ControlTransition>(from: block2, to: block6));

            Assert.That(this.Result, Graph.Has
                .Links<ControlTransition>(from: block2, to: block7));
        }

        private InstructionBlockNode FindMethodCallInstructionBlock(Expression<Action<ControlFlowAnalysisTarget>> target)
        {
            var method = Get.MethodOf(target);

            return this.Result.Nodes.OfType<InstructionBlockNode>().Single(x => x.Instructions.Any(y => y.OpCode == OpCodes.Call && method == (MethodInfo) y.Operand));
        }        
    }
}
