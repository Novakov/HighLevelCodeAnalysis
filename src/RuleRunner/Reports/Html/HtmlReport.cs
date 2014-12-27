using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CodeModel;
using CodeModel.Builder;
using CodeModel.Graphs;
using CodeModel.Rules;
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
                BaseTemplateType = typeof(ReportTemplateBase<>),
                Resolver = new EmebeddedTemplateResolver(typeof(HtmlReport).Assembly, "RuleRunner.Reports.Html.Templates"),
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

        public void Violation(Violation violation)
        {
            
        }

        public void VerificationRule(IRule rule)
        {
            this.reportModel.Violations.Add(rule, new RuleResult(rule));
        }

        public void NodeVerification(IRule rule, Node node, IEnumerable<Violation> violations)
        {
            var ruleResults = this.reportModel.Violations[rule];
            ruleResults.Verified++;
            
            var violationsList = violations as IList<Violation> ?? violations.ToList();
            
            ruleResults.TotalViolationsCount += violationsList.Count();

            if (violationsList.Any())
            {
                ruleResults.ViolatingNodesCount++;

                ruleResults.ViolatingNodes.Add(node, violationsList);
            }
            else
            {
                ruleResults.ComplyingNodesCount++;
            }           
        }

        public void GraphVerification(IRule rule, IEnumerable<Violation> violations)
        {
            var ruleResults = this.reportModel.Violations[rule];
            ruleResults.GraphViolations.AddRange(violations);
        }

        public void RunFinished(CodeModelBuilder modelBuilder)
        {
            this.reportModel.NodesCountByType = modelBuilder.Model.Nodes.GroupBy(x => x.GetType()).ToDictionary(x => x.Key, x => x.Count());
            this.reportModel.LinksCountByType = modelBuilder.Model.Links.GroupBy(x => x.GetType()).ToDictionary(x => x.Key, x => x.Count());
        }

        public void LoadedExtension(Assembly assembly)
        {
            this.reportModel.Extensions.Add(assembly.GetName());
        }
    }
}