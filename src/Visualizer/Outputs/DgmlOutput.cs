using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeModel.Builder;
using CodeModel.Extensions.DgmlExport;

namespace Visualizer.Outputs
{
    public class DgmlOutput : IOutput
    {
        public void Write(string path, CodeModelBuilder modelBuilder)
        {
            var exporter = new DgmlExporter();

            using (var stream = File.Create(path))
            {
                exporter.Export(modelBuilder.Model, stream);
            }
        }
    }

    public interface IOutput
    {
        void Write(string path, CodeModelBuilder modelBuilder);
    }
}
