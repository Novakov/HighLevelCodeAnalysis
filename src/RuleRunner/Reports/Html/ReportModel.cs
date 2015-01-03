using System;
using System.Collections.Generic;
using System.Linq;
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

        public NodesVerificationResult NodesVerification { get; private set; }

        public GraphVerificationResult GraphVerification { get; private set; }

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

            this.NodesVerification = new NodesVerificationResult();
            this.GraphVerification = new GraphVerificationResult();
        }
    }

    public class NodesVerificationResult
    {
        public int VerifiedCount { get; set; }
        public int ViolatingNodesCount { get; set; }
        public int ComplyingNodesCount { get; set; }
        public int ViolationsCount { get; set; }
        public double ViolatingRatio { get { return this.ViolatingNodesCount / (double)this.VerifiedCount; } }
        public IDictionary<Node, IList<Violation>> ViolationsByNode { get; private set; }

        public bool AnyViolations { get { return this.ViolationsByNode.Any(); } }

        public NodesVerificationResult()
        {
            this.ViolationsByNode = new Dictionary<Node, IList<Violation>>();
        }
    }

    public class GraphVerificationResult
    {
        public List<Violation> Violations { get; private set; }
        public int ViolationsCount { get { return this.Violations.Count; } }
        public bool AnyViolations { get { return this.ViolationsCount > 0; } }

        public GraphVerificationResult()
        {
            this.Violations = new List<Violation>();
        }
    }
}