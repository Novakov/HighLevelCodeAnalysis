using CodeModel;
using CodeModel.Builder;
using CodeModel.Extensions.DomainModel.Rules;
using CodeModel.Primitives;
using CodeModel.Primitives.Mutators;
using NUnit.Framework;
using Tests.Rules;
using TestTarget.DomainModel;
using CodeModel.Extensions.DomainModel.Mutators;

namespace Tests.Extensions.DomainModel
{
    [TestFixture]
    public class DoNotCallEntityMethodsFromOutsideOfAggregateTest : BaseRuleTest<DoNotCallEntityMethodsFromOutsideOfAggregateRule>, IHaveBuilder
    {
        public CodeModelBuilder Builder { get; private set; }

        [Test]
        public void ViolationForMethodOutsideOfEntityCallingEntitysMethod()
        {
            // arrange
            Builder = new CodeModelBuilder();
            Builder.RegisterConventionsFrom(TestTarget.Conventions.Marker.Assembly);

            Builder.Model.AddNode(new TypeNode(typeof(OrganizationUnit)));
            Builder.Model.AddNode(new TypeNode(typeof(Person)));
            Builder.Model.AddNode(new TypeNode(typeof(CallEntityMethods)));

            BuildModel();

            // act
            this.Verify(Builder);

            // assert
            var callEntityMethod = Builder.Model.GetNodeForMethod(Get.MethodOf<CallEntityMethods>(x => x.CallEntityMethod()));
            var doSomething = Builder.Model.GetNodeForMethod(Get.MethodOf<Person>(x => x.DoSomething()));

            Assert.That(this.VerificationContext.Violations, Has
                .Exactly(1)
                .InstanceOf<DoNotCallEntityMethodsFromOutsideOfAggregateViolation>()
                .And.Property("Node").EqualTo(callEntityMethod)
                .And.Property("CalledMethod").EqualTo(doSomething));
        }

        [Test]
        public void NoViolationForMethodOutsideOfAggregateCallingAggregateMethod()
        {
            // arrange
            Builder = new CodeModelBuilder();
            Builder.RegisterConventionsFrom(TestTarget.Conventions.Marker.Assembly);

            Builder.Model.AddNode(new TypeNode(typeof(OrganizationUnit)));
            Builder.Model.AddNode(new TypeNode(typeof(Person)));
            Builder.Model.AddNode(new TypeNode(typeof(CallEntityMethods)));

            BuildModel();

            // act
            this.Verify(Builder);

            // assert
            var callAggregateMethod = Builder.Model.GetNodeForMethod(Get.MethodOf<CallEntityMethods>(x => x.CallAggregateMethod()));
            var letManagerKnow = Builder.Model.GetNodeForMethod(Get.MethodOf<OrganizationUnit>(x => x.LetManagerKnow()));

            Assert.That(this.VerificationContext.Violations, Has
                .None
                .InstanceOf<DoNotCallEntityMethodsFromOutsideOfAggregateViolation>()
                .And.Property("Node").EqualTo(callAggregateMethod)
                .And.Property("CalledMethod").EqualTo(letManagerKnow));
        }

        [Test]
        public void NoViolationForAggregateMethodCallingContainedEntityMethod()
        {
            // arrange
            Builder = new CodeModelBuilder();
            Builder.RegisterConventionsFrom(TestTarget.Conventions.Marker.Assembly);

            Builder.Model.AddNode(new TypeNode(typeof(OrganizationUnit)));
            Builder.Model.AddNode(new TypeNode(typeof(Person)));

            BuildModel();

            // act
            this.Verify(Builder);

            // assert
            var letManagerKnow = Builder.Model.GetNodeForMethod(Get.MethodOf<OrganizationUnit>(x => x.LetManagerKnow()));
            var doSomething = Builder.Model.GetNodeForMethod(Get.MethodOf<Person>(x => x.DoSomething()));

            Assert.That(this.VerificationContext.Violations, Has
                .None
                .InstanceOf<DoNotCallEntityMethodsFromOutsideOfAggregateViolation>()
                .And.Property("Node").EqualTo(letManagerKnow)
                .And.Property("CalledMethod").EqualTo(doSomething));
        }

