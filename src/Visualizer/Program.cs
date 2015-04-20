using System.IO;
using Newtonsoft.Json;
using Visualizer.Configuration;

namespace Visualizer
{
    class Program
    {
        static void Main(string[] args)
        {
            var configurationPath = args[0];

            var configuration = LoadConfiguration(configurationPath);

            new Run(configuration).Execute();
        }

        private static RunConfiguration LoadConfiguration(string filePath)
        {
            var json = File.ReadAllText(filePath);

            return JsonConvert.DeserializeObject<RunConfiguration>(json);
        }
    }
}
