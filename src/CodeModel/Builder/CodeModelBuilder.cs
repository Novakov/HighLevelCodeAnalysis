using System;
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
            }
        }

        private void RunMutator<TInterface>(IMutator mutator, Action<TInterface> action)
            where TInterface : class, IMutator
        {
            var @interface = mutator as TInterface;
            if (@interface != null)
            {
                action(@interface);
            }
        }
    }
}
