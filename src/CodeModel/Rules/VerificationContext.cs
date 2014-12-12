using System.Collections.Generic;
using CodeModel.Graphs;
using CodeModel.Symbols;

namespace CodeModel.Rules
{
    public class VerificationContext
    {
        private readonly List<Violation> violations;

        public IEnumerable<Violation> Violations { get { return this.violations; } }

        public VerificationContext()
        {
            this.violations = new List<Violation>();
        }

        public Violation RecordViolation(object rule, Node errorNode, string category, SourceLocation? sourceLocation)
        {
            var violation = new Violation(rule, errorNode, category, sourceLocation);
            this.violations.Add(violation);

            return violation;
        }
    }
}