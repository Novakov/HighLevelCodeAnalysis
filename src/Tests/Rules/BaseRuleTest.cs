using CodeModel;
using CodeModel.Builder;
using CodeModel.Rules;
using NUnit.Framework;

namespace Tests.Rules
{
    public abstract class BaseRuleTest<TRule> 
        where TRule : class, IRule
    {
        protected Verificator Verificator { get; private set; }

        protected VerificationContext VerificationContext { get; private set; }

        [SetUp]
        public void BaseSetUp()
        {
            this.Verificator = new Verificator();             

            this.VerificationContext = new VerificationContext();            
        }

        protected void Verify(CodeModelBuilder codeModel)
        {
            this.Verificator.AddRule<TRule>();

            this.Verificator.Verify(this.VerificationContext, codeModel);
        }
    }
}