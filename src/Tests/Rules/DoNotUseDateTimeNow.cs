using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeModel.Builder;
using CodeModel.Model;
using CodeModel.Rules;
using NUnit.Framework;
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
			Assert.That(this.VerificationContext.Violations, Has
                .Exactly(1).Property("Category").EqualTo(DoNotUseDateTimeNow.UsesDateTimeNow));
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
