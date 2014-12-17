using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CodeModel.Builder;
using CodeModel.Mutators;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;
using RuleRunner.Configuration;

namespace RuleRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = LoadConfiguration(args[0]);

            var run = new Run(config);

            run.Execute();
        }

        private static RunConfiguration LoadConfiguration(string path)
        {
            var serializer = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var configText = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<RunConfiguration>(configText, serializer);
        }
    }

    internal class Run
    {
        private static readonly Logger Log = LogManager.GetLogger("Run");

        private readonly RunConfiguration config;
        private List<Assembly> assembliesToAnalyze;
        private List<Assembly> conventionAssemblies;
        private CodeModelBuilder modelBuilder;

        public Run(RunConfiguration config)
        {
            this.config = config;
        }

        public void Execute()
        {
            Log.Info("Executing run");
            
            LoadAssembliesToAnalyze();
            LoadConventionAssemblies();
            
            BuildModel();
        }

        private void LoadConventionAssemblies()
        {
            Log.Info("Loading convention assemblies");
            this.conventionAssemblies = LoadAssemblies(this.config.ConventionAssemblies);
        }

        private void LoadAssembliesToAnalyze()
        {
            Log.Info("Loading assemblies to analyze");

            this.assembliesToAnalyze = LoadAssemblies(this.config.AssembliesToAnalyze);
        }

        private void BuildModel()
        {
            Log.Info("Building model");
            this.modelBuilder = new CodeModelBuilder();

            Log.Trace("Adding assemblies to model");
            this.modelBuilder.RunMutator(new AddAssemblies(this.assembliesToAnalyze));
        }

        private List<Assembly> LoadAssemblies(string[] assemblyPaths)
        {
            var list = new List<Assembly>();

            foreach (var assemblyPath in assemblyPaths)
            {
                Log.Debug("Loading assembly {0}", assemblyPath);

                var assembly = Assembly.LoadFrom(assemblyPath);

                list.Add(assembly);
            }

            return list;
        }
    }
}
