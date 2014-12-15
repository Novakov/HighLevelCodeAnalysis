using System.Collections.Generic;

namespace CodeModel.Rules
{
    public class VerificationContext
    {
        private readonly List<Violation> violations;

        public IEnumerable<Violation> Violations { get { return this.violations; } }

        internal IRule CurrentRule { get; set; }

        public VerificationContext()
        {
            this.violations = new List<Violation>();
        }        

        public void RecordViolation(Violation violation)
        {
            violation.Rule = this.CurrentRule;
            this.violations.Add(violation);
        }
    }
}