using System;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;

namespace CodeModel
{
    public static class StringInterpolate
    {
        private static readonly Regex PlaceholderRegex = new Regex(@"{(?:(?<placeholder>.*?)(?::(?<format>.*?))?)}");

        public static string Interpolate(this string input, object model)
        {
            return input.Interpolate(model, CultureInfo.CurrentUICulture);
        }

        public static string Interpolate(this string input, object model, IFormatProvider culture)
        {
            var formatter = new DefaultFormatter(culture);

            return input.Interpolate(model, formatter);
        }

        public static string Interpolate(this string input, object model, IValueFormatter formatter)
        {
            var output = PlaceholderRegex.Replace(input, m => FillPlaceholder(m.Groups["placeholder"].Value, m.Groups["format"].Value, model, formatter));

            return output;
        }

        private static string FillPlaceholder(string valueSource, string format, object model, IValueFormatter formatter)
        {
            var property = model.GetType().GetProperty(valueSource);

            if (property == null)
            {
                throw new InvalidOperationException(string.Format("Property '{0}' does not exist", valueSource));
            }

            var value = property.GetValue(model);

            if (value == null)
            {
                return "";
            }

            return formatter.Format(value, format);
        }
    }

    public class DefaultFormatter : IValueFormatter
    {
        private readonly IFormatProvider formatProvider;

        public DefaultFormatter(IFormatProvider formatProvider)
        {
            this.formatProvider = formatProvider;
        }

        public virtual string Format(object value, string format)
        {
            if (!string.IsNullOrWhiteSpace(format) && value is IFormattable)
            {
                return ((IFormattable)value).ToString(format, this.formatProvider);
            }

            return value.ToString();
        }
    }

    public interface IValueFormatter
    {
        string Format(object value, string format);
    }    
}