using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Visualizer.Configuration
{
    public class RunConfiguration
    {
        public string[] ResolvePaths { get; set; }

        [JsonProperty("assemblies")]
        public string[] AssembliesToAnalyze { get; set; }

        [JsonProperty("conventions")]
        public string[] ConventionAssemblies { get; set; }

        [JsonProperty("extensions")]
        public string[] ExtensionAssemblies { get; set; }

        [JsonProperty("outputs")]
        public OutputConfiguration[] Outputs { get; set; }
    }

    public class OutputConfiguration
    {
        public string Type { get; set; }
        public string[] Resources { get; set; }

        [JsonProperty("path")]
        public string OutputPath { get; set; }
    }
}
