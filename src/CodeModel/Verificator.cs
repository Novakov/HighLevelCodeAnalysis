using System.Collections.Generic;
using CodeModel.Builder;
using CodeModel.Rules;
using TinyIoC;

namespace CodeModel
{
    public class Verificator
    {
        private readonly List<IRule> rules;
        private readonly TinyIoCContainer container;

        public Verificator()
        {
            this.rules = new List<IRule>();
            this.container = new TinyIoCContainer();
        }

        public Verificator AddRule<TRule>()
            where TRule: class, IRule
        {
            rules.Add(container.Resolve<TRule>());

            return this;
        }

        public void Verify(VerificationContext context, CodeModelBuilder codeModel)            
        {
            foreach (var rule in this.rules)
            {
                var nodeRule = rule as INodeRule;
                if (nodeRule != null)
                {
                    foreach (var node in codeModel.Model.Nodes)
                    {
                        if (nodeRule.IsApplicableTo(node))
                        {
                            nodeRule.Verify(context, node);
                        }
                    }
                }
            }            
        }
    }
}
