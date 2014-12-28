using System;
using System.Collections.Generic;
using System.Reflection;
using CodeModel;
using CodeModel.Dependencies;
using CodeModel.Graphs;
using CodeModel.RuleEngine;
using CodeModel.Rules;
using RuleRunner.Configuration;

namespace RuleRunner.Reports.Html
{
    public class ReportModel
    {
        public RunConfiguration Configuration { get; set; }
        public RunList<StepDescriptor> RunList { get; set; }
        public IDictionary<IRule, RuleResult> Violations { get; private set; }
        public Dictionary<Type, int> NodesCountByType { get; set; }
        public Dictionary<Type, int> LinksCountByType { get; set; }
        public List<AssemblyName> Extensions { get; private set; }

        public ReportModel()
        {
            this.Violations = new Dictionary<IRule, RuleResult>();
            this.Extensions = new List<AssemblyName>();
        }
    }

    public class RuleResult
    {
        public IRule Rule { get; private set; }

        public int Verified { get; set; }
        public int ViolatingNodesCount { get; set; }
        public int ComplyingNodesCount { get; set; }
        public int TotalViolationsCount { get; set; }

        public double ViolatingRatio { get { return this.ViolatingNodesCount/(double) this.Verified; } }
        
        public Dictionary<Node, IList<Violation>> ViolatingNodes { get; private set; }
        public bool AnyViolations { get { return this.ViolatingNodesCount > 0; } }
        public List<Violation> GraphViolations { get; private set; }

        public bool IsGraphRule
        {
            get { return this.Rule is IGraphRule; }
        }

        public bool IsNodeRule
        {
            get { return this.Rule is INodeRule; }
        }

        public RuleResult(IRule rule)
        {
            this.Rule = rule;
            this.ViolatingNodes = new Dictionary<Node, IList<Violation>>();
            this.GraphViolations = new List<Violation>();
        }
    }
}