using CodeModel;
using CodeModel.Builder;
using CodeModel.Rules;
using NUnit.Framework;

namespace Tests.Rules
{
    public abstract class BaseRuleTest<TRule> 
        where TRule : class, IRule, new()
    {
        protected Verificator Verificator { get; private set; }

        protected VerificationContext VerificationContext { get; private set; }

        [SetUp]
        public void SetUp()
        {
            this.Verificator = new Verificator()
                .AddRule<TRule>();

            this.VerificationContext = new VerificationContext();
        }

        protected void Verify(CodeModelBuilder codeModel)
        {
            this.Verificator.Verify(this.VerificationContext, codeModel);
        }
    }
}