using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CodeModel;
using CodeModel.Builder;
using CodeModel.Extensions.DgmlExport;
using CodeModel.Links;
using CodeModel.Mutators;
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

            LoadExtensions();

            LoadAssembliesToAnalyze();
            LoadConventionAssemblies();

            BuildModel();

            RunRules();

            ExportModelAsDgml();

            this.Report.RunFinished(this.modelBuilder);

            if (this.Report != null)
            {
                this.Report.Write();
            }
        }

        private void LoadExtensions()
        {
            foreach (var path in this.config.ExtensionAssemblies)
            {
                Log.Info("Loading extension {0}", path);
                var extensionAssembly = Assembly.LoadFrom(path);

                this.Report.LoadedExtension(extensionAssembly);
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
            foreach (var violation in violations)
            {
                Log.Warn("Category: {1} on {0}", violation.Node, violation.Name);
            }
        }

        private Assembly CurrentDomainOnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            Log.Trace("Resolving assembly {0}", args.Name);

            return null;
        }

        private void ExportModelAsDgml()
        {
            this.modelBuilder.RunMutator(new RemoveLink<ContainedInLink>(x => true));

            var exporter = new DgmlExporter();

            using (var fs = File.Create(@"d:\tmp\360.dgml"))
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

            var mutators = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => typeof(IMutator).IsAssignableFrom(x) && x.IsClass && !x.IsAbstract)
                .Select(x => new StepDescriptor(this.modelBuilder, x));

            var rules = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => typeof(IRule).IsAssignableFrom(x) && x.IsClass && !x.IsAbstract)
                .Select(x => new StepDescriptor(this.modelBuilder, x))
                .ToList();

            Log.Info("Determining runlist");

            this.runlist = DetermineRunList(mutators, rules);

            this.Report.RunList(runlist);

            Log.Info("Runlist valid: {0}", runlist.IsValid);

            if (runlist.IsValid)
            {
                Log.Info("Order: {0}", string.Join(", ", runlist.Elements));
            }
            else
            {
                Log.Error("Runlist error: {0}", string.Join(",", runlist.Errors));
                Log.Error("Missing resources: {0}", string.Join(",", runlist.Missing));
            }

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