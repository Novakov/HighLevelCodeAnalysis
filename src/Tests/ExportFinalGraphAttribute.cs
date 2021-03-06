﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeModel.Builder;
using CodeModel.Extensions.DgmlExport;
using CodeModel.Extensions.DomainModel;
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

            if (haveGraph != null)
            {
                var result = ((dynamic)haveGraph).Result;
                if (result != null)
                {
                    ((dynamic) this).ExportGraph(testDetails, result);
                }
            }
        }

        private void ExportGraph<TNode, TLink>(TestDetails testDetails, Graph<TNode, TLink> graph) 
            where TNode : Node 
            where TLink : Link
        {
            var exporter = new DgmlExporter
            {
                CategoryStyles =
                {
                    new CategoryStyle {Target = typeof(EntityNode), Background = "#E4465B"},
                    new CategoryStyle {Target = typeof(AggregateNode), Background = "#39E13F"}
                }
            };

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
    }

    internal interface IHaveGraph<TNode, TLink> : IHaveGraph
        where TNode : Node
        where TLink : Link
    {
        Graph<TNode, TLink> Result { get; }
    }

    internal interface IHaveBuilder
    {
        CodeModelBuilder Builder { get; }
    }
}
