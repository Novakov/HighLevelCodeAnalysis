using System.IO;
using System.Reflection;
using CodeModel.Builder;
using CodeModel.Extensions.DgmlExport;
using CodeModel.Extensions.EventSourcing.Links;
using CodeModel.Extensions.EventSourcing.Mutators;
using CodeModel.Extensions.EventSourcing.Nodes;
using CodeModel.Model;
using CodeModel.Mutators;
using NUnit.Framework;
using TestTarget;

namespace Tests.ExtensionsTests
{
    [TestFixture]
    public class DgmlExportTest
    {
        [Test]
        [Explicit("Cannot provide usefull assertions")]
        public void ExportToDgml()
        {
            var builder = new CodeModelBuilder();

            builder.RegisterConventionsFrom(typeof(TestTarget.Conventions.Marker).Assembly);

            builder.RunMutator(new AddAssemblies(typeof(Marker).Assembly));
            builder.RunMutator<AddTypes>();
            builder.RunMutator(new AddMethods(AddMethods.DefaultFlags | BindingFlags.NonPublic));           
            builder.RunMutator<AddProperties>();
            builder.RunMutator<AddFields>();
            builder.RunMutator<DetectEntities>();
            builder.RunMutator<LinkMethodCalls>();
            builder.RunMutator<LinkFieldAccess>();
            builder.RunMutator<LinkPropertyAccess>();
            
            builder.RunMutator<DetectApplyEvent>();
            builder.RunMutator<DetectApplyEventMethods>();

            var exporter = new DgmlExporter
            {
                CategoryStyles =
                {
                    new CategoryStyle {Target = typeof (TypeNode), Background = "#999933"},
                    new CategoryStyle {Target = typeof (MethodNode), Background = "LightGreen"},
                    new CategoryStyle {Target = typeof (PropertyNode), Background = "Yellow"},
                    new CategoryStyle {Target = typeof (FieldNode), Background = "#993300"},
                    new CategoryStyle {Target = typeof (ApplyEventMethod), Background = "Red"},

                    new CategoryStyle {Target = typeof(ApplyEventLink), Stroke = "#FF11FFBB"}
                }
            };

            using (var output = File.Create(Path.Combine(TestContext.CurrentContext.WorkDirectory, "graph.dgml")))
            {
                exporter.Export(builder.Model, output);
            }
        }
    }
}
