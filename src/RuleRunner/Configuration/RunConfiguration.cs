using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RuleRunner.Reports.Html;

namespace RuleRunner.Configuration
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

        public ReportConfiguration Reports { get; set; }
    }

    public class ReportConfiguration
    {
        public HtmlReport Html { get; set; }
    }
}