using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CodeModel.Builder;
using CodeModel.Extensions.DgmlExport;
using CodeModel.Extensions.EventSourcing.Mutators;
using CodeModel.Mutators;
using NUnit.Framework;
using TestTarget;

namespace Tests.ExtensionsTests
{
    [TestFixture]
    public class DgmlExportTest
    {
        [Test]
        [Ignore("Cannot provide usefull assertions")]
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

            var exporter = new DgmlExporter();

            using (var output = File.Create(Path.Combine(TestContext.CurrentContext.WorkDirectory, "graph.dgml")))
            {
                exporter.Export(builder.Model, output);
            }
        }
    }
}
