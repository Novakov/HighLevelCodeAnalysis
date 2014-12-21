using System;
using System.IO;
using System.Reflection;
using RazorEngine.Templating;

namespace RuleRunner.Reports.Html
{
    public class EmebeddedTemplateResolver : ITemplateResolver
    {
        private readonly Assembly assembly;
        private readonly string baseName;

        public EmebeddedTemplateResolver(Assembly assembly, string baseName)
        {
            this.assembly = assembly;
            this.baseName = baseName;
        }

        public string Resolve(string name)
        {
            var resourceName = baseName + "." + name + ".cshtml";
            
            using (var resourceStream = assembly.GetManifestResourceStream(resourceName))
            {
                if (resourceStream == null)
                {
                    throw new InvalidOperationException("View " + name + " not found");
                }

                using (var reader = new StreamReader(resourceStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}