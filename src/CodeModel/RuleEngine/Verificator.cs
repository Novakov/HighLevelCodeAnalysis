using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CodeModel.Builder;
using CodeModel.Conventions;
using CodeModel.Graphs;
using TinyIoC;

namespace CodeModel.RuleEngine
{
    public class Verificator
    {
        private readonly List<IRule> rules;
        private readonly TinyIoCContainer container;

        public event EventHandler<RuleRunEventArgs> StartingRule;
        public event EventHandler<RuleRunEventArgs> FinishedRule;
        public event EventHandler<NodeVerificationEventArgs> NodeVerified;
        public event EventHandler<GraphVerifiedEventArgs> GraphVerified;

        public Verificator()
        {
            this.rules = new List<IRule>();
            this.container = new TinyIoCContainer();
        }

        public Verificator AddRule<TRule>()
            where TRule : class, IRule
        {
            rules.Add(container.Resolve<TRule>());

            return this;
        }

        public Verificator AddRule(Type ruleType)
        {
            rules.Add((IRule)container.Resolve(ruleType));

            return this;
        }

        public void Verify(VerificationContext context, CodeModelBuilder codeModel)
        {
            foreach (var rule in this.rules)
            {
                context.CurrentRule = rule;

                this.OnStartingRule(rule);

                var bootstrapRule = rule as IRuleWithBootstrap;

                if (bootstrapRule != null)
                {
                    bootstrapRule.Initialize(context, codeModel.Model);
                }

                var nodeRule = rule as INodeRule;
                if (nodeRule != null)
                {
                    foreach (var node in codeModel.Model.Nodes)
                    {
                        if (nodeRule.IsApplicableTo(node))
                        {
                            var violations = nodeRule.Verify(context, node).ToList();

                            this.OnNodeVerified(rule, node, violations);

                            context.RecordAll(violations);
                        }
                    }
                }

                var graphRule = rule as IGraphRule;
                if (graphRule != null)
                {
                    var violations = graphRule.Verify(context, codeModel.Model).ToList();

                    this.OnGraphVerified(rule, violations);

                    context.RecordAll(violations);
                }

                this.OnRuleFinished(rule);
            }
        }

        public void RegisterConventionsFrom(params Assembly[] assemblies)
        {
            var toRegister = from assembly in assemblies
                             from type in assembly.GetTypes()
                             where typeof(IConvention).IsAssignableFrom(type)
                             from @interface in type.GetInterfaces()
                             where typeof(IConvention).IsAssignableFrom(@interface)
                             select new { Interface = @interface, Implementation = type };

            foreach (var item in toRegister)
            {
                this.container.Register(item.Interface, item.Implementation);
            }
        }

        protected void OnStartingRule(IRule rule)
        {
            this.StartingRule.Call(this, new RuleRunEventArgs(rule));
        }

        protected void OnRuleFinished(IRule rule)
        {
            this.FinishedRule.Call(this, new RuleRunEventArgs(rule));
        }

        protected void OnNodeVerified(IRule rule, Node node, IEnumerable<Violation> violations)
        {
            this.NodeVerified.Call(this, new NodeVerificationEventArgs(rule, node, violations));
        }

        protected void OnGraphVerified(IRule rule, IEnumerable<Violation> violations)
        {
            this.GraphVerified.Call(this, new GraphVerifiedEventArgs(rule, violations));
        }
    }

    public class GraphVerifiedEventArgs : EventArgs
    {
        public IRule Rule { get; private set; }
        public IEnumerable<Violation> Violations { get; private set; }

        public GraphVerifiedEventArgs(IRule rule, IEnumerable<Violation> violations)
        {
            Rule = rule;
            Violations = violations;
        }
    }

    public class NodeVerificationEventArgs : EventArgs
    {
        public IRule Rule { get; private set; }
        public Node Node { get; private set; }
        public IEnumerable<Violation> Violations { get; private set; }

        public NodeVerificationEventArgs(IRule rule, Node node, IEnumerable<Violation> violations)
        {
            Rule = rule;
            Node = node;
            Violations = violations;
        }
    }

    public class RuleRunEventArgs : EventArgs
    {
        public IRule Rule { get; private set; }

        public RuleRunEventArgs(IRule rule)
        {
            Rule = rule;
        }
    }
}
