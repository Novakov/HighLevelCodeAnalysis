using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeModel.Dependencies;
using CodeModel.Graphs;
using CodeModel.Primitives;
using CodeModel.RuleEngine;

namespace CodeModel.Rules
{
    [Need(Resources.Dependencies, Resources.InlinedImplementations)]
    public class NoCyclesInDependenciesRule : IGraphRule
    {
        public IEnumerable<Violation> Verify(VerificationContext context, Graph graph)
        {
            var finder = new FindCycles<Node, Link>()
            {
                NodeFilter = x => x is TypeNode,
                OutboundTargets = x => x.OutboundLinks.OfType<DependencyLink>().GroupBy(y => y.Target)
            };

            var cycles = finder.Find(graph);

            foreach (var cycle in cycles)
            {
                yield return new CycleInDependenciesViolation(cycle.OfType<TypeNode>());
            }
        }
    }

    [Violation]
    public class CycleInDependenciesViolation : Violation
    {
        public IEnumerable<TypeNode> DependencyCycle { get; private set; }

        public CycleInDependenciesViolation(IEnumerable<TypeNode> dependencyCycle)
        {
            DependencyCycle = dependencyCycle;
        }

        public override string DisplayText
        {
            get { return "Cycle in dependencies: " + string.Join(", ", this.DependencyCycle.Select(x => x.Type.FullName)); }
        }
    }
}
