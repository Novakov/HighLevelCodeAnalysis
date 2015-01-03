using CodeModel;
using CodeModel.Builder;
using CodeModel.Extensions.Cqrs.Mutators;
using CodeModel.Extensions.Cqrs.Rules;
using CodeModel.Primitives;
using CodeModel.Primitives.Mutators;
using CodeModel.RuleEngine;
using NUnit.Framework;
using TestTarget.Cqrs;
using TestTarget.Rules.UnusedCommands;

namespace Tests.Rules
{
    [TestFixture]
    public class UnusedCommandRuleTest : BaseRuleTest<UnusedCommandRule>
    {
        [Test]
        public void ShouldReportViolationForUnusedCommand()
        {
            // arrange
            var model = BuildModel("CallOnlyOneCommand");

            // act
            this.Verify(model);

            // assert
            Assert.That(this.VerificationContext.Violations, Has
                .Exactly(1)
                .InstanceOf<UnusedCommandViolation>()
                .And.Property("Node").EqualTo(model.Model.GetNodeForType(typeof(UnregisterUser))));
        }

        [Test]
        public void ShouldNotReportViolationWhenAllCommandsAreUsed()
        {
            // arrange
            var model = BuildModel("CallTwoCommands");

            // act
            this.Verify(model);

            // assert
            Assert.That(this.VerificationContext.Violations, Is.Empty);
        }

        private static CodeModelBuilder BuildModel(string entryPointName)
        {
            var model = new CodeModelBuilder();

            model.RegisterConventionsFrom(TestTarget.Conventions.Marker.Assembly);

            model.Model.AddNode(new TypeNode(typeof (Targets)));
            model.Model.AddNode(new TypeNode(typeof (RegisterUser)));
            model.Model.AddNode(new TypeNode(typeof (UnregisterUser)));
            model.Model.AddNode(new TypeNode(typeof (ICommandDispatcher)));

            model.RunMutator<AddMethods>();
            model.RunMutator<AddApplicationEntryPoint>();
            model.RunMutator(new LinkApplicationEntryPointTo<MethodNode>(m => m.Method.Name == entryPointName));

            model.RunMutator<LinkMethodCalls>();
            model.RunMutator<DetectCommands>();
            model.RunMutator<LinkCommandExecutions>();
            return model;
        }
    }
}