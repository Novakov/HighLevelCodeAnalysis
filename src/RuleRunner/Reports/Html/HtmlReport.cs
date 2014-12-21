using System.IO;
using CodeModel;
using Newtonsoft.Json;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using RuleRunner.Configuration;

namespace RuleRunner.Reports.Html
{
    public class HtmlReport
    {
        private readonly ReportModel reportModel;

        public HtmlReport()
        {
            reportModel = new ReportModel();
        }

        [JsonProperty("output")]
        public string OutputDirectory { get; private set; }     

        public void Write()
        {
            if (!Directory.Exists(this.OutputDirectory))
            {
                Directory.CreateDirectory(this.OutputDirectory);
            }
            
            var indexPath = Path.Combine(this.OutputDirectory, "index.html");

            var context = new ExecuteContext();

            var templateConfig = new TemplateServiceConfiguration()
            {
                BaseTemplateType = typeof (ReportTemplateBase<>),
                Resolver = new EmebeddedTemplateResolver(typeof (HtmlReport).Assembly, "RuleRunner.Reports.Html.Templates"),
                Namespaces =
                {
                    "RuleRunner.Reports.Html"
                }
            };

            var templateService = new TemplateService(templateConfig);

            var template = templateService.Resolve("Index", reportModel);

            var result = template.Run(context);

            File.WriteAllText(indexPath, result);
        }

        public void Configuration(RunConfiguration config)
        {
            this.reportModel.Configuration = config;
        }

        public void RunList(RunList<StepDescriptor> runlist)
        {
            this.reportModel.RunList = runlist;
        }
    }
}