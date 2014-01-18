using System.IO;
using System.Linq;
using Microsoft.VisualStudio.GraphModel;
using Graph = CodeModel.Graphs.Graph;

namespace CodeModel.Extensions.DgmlExport
{
    public class DgmlExporter
    {
        public void Export(Graph model, Stream output)
        {
            var graph = new Microsoft.VisualStudio.GraphModel.Graph();

            var schema = new GraphSchema("CodeModelSchema");            

            graph.AddSchema(schema);

            foreach (var modelNode in model.Nodes)
            {
                var graphNode = graph.Nodes.GetOrCreate(modelNode.Id);

                graphNode.Label = modelNode.DisplayLabel;

                graphNode.AddCategory(schema.GetOrCreateCategory(modelNode.GetType().FullName));
            }

            foreach (var modelLink in model.Links)
            {
                var graphLink = graph.Links.GetOrCreate(graph.Nodes.Get(modelLink.Source.Id), graph.Nodes.Get(modelLink.Target.Id));
                graphLink.AddCategory(schema.GetOrCreateCategory(modelLink.GetType().FullName));
            }

            graph.Save(output);
        }
    }
}
