using System.Globalization;
using System.Reflection;
using CodeModel.Graphs;
using CodeModel.Symbols;

namespace CodeModel.RuleEngine
{  
    public abstract class Violation
    {       
        public IRule Rule { get; internal set; }

        public string Name
        {
            get { return this.GetType().Name; }
        }

        public string DisplayText
        {
            get { return this.FormatDisplayTextWith(new DefaultFormatter(CultureInfo.CurrentUICulture)); }
        }

        public string FormatDisplayTextWith(IValueFormatter formatter)
        {
            var attribute = this.GetType().GetCustomAttribute<ViolationAttribute>();

            if (attribute == null || attribute.DisplayText == null)
            {
                return this.Name;
            }

            var description = attribute.DisplayText.Interpolate(this, formatter);

            return this.Name + ": " + description;
        }

        public override string ToString()
        {
            return this.DisplayText;
        }
    }

    public interface IViolationWithSourceLocation
    {
        SourceLocation? SourceLocation { get; }
    }

    public interface INodeViolation
    {
        Node Node { get; }
    }
}