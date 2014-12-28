using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeModel.Builder;
using CodeModel.Primitives;
using CodeModel.Rules;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using TestTarget.Rules.DateTimeNow;

namespace Tests.Rules
{
	[TestFixture]
    public class DoNotUseDateTimeNowTest : BaseRuleTest<DoNotUseDateTimeNow>
	{
		[Test]
        [TestCase("UseDateTimeNow")]
        [TestCase("UseDateTimeUtcNow")]
        [TestCase("UseDateTimeOffsetNow")]
        [TestCase("UseDateTimeOffsetUtcNow")]        
	    public void ShouldViolate(string methodName)
	    {
	        // arrange
		    var builder = new CodeModelBuilder();
            builder.Model.AddNode(new MethodNode(typeof(Targets).GetMethod(methodName)));

			// act
			this.Verify(builder);

			// assert
			Assert.That(this.VerificationContext.Violations, Has.Count.EqualTo(1));

            Assert.That(this.VerificationContext.Violations.ElementAt(0), Is                
                .InstanceOf<UsesDateTimeNowViolation>()                
            );
	    }

		[Test]
	    public void ShouldRecordSourceLocation()
	    {
            // arrange
            var builder = new CodeModelBuilder();
            builder.Model.AddNode(new MethodNode(typeof(Targets).GetMethod("UseDateTimeNow")));

            // act
            this.Verify(builder);

            // assert
		    var violation = this.VerificationContext.Violations.First();

            Assert.That(((UsesDateTimeNowViolation)violation).SourceLocation, Is.Not.Null);
			Assert.That(((UsesDateTimeNowViolation)violation).SourceLocation.Value.FileName, Is.StringEnding(@"TestTarget\Rules\DateTimeNow\Targets.cs"));
			Assert.That(((UsesDateTimeNowViolation)violation).SourceLocation.Value.StartLine, Is.EqualTo(13), "Start line mismatch");
			Assert.That(((UsesDateTimeNowViolation)violation).SourceLocation.Value.EndLine, Is.EqualTo(13), "End line mismatch");
            Assert.That(((UsesDateTimeNowViolation)violation).SourceLocation.Value.StartColumn, Is.EqualTo(13), "Start column mismatch");
            Assert.That(((UsesDateTimeNowViolation)violation).SourceLocation.Value.EndColumn, Is.EqualTo(45), "End column mismatch");
	    }


        [Test]
        public void ShouldNotViolate()
        {
            // arrange
            var builder = new CodeModelBuilder();
            builder.Model.AddNode(new MethodNode(typeof(Targets).GetMethod("DoNotUseDateTimeNow")));

            // act
            this.Verify(builder);

            // assert
            Assert.That(this.VerificationContext.Violations, Is.Empty);
        }
    }
}
