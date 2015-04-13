using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeModel.Builder;
using CodeModel.Conventions;
using CodeModel.Dependencies;

namespace CodeModel.Primitives.Mutators
{
    [Need(Resources.Types)]
    [Provide(Resources.Dependencies)]
    public class LinkTypeDependencies : INodeMutator<TypeNode>
    {
        private readonly IDependencyConvention dependencyConvention;

        public LinkTypeDependencies(IDependencyConvention dependencyConvention)
        {
            this.dependencyConvention = dependencyConvention;
        }

        public void Mutate(TypeNode node, IMutateContext context)
        {
            var dependencies = this.dependencyConvention.GetDependenciesForType(node.Type);

            foreach (var dependency in dependencies)
            {
                var dependencyNode = context.FindNode<TypeNode>(t => t.Type == dependency);

                if (dependencyNode != null)
                {
                    context.AddLink(node, dependencyNode, new DependencyLink());
                }              
            }
        }
    }
}
