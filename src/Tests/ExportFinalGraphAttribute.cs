using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeModel.Builder;
using CodeModel.Extensions.DgmlExport;
using CodeModel.Graphs;
using NUnit.Framework;

namespace Tests
{
    class ExportFinalGraphAttribute : NUnit.Framework.TestActionAttribute
    {
        public override ActionTargets Targets
        {
            get { return ActionTargets.Test; }
        }

        public override void AfterTest(TestDetails testDetails)
        {
            var haveBuilder = testDetails.Fixture as IHaveBuilder;

            if (haveBuilder != null && haveBuilder.Builder != null)
            {
                ExportGraph(testDetails, haveBuilder.Builder.Model);
            }

            var haveGraph = testDetails.Fixture as IHaveGraph;
            if (haveGraph != null && haveGraph.Result != null)
            {
                ExportGraph(testDetails, haveGraph.Result);
            }
        }               

        private static void ExportGraph(TestDetails testDetails, Graph graph)
        {
            var exporter = new DgmlExporter();

            var targetDirectory = Path.Combine(TestContext.CurrentContext.WorkDirectory, "Graphs", testDetails.Fixture.GetType().FullName);

            if (!Directory.Exists(targetDirectory))
            {
                Directory.CreateDirectory(targetDirectory);
            }

            using (var output = File.Create(Path.Combine(targetDirectory, testDetails.Method.Name + ".dgml")))
            {
                exporter.Export(graph, output);
            }
        }
    }

    internal interface IHaveGraph
    {
        Graph Result { get; }
    }

    internal interface IHaveBuilder
    {
        CodeModelBuilder Builder { get; }
    }
}
