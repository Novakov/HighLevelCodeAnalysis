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

        [Violation(DisplayText = "Display text is composed of {MethodName}")]
        public class SampleViolation : Violation
        {
            public string MethodName { get; private set; }

            public string DisplayText
            {
                get
                {
                    var s = "";

                    return s;
                }
            }

            public SampleViolation(string methodName)
            {
                MethodName = methodName;
            }
        }
    }
}