using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using CodeModel.FlowAnalysis;
using NUnit.Framework;
using Tests.Constraints;
using TestTarget;

namespace Tests.FlowAnalysisTests
{
    [TestFixture]
    public class ControlFlowTest : IHaveGraph
    {
        public ControlFlowGraph Result { get; private set; }

        CodeModel.Graphs.Graph IHaveGraph.Result
        {
            get { return this.Result; }
        }

        [Test]
        public void MethodWithOneBranch()
        {
            // arrage
            var flowAnalyzer = new ControlFlow();
            var method = Get.MethodOf<ControlFlowAnalysisTarget>(x => x.MethodWithOneBranch());

            // act
            this.Result = flowAnalyzer.AnalyzeMethod(method);

            // assert           
            var paths = this.Result.FindPaths().ToList();
            Assert.That(paths, Has.Count.EqualTo(1));
        }

        [Test]
        public void MethodWithOneIf()
        {
            // arrage
            var flowAnalyzer = new ControlFlow();
            var method = Get.MethodOf<ControlFlowAnalysisTarget>(x => x.MethodWithOneIf());

            // act
            this.Result = flowAnalyzer.AnalyzeMethod(method);

            // assert          
            var paths = this.Result.FindPaths().ToList();
            Assert.That(paths, Has.Count.EqualTo(2));
        }

        [Test]
        public void MethodWithTwoIfs()
        {
            // arrage
            var flowAnalyzer = new ControlFlow();
            var method = Get.MethodOf<ControlFlowAnalysisTarget>(x => x.MethodWithTwoIfs());

            // act
            this.Result = flowAnalyzer.AnalyzeMethod(method);

            // assert    
            var paths = this.Result.FindPaths().ToList();
            Assert.That(paths, Has.Count.EqualTo(4));
        }

        [Test]
        public void MethodWithOneIfAndElse()
        {
            // arrage
            var flowAnalyzer = new ControlFlow();
            var method = Get.MethodOf<ControlFlowAnalysisTarget>(x => x.MethodWithOneIfAndElse());

            // act
            this.Result = flowAnalyzer.AnalyzeMethod(method);

            // assert          
            var paths = this.Result.FindPaths().ToList();
            Assert.That(paths, Has.Count.EqualTo(2));
        }

        [Test]
        public void MethodWithWhile()
        {
            // arrage
            var flowAnalyzer = new ControlFlow();
            var method = Get.MethodOf<ControlFlowAnalysisTarget>(x => x.MethodWithWhile());

            // act
            this.Result = flowAnalyzer.AnalyzeMethod(method);

            // assert          
            var paths = this.Result.FindPaths().ToList();

            Assert.That(this.Result, Graph.Has
                .Nodes<InstructionNode>(exactly: 1, matches: x => x.OutboundLinks.Count() == 2)
                .Nodes<InstructionNode>(exactly: 1, matches: x => x.InboundLinks.Count() == 2)
                );
        }

        [Test]
        public void MethodWithThrow()
        {
            // arrage
            var flowAnalyzer = new ControlFlow();
            var method = Get.MethodOf<ControlFlowAnalysisTarget>(x => x.MethodWithThrow());

            // act
            this.Result = flowAnalyzer.AnalyzeMethod(method);

            // assert          
            var throwNode = this.Result.Nodes.OfType<InstructionNode>().Single(x => x.Instruction.OpCode == OpCodes.Throw);

            Assert.That(throwNode.OutboundLinks, Has.Exactly(1)
                .Matches<ControlTransition>(x => x.Target.Equals(this.Result.ExitPoint)));
        }

        [Test]
        public void MethodWithFinally()
        {
            // arrage
            var flowAnalyzer = new ControlFlow();
            var method = Get.MethodOf<ControlFlowAnalysisTarget>(x => x.MethodWithFinally());

            // act
            this.Result = flowAnalyzer.AnalyzeMethod(method);

            // assert
            var marker2Call = FindMethodCallInstruction(x => ControlFlowAnalysisTarget.Marker2());
           
            var marker3Call = FindMethodCallInstruction(x => ControlFlowAnalysisTarget.Marker3());
          
            var paths = this.Result.FindPaths().ToList();
            Assert.That(paths, Has.Count.EqualTo(2)
                .And.Exactly(1).Contains(marker3Call)
                .And.Exactly(2).Contains(marker2Call));
        }

        [Test]
        public void MethodWithThrowAndSingleCatchClause()
        {
            // arrage
            var flowAnalyzer = new ControlFlow();
            var method = Get.MethodOf<ControlFlowAnalysisTarget>(x => x.MethodWithThrowAndSingleCatchClause());

            // act
            this.Result = flowAnalyzer.AnalyzeMethod(method);

            // assert   
            var marker1Call = FindMethodCallInstruction(x => ControlFlowAnalysisTarget.Marker1());

            var paths = this.Result.FindPaths().ToList();
            Assert.That(paths, Has.Count.EqualTo(2)
                .And.Exactly(1).Contains(marker1Call));
        }

        [Test]
        public void MethodWithThrowAndMultipleCatchClause()
        {
            // arrage
            var flowAnalyzer = new ControlFlow();
            var method = Get.MethodOf<ControlFlowAnalysisTarget>(x => x.MethodWithThrowAndMultipleCatchClauses());

            // act
            this.Result = flowAnalyzer.AnalyzeMethod(method);

            // assert
            var paths = this.Result.FindPaths().ToList();
            Assert.That(paths, Has.Count.EqualTo(4));

            var marker1Call = FindMethodCallInstruction(x => ControlFlowAnalysisTarget.Marker1());

            Assert.That(marker1Call.InboundLinks, Has.Count.EqualTo(1), "Catch block not reached");

            var marker2Call = FindMethodCallInstruction(x => ControlFlowAnalysisTarget.Marker2());

            Assert.That(marker2Call.InboundLinks, Has.Count.EqualTo(1), "Catch block not reached");

            var marker3Call = FindMethodCallInstruction(x => ControlFlowAnalysisTarget.Marker3());

            Assert.That(marker3Call.InboundLinks, Has.Count.EqualTo(1), "Catch block not reached");

            var marker4Call = FindMethodCallInstruction(x => ControlFlowAnalysisTarget.Marker4());

            Assert.That(marker4Call.InboundLinks, Has.Count.AtLeast(1), "Block after try-catch not reached");

            Assert.That(this.Result.ExitPoint.InboundLinks, Has.Count.EqualTo(2), "Exit point can be reached from throw or catched exception");
        }

        [Test]
        public void MethodWithSwitch()
        {
            // arrage
            var flowAnalyzer = new ControlFlow();
            var method = Get.MethodOf<ControlFlowAnalysisTarget>(x => x.MethodWithSwitch());

            // act
            this.Result = flowAnalyzer.AnalyzeMethod(method);

            // assert
            var paths = this.Result.FindPaths().ToList();
            Assert.That(paths, Has.Count.EqualTo(4));
        }

        private InstructionNode FindMethodCallInstruction(Expression<Action<ControlFlowAnalysisTarget>> target)
        {
            var method = Get.MethodOf(target);

            return this.Result.Nodes.OfType<InstructionNode>().Single(x => x.Instruction.OpCode == OpCodes.Call && method == (MethodInfo)x.Instruction.Operand);
        }

        //TODO: test for nested trys
    }
}
