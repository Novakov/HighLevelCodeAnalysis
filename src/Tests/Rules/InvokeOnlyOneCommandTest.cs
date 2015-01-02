using CodeModel.Builder;
using CodeModel.Extensions.Cqrs;
using CodeModel.Extensions.Cqrs.Mutators;
using CodeModel.Extensions.Cqrs.Rules;
using CodeModel.Primitives;
using CodeModel.Primitives.Mutators;
using NUnit.Framework;
using TestTarget.Cqrs;
using TestTarget.Rules.InvokeOnlyOneCommand;

namespace Tests.Rules
{
    [TestFixture]
    public class InvokeOnlyOneCommandTest : BaseRuleTest<InvokeOnlyOneCommandInMethod>
    {
        [SetUp]
        public void SetUp()
        {
            this.Verificator.RegisterConventionsFrom(TestTarget.Conventions.Marker.Assembly);
        }

        [Test]
        [TestCase("DispatchTwoCommands")]
        public void ShouldViolate(string methodName)
        {
            // arrange
            var model = CreateModel(methodName);

            // act
            this.Verify(model);

            // assert
            Assert.That(this.VerificationContext.Violations, Has.Exactly(1)
                .InstanceOf<MethodExecutesMoreThanOneCommandViolation>()
                );
        }

        [Test]
        [TestCase("DispatchOneCommandInSingleBranch")]
        [TestCase("NoCommandDispatch")]
        [TestCase("DispatchTwoCommandOnePerBranch")]
        public void ShouldNotViolate(string methodName)
        {
            // arrange
            var model = CreateModel(methodName);

            // act
            this.Verify(model);

            // assert
            Assert.That(this.VerificationContext.Violations, Is.Empty);
        }


        private static CodeModelBuilder CreateModel(string methodName)
        {
            var model = new CodeModelBuilder();
            model.Model.AddNode(new MethodNode(typeof(Targets).GetMethod(methodName)));
            model.Model.AddNode(new MethodNode(Get.MethodOf<ICommandDispatcher>(x => x.Execute(null))));
            model.Model.AddNode(new TypeNode(typeof(RegisterUser)));
            model.Model.AddNode(new TypeNode(typeof(UnregisterUser)));

            model.RegisterConventionsFrom(TestTarget.Conventions.Marker.Assembly);

            model.RunMutator<DetectCommands>();

            model.RunMutator<LinkMethodCalls>();

            model.RunMutator<LinkCommandExecutions>();

            model.RunMutator<CountCommandExecutions>();

            return model;
        }
    }
}