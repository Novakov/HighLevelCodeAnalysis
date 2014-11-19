using System.Reflection;
using CodeModel.Builder;
using CodeModel.Extensions.Cqrs;
using CodeModel.Extensions.Cqrs.Rules;
using CodeModel.Graphs;
using CodeModel.Model;
using CodeModel.Mutators;
using NUnit.Framework;
using TestTarget.Cqrs;
using TestTarget.Rules.InvokeOnlyOneCommand;

namespace Tests.Rules
{
    [TestFixture]
    public class InvokeOnlyOneCommandTest : BaseRuleTest<InvokeOnlyOneCommand>
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
                .Property("Category").EqualTo(InvokeOnlyOneCommand.Category));
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

    public class VerifyPath : BaseRuleTest<Dupa>, IHaveGraph<Node, Link>
    {
        public Graph<Node, Link> Result { get; private set; }

        [Test]
        [TestCase("PathWithThreeMethodsSecondAndThirdExecuteOneCommand")]
        public void ShouldViolate(string entryPointName)
        {
            // arrange
            var builder = new CodeModelBuilder();

            builder.RegisterConventionsFrom(TestTarget.Conventions.Marker.Assembly);

            var entryPoint = typeof (ChainedCommandsTarget).GetMethod(entryPointName);           

            builder.Model.AddNode(new TypeNode(typeof (ChainedCommandsTarget)));
            builder.RunMutator(new AddMethods(BindingFlags.Instance | BindingFlags.NonPublic));            
            builder.Model.AddNode(new MethodNode(entryPoint));
            builder.RunMutator<AddApplicationEntryPoint>();
            builder.RunMutator(new LinkApplicationEntryPointTo<MethodNode>(x => x.Method == entryPoint));
            builder.RunMutator<CountCommandExecutions>();
            builder.RunMutator<LinkMethodCalls>();

            this.Result = builder.Model;

            // act
            this.Verify(builder);

            // assert
            Assert.That(this.VerificationContext.Violations, Has
                .Some
                .Property("Category").EqualTo(Dupa.Category));
        }
    }
}