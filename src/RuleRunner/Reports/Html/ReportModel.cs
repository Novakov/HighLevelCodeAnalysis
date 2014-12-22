using System.Collections.Generic;
using CodeModel;
using CodeModel.Rules;
using RuleRunner.Configuration;

namespace RuleRunner.Reports.Html
{
    public class ReportModel
    {
        public RunConfiguration Configuration { get; set; }
        public RunList<StepDescriptor> RunList { get; set; }
        public IDictionary<IRule, IList<Violation>> Violations { get; private set; }

        public ReportModel()
        {
            this.Violations = new Dictionary<IRule, IList<Violation>>();
        }
    }
}