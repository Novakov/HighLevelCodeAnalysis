using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeModel.Extensions.DgmlExport;
using CodeModel.FlowAnalysis;
using NUnit.Framework;

namespace Tests.FlowAnalysisTests
{
    [TestFixture]
    public class Bugs
    {
        [Test]
        public void Test()
        {
            var method = typeof(Console).GetProperty("InputEncoding").GetSetMethod();

            ControlFlowGraph g = null;
            try
            {
                g = ControlFlowGraphFactory.BuildForMethod(method);                
            }
            finally
            {
                using (var fs = File.Create(@"d:\dupa.dgml"))
                {
                    if (g != null)
                        new DgmlExporter().Export(g, fs);                    
                }
            }


        }
    }
}
