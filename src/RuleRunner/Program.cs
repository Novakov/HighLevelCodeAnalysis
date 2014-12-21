using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CodeModel;
using CodeModel.Builder;
using CodeModel.Graphs;
using CodeModel.Rules;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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

    public class StepDescriptor
    {
        public Type Type { get; private set; }

        public IEnumerable<string> Provides { get; private set; }

        public IEnumerable<string> Needs { get; set; }

        public bool IsRule { get; private set; }

        public bool IsMutator { get; private set; }

        public StepDescriptor(Type type)
        {
            this.Type = type;

            var provideAttribute = type.GetCustomAttribute<ProvideAttribute>();
            if (provideAttribute != null)
            {
                this.Provides = provideAttribute.Provides;
            }
            else
            {
                this.Provides = Enumerable.Empty<string>();
            }


            var needAttribute = type.GetCustomAttribute<NeedAttribute>();
            if (needAttribute != null)
            {
                this.Needs = needAttribute.Needs;
            }
            else
            {
                this.Needs = Enumerable.Empty<string>();
            }

            this.IsRule = typeof (IRule).IsAssignableFrom(this.Type);
            this.IsMutator = typeof (IMutator).IsAssignableFrom(this.Type);
        }

        public override string ToString()
        {
            return this.Type.Name;
        }
    }
}
