using System;
using System.Globalization;
using CodeModel;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class StringInterpolateTest
    {
        [Test]
        public void ShouldReturnTheSameStringIfNoPlaceholders()
        {
            // arrange
            var input = "Sample text";

            // act
            var result = input.Interpolate(new { });

            // assert
            Assert.That(result, Is.EqualTo("Sample text"));
        }

        [Test]
        public void ShouldInterpolateStringProperty()
        {
            // arrange
            var input = "Sample text with placeholder {Property} and {Property2}!";

            // act
            var result = input.Interpolate(new { Property = "here", Property2 = "also here" });

            // assert
            Assert.That(result, Is.EqualTo("Sample text with placeholder here and also here!"));
        }

        [Test]
        public void ShouldInterpolateIntegerProperty()
        {
            // arrange
            var input = "{A} + {B} = {C}!";

            // act
            var result = input.Interpolate(new { A = 2, B = 3, C = 5 });

            // assert
            Assert.That(result, Is.EqualTo("2 + 3 = 5!"));
        }

        [Test]
        public void ShouldInterpolateDoubleWithFormattingProperty()
        {
            // arrange
            var input = "{Text} {A:0.####} + {B:0.####} = {C:0.####}!";

            // act
            var result = input.Interpolate(new { Text = "e.g.", A = 1 / 7.0, B = 2 / 7.0, C = 3 / 7.0 });

            // assert
            Assert.That(result, Is.EqualTo("e.g. 0,1429 + 0,2857 = 0,4286!"));
        }

        [Test]
        public void ShouldThrowExceptionWhenUsingNotExistingProperty()
        {
            // arrange
            var input = "{NotExistingProperty}";

            // act & assert
            Assert.That(() => input.Interpolate(new { }), Throws
                .InvalidOperationException
                .With.Message.EqualTo("Property 'NotExistingProperty' does not exist")
            );
        }

        [Test]
        public void ShouldPutEmptyStringWhenPropertyIsNull()
        {
            // arrange
            var input = "'{Property}'";

            // act
            var result = input.Interpolate(new { Property = (string)null });

            // assert
            Assert.That(result, Is.EqualTo("''"));
        }

        [Test]
        public void ShouldUseCustomFormatter()
        {
            // arrange
            var input = "Int = {A}, StringAsHtml = {B}";

            var formatter = new StringAsHtmlFormatter(CultureInfo.CurrentUICulture);

            // act
            var result = input.Interpolate(new { A = 2, B = "test" }, formatter);

            // assert
            Assert.That(result, Is.EqualTo("Int = 2, StringAsHtml = <strong>test</strong>"));
        }

        public class StringAsHtmlFormatter : DefaultFormatter
        {
            public StringAsHtmlFormatter(IFormatProvider formatProvider) : base(formatProvider)
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