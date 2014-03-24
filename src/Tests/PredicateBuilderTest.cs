using System.Text;
using CodeModel;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class PredicateBuilderTest
    {
        [Test]
        public void ShouldReturnDefaultWhenNoClauses()
        {
            // arrange
            const string value = "value";

            var patternMatcher = PatternMatch.For<object, string>()
               .Default(_ => value)
               .AsDelegate();

            // act 
            var result = patternMatcher(null);

            // assert
            Assert.That(result, Is.EqualTo(value));
        }

        [Test]
        public void ShouldMatchWhenConditionalClause()
        {
            // arrange
            const string value = "value";

            const string expected = "aaaa";

            var patternMatcher = PatternMatch.For<string, string>()
               .When(x => x.StartsWith("v")).Return(_ => expected)
               .Default(_ => "x")
               .AsDelegate();

            // act 
            var result = patternMatcher(value);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void ShouldReturnDefaultWhenNoConditionalClauseMatches()
        {
            // arrange
            const string value = "Xvalue";

            const string expected = "X";

            var patternMatcher = PatternMatch.For<string, string>()
               .When(x => x.StartsWith("v")).Return(_ => "aaaa")
               .Default(_ => "X")
               .AsDelegate();

            // act 
            var result = patternMatcher(value);

            // assert
            Assert.That(result, Is.EqualTo(expected));
        }


        [Test]
        public void ShouldThrowWhenNoClauseMatches()
        {
            // arrange
            const string value = "Yvalue";


            var patternMatcher = PatternMatch.For<string, string>()
               .When(x => x.StartsWith("v")).Return(_ => "aaaa")
               .AsDelegate();

            // act & assert            
            Assert.That(() => patternMatcher(value), Throws.InvalidOperationException);
        }

        [Test]
        public void ShouldMatchByTypeAndCondition()
        {
            // arrange            
            var patternMatcher = PatternMatch.For<object, string>()
                .When<string>(x => x.StartsWith("v")).Return(_ => "str_cond")
                .When<string>().Return(_ => "str")
                .When<int>(x => x > 5).Return(_ => "int")
                .Default(_ => "def")
                .AsDelegate();

            // act & assert
            Assert.That(patternMatcher("aaaa"), Is.EqualTo("str"));
            Assert.That(patternMatcher("vaaaa"), Is.EqualTo("str_cond"));
            Assert.That(patternMatcher(6), Is.EqualTo("int"));
            Assert.That(patternMatcher(0), Is.EqualTo("def"));
            Assert.That(patternMatcher((StringBuilder)null), Is.EqualTo("def"));
        }
    }   
}
