using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace RuleRunner.Reports.Html
{
    public class HtmlReport
    {
        [JsonProperty("output")]
        public string OutputDirectory { get; private set; }

        public void Write()
        {
            if (!Directory.Exists(this.OutputDirectory))
            {
                Directory.CreateDirectory(this.OutputDirectory);
            }

            using(var indexTemplateStream = typeof (HtmlReport).Assembly.GetManifestResourceStream("RuleRunner.Reports.Html.Templates.Index.cshtml"))
            {
                using (var reader = new StreamReader(indexTemplateStream))
                {                    
                    var indexPath = Path.Combine(this.OutputDirectory, "index.html");

                    var context = new ExecuteContext();

                    var templateConfig = new TemplateServiceConfiguration()
                    {
                        BaseTemplateType = typeof (ReportTemplateBase<>),
                        Resolver = new EmebeddedTemplateResolver(typeof(HtmlReport).Assembly, "RuleRunner.Reports.Html.Templates")
                    };

                    var templateService = new TemplateService(templateConfig);

                    var template = templateService.Resolve("Index", "");

                    var result = template.Run(context);

                    File.WriteAllText(indexPath, result);
                }
            }            
        }
    }

    public class EmebeddedTemplateResolver : ITemplateResolver
    {
        private readonly Assembly assembly;
        private readonly string baseName;

        public EmebeddedTemplateResolver(Assembly assembly, string baseName)
        {
            this.assembly = assembly;
            this.baseName = baseName;
        }

        public string Resolve(string name)
        {
            var resourceName = baseName + "." + name + ".cshtml";

            using (var resourceStream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(resourceStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}