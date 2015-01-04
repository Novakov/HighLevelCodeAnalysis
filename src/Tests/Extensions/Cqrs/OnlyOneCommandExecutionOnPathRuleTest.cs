using System.Reflection;
using CodeModel.Builder;
using CodeModel.Extensions.Cqrs.Mutators;
using CodeModel.Extensions.Cqrs.Rules;
using CodeModel.Graphs;
using CodeModel.Primitives;
using CodeModel.Primitives.Mutators;
using NUnit.Framework;
using Tests.Rules;
using TestTarget.Rules.InvokeOnlyOneCommand;

namespace Tests.Extensions.Cqrs
{
    public class OnlyOneCommandExecutionOnPathRuleTest : BaseRuleTest<OnlyOneCommandExecutionOnPathRule>, IHaveGraph<Node, Link>
    {
        public Graph<Node, Link> Result { get; private set; }

        [Test]
        [TestCase("PathWithThreeMethodsSecondAndThirdExecuteOneCommand")]
        [TestCase("TwoBranchesOneWithSingleCommandSecondWithTwoCommands")]
        [TestCase("OneMethodCallsTwoMethodsEachExecuteOneCommand")]
        public void ShouldViolate(string entryPointName)
        {
            // arrange
            var builder = PrepareModel(entryPointName);

            this.Result = builder.Model;

            // act
            this.Verify(builder);

            // assert
            Assert.That(this.VerificationContext.Violations, Has
                .Some.InstanceOf<MethodCanLeadToExecutionOfMoreThanOneCommandViolation>()
                );
        }

        [Test]
        [TestCase("TwoBranchesEachWithOneCommand")]
        [Ignore]
        public void ShouldNotViolate(string entryPointName)
        {
            // arrange
            var builder = PrepareModel(entryPointName);

            this.Result = builder.Model;

            // act
            this.Verify(builder);

            // assert
            Assert.That(this.VerificationContext.Violations, Is.Empty);
        }

        private static CodeModelBuilder PrepareModel(string entryPointName)
        {
            var builder = new CodeModelBuilder();

            builder.RegisterConventionsFrom(TestTarget.Conventions.Marker.Assembly);

            var entryPoint = typeof(ChainedCommandsTarget).GetMethod(entryPointName);

            builder.Model.AddNode(new TypeNode(typeof(ChainedCommandsTarget)));
            builder.RunMutator(new AddMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public));
            builder.RunMutator<AddApplicationEntryPoint>();
            builder.RunMutator(new LinkApplicationEntryPointTo<MethodNode>(x => x.Method == entryPoint));
            builder.RunMutator<CountCommandExecutions>();
            builder.RunMutator<LinkMethodCalls>();
            return builder;
        }
    }
}