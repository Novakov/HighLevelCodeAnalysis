using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CodeModel.Builder;
using CodeModel.Convetions;
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

                var graphRule = rule as IGraphRule;
                if (graphRule != null)
                {
                    graphRule.Verify(context, codeModel.Model);
                }
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
    }
}
