using System;
using System.Linq;
using System.Reflection;
using CodeModel.Builder;
using CodeModel.Dependencies;
using CodeModel.Primitives.Mutators;
using NLog;
using Visualizer.Configuration;
using Visualizer.Outputs;

namespace Visualizer
{
    internal class Run
    {
        private static readonly Logger Log = LogManager.GetLogger("Run");

        private readonly RunConfiguration config;

        public Run(RunConfiguration config)
        {
            this.config = config;
        }

        public void Execute()
        {
            Log.Info("Executing run");

            Log.Info("Loading extensions");

            foreach (var assembly in this.config.ExtensionAssemblies)
            {
                Log.Debug("Loading extension {0}", assembly);
                Assembly.LoadFrom(assembly);
            }

            foreach (var output in this.config.Outputs)
            {
                GenerateOutput(output);                
            }

            Log.Info("Run finished");
        }

        private void GenerateOutput(OutputConfiguration output)
        {
            Log.Info("Generating output {0} (type: {1})", output.OutputPath, output.Type);

            var modelBuilder = SetUpCodeModelBuilder();
            RunList<StepDescriptor> runlist;
            
            try
            {
                runlist = BuildRunlist(modelBuilder, output.Resources);
            }
            catch (NeedsNotSatisfiedException e)
            {
                Log.Error("Unable to build runlist. Missing resource: {0}", e.MissingResource);
                return;
            }

            Log.Debug("Output runlist");
            foreach (var stepDescriptor in runlist.Elements.OfType<MutatorStepDescriptor>())
            {
                Log.Debug("Step: {0}", stepDescriptor);
                modelBuilder.RunMutator(stepDescriptor.Type);
            }

            Log.Info("Model built");

            var outputType = Type.GetType(typeof (IOutput).Namespace + "." + output.Type.CamelCase() + "Output");

            var outputer = (IOutput)Activator.CreateInstance(outputType); //FIXME: just fix...

            outputer.Write(output.OutputPath, modelBuilder);
        }

        private CodeModelBuilder SetUpCodeModelBuilder()
        {
            var modelBuilder = new CodeModelBuilder();

            foreach (var assembly in this.config.ConventionAssemblies)
            {
                modelBuilder.RegisterConventionsFrom(Assembly.LoadFrom(assembly));
            }

            foreach (var assembly in this.config.AssembliesToAnalyze)
            {
                modelBuilder.RunMutator(new AddAssemblies(Assembly.LoadFrom(assembly)));
            }

            return modelBuilder;
        }

        private RunList<StepDescriptor> BuildRunlist(CodeModelBuilder modelBuilder, string[] finalResources)
        {
            var mutators = AppDomain.CurrentDomain.GetAssemblies()
               .SelectMany(x => x.GetTypes())
               .Where(x => typeof(IMutator).IsAssignableFrom(x) && x.IsClass && !x.IsAbstract)
               .Select(x => new MutatorStepDescriptor(modelBuilder, x));

            var finalResourcesStep = new RequiredResources(finalResources);

            var dependencyNetwork = new DependencyManager<StepDescriptor>(x => x.Provides, x => x.Needs, x => x.OptionalNeeds);

            dependencyNetwork.AddRange(mutators);           
            dependencyNetwork.Add(finalResourcesStep);
            dependencyNetwork.RequireElements(finalResourcesStep);

            return dependencyNetwork.CalculateRunList();
        }
    }

    static class Extensions
    {
        public static string CamelCase(this string s)
        {
            switch (s.Length)
            {
                case 0:
                    return s;
                case 1:
                    return s.ToUpper();
                default:
                    return s.Substring(0, 1).ToUpper() + s.Substring(1);
            }
        }
    }
}