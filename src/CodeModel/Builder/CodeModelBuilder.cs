using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using CodeModel.Conventions;
using CodeModel.Dependencies;
using CodeModel.Graphs;
using CodeModel.Rules;
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
                             where typeof(IConvention).IsAssignableFrom(type)
                             from @interface in type.GetInterfaces()
                             where typeof(IConvention).IsAssignableFrom(@interface)
                             select new { Interface = @interface, Implementation = type };

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

        public void RunMutator(Type mutatorType)
        {
            RunMutator((IMutator)this.container.Resolve(mutatorType));
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
                    try
                    {
                        mutateMethod.Invoke(mutator, new object[] { node, context });
                    }
                    catch (TargetInvocationException e)
                    {
                        ExceptionDispatchInfo.Capture(e.InnerException).Throw();
                    }
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

        public IEnumerable<string> GetNeededResources(Type type)
        {
            var resources = new HashSet<string>();

            var needAttribute = type.GetCustomAttribute<NeedAttribute>();
            if (needAttribute != null)
            {
                resources.UnionWith(needAttribute.Needs);
            }

            if (type.GetCustomAttribute<DynamicNeed>() != null)
            {
                var instance = (IDynamicNeed)this.container.Resolve(type);
                resources.UnionWith(instance.NeededResources);
            }

            return resources;
        }

        public IEnumerable<string> GetProvidedResources(Type type)
        {
            var provideAttribute = type.GetCustomAttribute<ProvideAttribute>();
            if (provideAttribute != null)
            {
                return provideAttribute.Provides;
            }
            else
            {
                return Enumerable.Empty<string>();
            }
        }

        public IEnumerable<string> GetOptionalNeeds(Type type)
        {
            var attribute = type.GetCustomAttribute<OptionalNeedAttribute>();
            if (attribute != null)
            {
                return attribute.OptionalNeeds;
            }
            else
            {
                return Enumerable.Empty<string>();
            }
        }
    }
}
