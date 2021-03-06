﻿using Newtonsoft.Json;
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
        
        public string[] DisabledRules { get; set; }

        public ReportConfiguration Reports { get; set; }

        public RunConfiguration()
        {
            this.ResolvePaths = new string[0];
            this.ConventionAssemblies = new string[0];
            this.ExtensionAssemblies = new string[0];
            this.DisabledRules = new string[0];
        }
    }

    public class ReportConfiguration
    {
        public HtmlReport Html { get; set; }
        public DgmlResult Dgml { get; set; }
    }

    public class DgmlResult
    {
        public string Path { get; set; }
    }
}