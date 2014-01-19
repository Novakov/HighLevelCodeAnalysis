using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CodeModel.Graphs;
using Microsoft.VisualStudio.GraphModel;
using Microsoft.VisualStudio.GraphModel.Styles;
using Graph = CodeModel.Graphs.Graph;

namespace CodeModel.Extensions.DgmlExport
{
    public class DgmlExporter
    {
        public ICollection<CategoryStyle> CategoryStyles { get; private set; }

        public DgmlExporter()
        {
            this.CategoryStyles = new List<CategoryStyle>();
        }

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

            foreach (var categoryStyle in this.CategoryStyles)
            {
                var style = new GraphConditionalStyle(graph);

                if (typeof (Node).IsAssignableFrom(categoryStyle.Target))
                {
                    style.TargetType = typeof(GraphNode);   
                }
                else
                {
                    style.TargetType = typeof (GraphLink);
                }

                style.ValueLabel = "True";
                style.GroupLabel = categoryStyle.Target.Name;

                style.Conditions.Add(new GraphCondition(style)
                {
                    Expression = string.Format("HasCategory('{0}')", categoryStyle.Target.FullName)
                });

                if (categoryStyle.Background != null)
                {
                    style.Setters.Add(new GraphSetter(style, "Background") {Value = categoryStyle.Background});
                }

                if (categoryStyle.Stroke != null)
                {
                    style.Setters.Add(new GraphSetter(style, "Stroke") { Value = categoryStyle.Stroke });
                }

                graph.Styles.Add(style);
            }

            graph.Save(output);
        }
    }

    public class CategoryStyle
    {
        public Type Target { get; set; }        

        public string Background { get; set; }

        public string Stroke { get; set; }
    }
}
