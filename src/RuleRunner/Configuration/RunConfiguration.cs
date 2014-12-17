using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace RuleRunner.Configuration
{
    public class RunConfiguration
    {
        public string[] ResolvePaths { get; set; }
        
        [JsonProperty("assemblies")]
        public string[] AssembliesToAnalyze { get; set; }

        [JsonProperty("conventions")]
        public string[] ConventionAssemblies { get; set; }
    }
}