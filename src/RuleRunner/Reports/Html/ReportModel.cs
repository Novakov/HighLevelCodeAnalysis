using CodeModel;
using RuleRunner.Configuration;

namespace RuleRunner.Reports.Html
{
    public class ReportModel
    {
        public RunConfiguration Configuration { get; set; }
        public RunList<StepDescriptor> RunList { get; set; }
    }
}