using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using CodeModel.FlowAnalysis;
using CodeModel.Graphs;
using Graph = CodeModel.Graphs.Graph;

namespace CodeModel.Extensions.DgmlExport
{
    public class DgmlExporter
    {
        private const string Namespace = "http://schemas.microsoft.com/vs/2009/dgml";

        public ICollection<CategoryStyle> CategoryStyles { get; private set; }

        private XmlWriter writer;

        public DgmlExporter()
        {
            this.CategoryStyles = new List<CategoryStyle>();
        }

        public void Export(Graph model, Stream output)
        {            
            using (this.writer = XmlWriter.Create(output, new XmlWriterSettings() { Indent = true }))
            {
                writer.WriteStartElement("DirectedGraph", Namespace);

                WriteNodes(model);

                WriteLinks(model);

                writer.WriteStartElement("Styles", Namespace);

                foreach (var categoryStyle in this.CategoryStyles)
                {
                    writer.WriteStartElement("Style", Namespace);

                    writer.WriteAttributeString("ValueLabel", "True");
                    writer.WriteAttributeString("GroupLabel", categoryStyle.Target.Name);                    

                    if (typeof(Node).IsAssignableFrom(categoryStyle.Target))
                    {
                        writer.WriteAttributeString("TargetType", "Node");
                    }
                    else
                    {
                        writer.WriteAttributeString("TargetType", "Link");
                    }
                  
                    writer.WriteStartElement("Condition", Namespace);
                    writer.WriteAttributeString("Expression", string.Format("HasCategory('{0}')", categoryStyle.Target.FullName));
                    writer.WriteEndElement();
                                      
                    WriteStyleSetter("Background", categoryStyle.Background);

                    WriteStyleSetter("Stroke", categoryStyle.Stroke);

                    writer.WriteEndElement();                                        
                }

                writer.WriteEndElement();

                writer.WriteEndElement();
            }
        }

        private void WriteStyleSetter(string property, string value)
        {
            if (value != null)
            {
                this.writer.WriteStartElement("Setter", Namespace);

                this.writer.WriteAttributeString("Property", property);
                this.writer.WriteAttributeString("Value", value);

                this.writer.WriteEndElement();
            }
        }

        private void WriteLinks(Graph model)
        {
            this.writer.WriteStartElement("Links", Namespace);

            foreach (var modelLink in model.Links)
            {
                this.writer.WriteStartElement("Link", Namespace);

                this.writer.WriteAttributeString("Source", modelLink.Source.Id);
                this.writer.WriteAttributeString("Target", modelLink.Target.Id);
                this.writer.WriteAttributeString("Category", modelLink.GetType().FullName);

                var ct = modelLink as ControlTransition;
                if (ct != null)
                {
                    this.writer.WriteAttributeString("TransitionKind", ct.Kind.ToString());
                }

                this.writer.WriteEndElement();
            }

            this.writer.WriteEndElement();
        }

        private void WriteNodes(Graph model)
        {
            this.writer.WriteStartElement("Nodes", Namespace);
            foreach (var modelNode in model.Nodes)
            {
                this.writer.WriteStartElement("Node");

                this.writer.WriteAttributeString("Id", modelNode.Id);
                this.writer.WriteAttributeString("Category", modelNode.GetType().FullName);
                this.writer.WriteAttributeString("Label", modelNode.DisplayLabel);

                this.writer.WriteEndElement();
            }

            this.writer.WriteEndElement();
        }
    }

    public class CategoryStyle
    {
        public Type Target { get; set; }

        public string Background { get; set; }

        public string Stroke { get; set; }
    }
}
