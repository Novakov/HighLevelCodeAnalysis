using System.Collections.Generic;
using CodeModel.Graphs;

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

        public void RecordViolation(object rule, Node errorNode, string category)
        {
            this.violations.Add(new Violation(rule, errorNode, category));
        }
    }
}