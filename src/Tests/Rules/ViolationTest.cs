using System;
using System.Globalization;
using System.Reflection;
using CodeModel;
using CodeModel.RuleEngine;
using NUnit.Framework;

namespace Tests.Rules
{
    [TestFixture]
    public class ViolationTest
    {
        [Test]
        public void ShouldBuildProperDisplayTextBasedOnAttribute()
        {
            // arrange
            var violation = new SampleViolation("AwesomeMethod");

            // act
            var displayText = violation.DisplayText;

            // assert
            Assert.That(displayText, Is.EqualTo("SampleViolation: Display text is composed of AwesomeMethod"));
        }

        [Test]
        public void ShouldBuildDisplayTextUsingSpecifiedFormatter()
        {
            // arrange                        
            var violation = new SampleViolation("AwesomeMethod");

            var formatter = new StringAsHtmlFormatter(CultureInfo.CurrentUICulture);

            // act
            var displayText = violation.FormatDisplayTextWith(formatter);

            // assert
            Assert.That(displayText, Is.EqualTo("SampleViolation: Display text is composed of <strong>AwesomeMethod</strong>"));
        }

        [Violation(DisplayText = "Display text is composed of {MethodName}")]
        private class SampleViolation : Violation
        {
            public string MethodName { get; private set; }

            public SampleViolation(string methodName)
            {
                MethodName = methodName;
            }
        }

        private class StringAsHtmlFormatter : DefaultFormatter
        {
            public StringAsHtmlFormatter(IFormatProvider formatProvider)
                : base(formatProvider)
            {
            }

            public override string Format(object value, string format)
            {
                if (value is string)
                {
                    return "<strong>" + value + "</strong>";
                }

                return base.Format(value, format);
            }
        }
    }
}