using System;
using System.Linq;
using CodeModel.MonadicParser;
using NUnit.Framework;

namespace Tests.MonadicParserTests
{
    public class MonadicParserTest
    {
        [Test]
        public void ShouldParseSingleCharacter()
        {
            // arrange
            var input = "abc";

            // act
            var result = Parsers.AnyChar()(input);

            // assert
            Assert.That(result, Is
                .Not.Null
                .And.Property("Value").EqualTo('a')
                .And.Property("Rest").EqualTo("bc")
            );
        }

        [Test]
        public void ShouldParseThreeCharacters()
        {
            // arrange
            var input = "abc";

            // act
            var parser = from c1 in Parsers.AnyChar()
                         from c2 in Parsers.AnyChar()
                         from c3 in Parsers.AnyChar()
                         select c1 + "*" + c2 + "*" + c3;

            var result = parser(input);

            // assert
            Assert.That(result.Value, Is.EqualTo("a*b*c"));
        }

        [Test]
        public void ShouldParseManyCharacters()
        {

            // arrange
            var input = "abc";

            // act
            var parser = from chars in Parsers.AnyChar().Many()
                         select string.Join("*", chars);

            var result = parser(input);

            // assert
            Assert.That(result.Value, Is.EqualTo("a*b*c"));
        }

        [Test]
        [TestCase("1234", 1234)]
        [TestCase("1", 1)]
        [TestCase("-1", -1)]
        public void ShouldParseNumber(string input, int expected)
        {
            // arrange
            var parser =
                from sign in Parsers.Char('-').Optional('+')
                from c in Parsers.AnyChar().Many().Reverse()
                let signNum = sign == '-' ? -1 : 1
                select signNum * c.Select((x, i) => int.Parse(x.ToString()) * (int)Math.Pow(10, i)).Sum();

            // act
            var result = parser(input);

            // assert
            Assert.That(result, Is.Not.Null
                .And.Property("Value").EqualTo(expected));
        }
    }
}
