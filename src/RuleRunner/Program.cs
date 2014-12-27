using System.IO;
using System.Text;
using System.Threading.Tasks;
using CodeModel.Graphs;
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
}
