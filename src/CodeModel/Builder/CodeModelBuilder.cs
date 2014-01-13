using System;
using System.Linq;
using CodeModel.Graphs;

namespace CodeModel.Builder
{
    public class CodeModelBuilder
    {
        public Graph Model { get; private set; }

        public CodeModelBuilder()
        {
            this.Model = new Graph();
        }

        public void RunMutators(params IMutator[] mutators)
        {            
            foreach (var mutator in mutators)
            {
                RunMutator<IGraphMutator>(mutator, m => m.Mutate(this.Model));
                RunMutator<INodeMutator>(mutator, MutateNodes);
            }
        }

        private void MutateNodes(INodeMutator mutator)
        {            
            var implementedInterfaces = mutator.GetType().GetGenericImplementationsOfInterface(typeof(INodeMutator<>));

            foreach (var interfaceType in implementedInterfaces)
            {
                var nodeType = interfaceType.GenericTypeArguments[0];
                
                var mutateMethod = interfaceType.GetMethod("Mutate");

                var context = new MutateContext(this.Model);

                foreach (var node in context.NodesToProcess().OfType(nodeType))
                {
                    mutateMethod.Invoke(mutator, new object[] { node, context });
                }
            }
        }

        private void RunMutator<TInterface>(IMutator mutator, Action<TInterface> action)
            where TInterface : class
        {
            var @interface = mutator as TInterface;
            if (@interface != null)
            {
                action(@interface);
            }
        }
    }
}
