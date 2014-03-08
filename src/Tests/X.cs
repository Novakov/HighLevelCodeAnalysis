using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeModel.FlowAnalysis;
using CodeModel.Graphs;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class X : IHaveGraph
    {
        [Test]
        public void a()
        {
            var x = typeof (TestTarget.IL.Test).GetMethod("Kopytko");
            var cfg = new ControlFlow().AnalyzeMethod(x);

            this.Result = cfg;
        }

        public Graph Result { get; private set; }
    }
}
