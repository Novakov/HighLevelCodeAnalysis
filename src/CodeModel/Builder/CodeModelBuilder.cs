using System;
using System.Linq;
using System.Reflection;
using CodeModel.Convetions;
using CodeModel.Graphs;
using TinyIoC;

namespace CodeModel.Builder
{
    public class CodeModelBuilder
    {
        private readonly TinyIoCContainer container;

        public Graph Model { get; private set; }

        public CodeModelBuilder()
        {
            this.Model = new Graph();

            this.container = new TinyIoCContainer();            
        }

        public void RegisterConventionsFrom(params Assembly[] assemblies)
        {
            var toRegister = from assembly in assemblies
                from type in assembly.GetTypes()
                where typeof (IConvention).IsAssignableFrom(type)
                from @interface in type.GetInterfaces()
                where typeof (IConvention).IsAssignableFrom(@interface)
                select new {Interface = @interface, Implementation = type};

            foreach (var item in toRegister)
            {
                this.container.Register(item.Interface, item.Implementation);
            }
        }

        public void RunMutator<TMutator>(TMutator mutator)
            where TMutator : class, IMutator
        {
            RunMutator<ICompositeMutator>(mutator, m => m.Mutate(this));
            RunMutator<IGraphMutator>(mutator, m => m.Mutate(this.Model));
            RunMutator<INodeMutator>(mutator, MutateNodes);
        }

        public void RunMutator<TMutator>()
            where TMutator : class, IMutator
        {
            RunMutator(this.container.Resolve<TMutator>());
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
