using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CodeModel;
using CodeModel.Builder;
using CodeModel.Dependencies;
using CodeModel.Extensions.DgmlExport;
using CodeModel.Graphs;
using CodeModel.Primitives;
using CodeModel.Primitives.Mutators;
using CodeModel.RuleEngine;
using CodeModel.Rules;
using NLog;
using RuleRunner.Configuration;
using RuleRunner.Reports.Html;

namespace RuleRunner
{
    internal class Run
    {
        private static readonly Logger Log = LogManager.GetLogger("Run");

        private readonly RunConfiguration config;
        private List<Assembly> assembliesToAnalyze;
        private List<Assembly> conventionAssemblies;
        private CodeModelBuilder modelBuilder;
        private RunList<StepDescriptor> runlist;
        private Verificator verificator;
        private VerificationContext verificationContext;
        private List<Assembly> toolkitAssemblies; 

        private HtmlReport Report { get { return this.config.Reports.Html; } }

        public Run(RunConfiguration config)
        {
            this.config = config;
        }

        public void Execute()
        {
            Log.Info("Executing run");

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainOnAssemblyResolve;

            this.Report.Configuration(this.config);

            this.toolkitAssemblies = new List<Assembly>();

            LoadExtensions();

            LoadAssembliesToAnalyze();
            LoadConventionAssemblies();

            BuildModel();

            RunRules();

            if (this.config.Reports.Dgml != null)
            {
                ExportModelAsDgml();
            }

            this.Report.RunFinished(this.modelBuilder);

            if (this.Report != null)
            {
                this.Report.Write();
            }
        }

        private void LoadExtensions()
        {
            this.toolkitAssemblies.Add(typeof(Graph).Assembly);

            foreach (var path in this.config.ExtensionAssemblies)
            {
                Log.Info("Loading extension {0}", path);
                var extensionAssembly = Assembly.LoadFrom(path);

                this.Report.LoadedExtension(extensionAssembly);

                this.toolkitAssemblies.Add(extensionAssembly);
            }
        }

        private void RunRules()
        {
            this.verificator = new Verificator();
            this.verificator.RegisterConventionsFrom(this.conventionAssemblies.ToArray());

            this.verificator.StartingRule += (s, e) =>
            {
                Log.Debug("Starting rule {0}", e.Rule.GetType().Name);
                this.Report.VerificationRule(e.Rule);
            };

            this.verificator.NodeVerified += (s, e) =>
            {
                this.Report.NodeVerification(e.Rule, e.Node, e.Violations);
            };

            this.verificator.GraphVerified += (s, e) =>
            {
                this.Report.GraphVerification(e.Rule, e.Violations);
            };

            this.verificator.FinishedRule += (s, e) => Log.Info("Finished rule {0}", e.Rule.GetType().Name);

            this.verificationContext = new VerificationContext();

            foreach (var rule in this.runlist.Elements.Where(x => x.IsRule))
            {
                this.verificator.AddRule(rule.Type);
            }

            try
            {
                this.verificator.Verify(this.verificationContext, this.modelBuilder);
            }
            catch (Exception e)
            {
                Log.Fatal("Error running rules", e);
            }

            ReportViolations(this.verificationContext.Violations);
        }

        private void ReportViolations(IEnumerable<Violation> violations)
        {
            Log.Warn("Violations count: {0}", violations.Count());
        }

        private Assembly CurrentDomainOnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            Log.Trace("Resolving assembly {0}", args.Name);

            var name = new AssemblyName(args.Name);

            return this.config.ResolvePaths
                .Select(x => Path.Combine(x, name.Name + ".dll"))
                .Where(File.Exists)
                .Select(Assembly.LoadFrom)
                .FirstOrDefault();
        }

        private void ExportModelAsDgml()
        {
            this.modelBuilder.RunMutator(new RemoveLink<ContainedInLink>(x => true));

            var exporter = new DgmlExporter();

            using (var fs = File.Create(this.config.Reports.Dgml.Path))
            {
                exporter.Export(this.modelBuilder.Model, fs);
            }
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

            this.modelBuilder.RegisterConventionsFrom(this.conventionAssemblies.ToArray());

            Log.Trace("Adding assemblies to model");
            this.modelBuilder.RunMutator(new AddAssemblies(this.assembliesToAnalyze));

            var enabledRules = this.toolkitAssemblies
               .SelectMany(x => x.GetTypes())
               .Where(x => typeof(IRule).IsAssignableFrom(x) && x.IsClass && !x.IsAbstract)
               .Where(x => !this.config.DisabledRules.Contains(x.Name))
               .ToList();

            Log.Info("Enabled rules: {0}", string.Join(", ", enabledRules.Select(x => x.Name)));

            var mutators = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => typeof(IMutator).IsAssignableFrom(x) && x.IsClass && !x.IsAbstract)
                .Select(x => new StepDescriptor(this.modelBuilder, x));
           
            var rules = enabledRules
                .Select(x => new StepDescriptor(this.modelBuilder, x))
                .ToList();

            Log.Info("Determining runlist");

            try
            {
                this.runlist = DetermineRunList(mutators, rules);
            }
            catch (NeedsNotSatisfiedException e)
            {
                Log.Error("Missing resources: {0}", e.MissingResource);
                throw;
            }
            catch (UnableToBuildRunListException e)
            {
                Log.Error("Unable to build runlist {0}", e.InnerException.Message);
                throw;
            }

            this.Report.RunList(runlist);

            Log.Info("Order: {0}", string.Join(", ", runlist.Elements));

            foreach (var mutatorType in this.runlist.Elements.Where(x => x.IsMutator))
            {
                Log.Debug("Running mutator {0}", mutatorType);
                this.modelBuilder.RunMutator(mutatorType.Type);
            }
        }

        private RunList<StepDescriptor> DetermineRunList(IEnumerable<StepDescriptor> descriptors, IList<StepDescriptor> rules)
        {
            var manager = new DependencyManager<StepDescriptor>(x => x.Provides, x => x.Needs, x => x.OptionalNeeds);

            manager.AddRange(descriptors);
            manager.AddRange(rules);

            manager.RequireElements(rules);

            return manager.CalculateRunList();
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