        [Test]
        public void NoViolationForEntityMethodCallingMethodOnItSelf()
        {
            // arrange
            Builder = new CodeModelBuilder();
            Builder.RegisterConventionsFrom(TestTarget.Conventions.Marker.Assembly);

            Builder.Model.AddNode(new TypeNode(typeof(OrganizationUnit)));
            Builder.Model.AddNode(new TypeNode(typeof(Person)));

            BuildModel();

            // act
            this.Verify(Builder);

            // assert          
            var doSomething = Builder.Model.GetNodeForMethod(Get.MethodOf<Person>(x => x.DoSomething()));
            var callSomething = Builder.Model.GetNodeForMethod(Get.MethodOf<Person>(x => x.CallSomething()));

            Assert.That(this.VerificationContext.Violations, Has
                .None
                .InstanceOf<DoNotCallEntityMethodsFromOutsideOfAggregateViolation>()
                .And.Property("Node").EqualTo(callSomething)
                .And.Property("CalledMethod").EqualTo(doSomething));
        }

        [Test]
        public void NoViolationForEntityMethodCallingMethodOnContainedEntity()
        {
            // arrange
            Builder = new CodeModelBuilder();
            Builder.RegisterConventionsFrom(TestTarget.Conventions.Marker.Assembly);

            Builder.Model.AddNode(new TypeNode(typeof(OrganizationUnit)));
            Builder.Model.AddNode(new TypeNode(typeof(Person)));

            BuildModel();

            // act
            this.Verify(Builder);

            // assert          
            var doSomething = Builder.Model.GetNodeForMethod(Get.MethodOf<SomeEntity>(x => x.DoSomethingOnSomeEntity()));
            var callSomething = Builder.Model.GetNodeForMethod(Get.MethodOf<Person>(x => x.CallSomethingOnSomeEntity()));

            Assert.That(this.VerificationContext.Violations, Has
                .None
                .InstanceOf<DoNotCallEntityMethodsFromOutsideOfAggregateViolation>()
                .And.Property("Node").EqualTo(callSomething)
                .And.Property("CalledMethod").EqualTo(doSomething));
        }

        [Test]
        public void ViolationForAggregateMethodCallingMethodOnEntityInAnotherAggregate()
        {
            // arrange
            Builder = new CodeModelBuilder();
            Builder.RegisterConventionsFrom(TestTarget.Conventions.Marker.Assembly);

            Builder.Model.AddNode(new TypeNode(typeof(OrganizationUnit)));
            Builder.Model.AddNode(new TypeNode(typeof(Person)));
            Builder.Model.AddNode(new TypeNode(typeof(OtherAggregate)));
            Builder.Model.AddNode(new TypeNode(typeof(SomeOtherEntity)));

            BuildModel();

            // act
            this.Verify(Builder);

            // assert          
            var doSomethingOnEntityInAggregate = Builder.Model.GetNodeForMethod(Get.MethodOf<OrganizationUnit>(x => x.DoSomethingOnEntityInAggregate(null)));
            var doSomething = Builder.Model.GetNodeForMethod(Get.MethodOf<SomeOtherEntity>(x => x.DoSomething()));

            Assert.That(this.VerificationContext.Violations, Has
                .Exactly(1)
                .InstanceOf<DoNotCallEntityMethodsFromOutsideOfAggregateViolation>()
                .And.Property("Node").EqualTo(doSomethingOnEntityInAggregate)
                .And.Property("CalledMethod").EqualTo(doSomething));            
        }

        private void BuildModel()
        {
            Builder.RunMutator<AddMethods>();
            Builder.RunMutator<AddProperties>();
            Builder.RunMutator<LinkMethodCalls>();
            Builder.RunMutator<LinkPropertyAccess>();
            Builder.RunMutator<LinkToContainer>();
            Builder.RunMutator<DetectEntities>();
            Builder.RunMutator<DetectAggregates>();
            Builder.RunMutator<LinkContainedEntities>();
        }
    }
}