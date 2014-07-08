using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CodeModel.Extensions.DgmlExport;
using CodeModel.FlowAnalysis;
using CodeModel.Graphs;
using NUnit.Framework;
using TestTarget;

namespace Tests.FlowAnalysisTests
{
    [TestFixture]
    public class Bugs : IHaveGraph<BlockNode, ControlTransition>
    {
        public Graph<BlockNode, ControlTransition> Result { get; private set; }

        [Test]
        public void ConsoleInputEncoding()
        {
            var method = typeof(Console).GetProperty("InputEncoding").GetSetMethod();

            ControlFlowGraphFactory.BuildForMethod(method);
        }

        [Test]
        public void LeaveInTryFinallyAndRetReachableOnlyByBranch()
        {            
            var method = typeof (Environment).GetMethod("GetEnvironmentVariables", new[] {typeof (EnvironmentVariableTarget)});

            var cfg = ControlFlowGraphFactory.BuildForMethod(method);

            this.Result = cfg;

            foreach (var block in cfg.Blocks)
            {
                Assert.That(block.OutStackHeight, Is.GreaterThanOrEqualTo(0), "Block: {0} out stack height below 0", block);
            }
        }

        [Test]
        public void JumpToFirstInstruction()
        {
            var method = typeof(DecoderFallbackBuffer).GetMethod("Reset");

            ControlFlowGraphFactory.BuildForMethod(method);
        }

        [Test]
        public void JumpFromFirstInstruction()
        {
            var method = typeof(ConcurrentQueue<>).GetMethod("TryDequeue");

            ControlFlowGraphFactory.BuildForMethod(method);
        }
    }
}
