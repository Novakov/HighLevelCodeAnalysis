using System;
using System.Linq;
using System.Reflection;
using CodeModel;
using CodeModel.Builder;
using CodeModel.Model;
using CodeModel.Mutators;
using CodeModel.Rules;
using NUnit.Framework;
using TestTarget.Rules.Immutability;

namespace Tests.Rules
{
    [TestFixture]
    public class ImmutabilityTest : BaseRuleTest<TypeIsImmutable>
    {
        private const BindingFlags All = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        [Test]        
        public void ShouldReportViolationWhenSettingFieldOutsideOfConsturctor()
        {
            // arrange
            var builder = BuildCodeModel(typeof(SetFieldOutsideOfCtor));

            // act            
            Verify(builder);

            // assert
            Assert.That(this.VerificationContext.Violations, Has.Count.EqualTo(2));
            Assert.That(this.VerificationContext.Violations.OfType<ImmutableTypeSetsFieldOutsideOfConstructorViolation>().ElementAt(0), Is
                .InstanceOf<ImmutableTypeSetsFieldOutsideOfConstructorViolation>()
                .And.Property("Node").EqualTo(builder.Model.GetNodeForType(typeof(SetFieldOutsideOfCtor)))
                .And.Property("ViolatingMethod").EqualTo(builder.Model.GetNodeForMethod(typeof(SetFieldOutsideOfCtor).GetMethod("SetField")))
                );

        }

        [Test]
        public void ShouldReportViolationWhenSettingPropertyOutsideOfConstructor()
        {
            // arrange
            var builder = BuildCodeModel(typeof (SetPropertyOutsideOfCtor));

            // act
            Verify(builder);

            // assert
            Assert.That(this.VerificationContext.Violations, Has.Count.EqualTo(1));
            Assert.That(this.VerificationContext.Violations.ElementAt(0), Is
                .InstanceOf<ImmutableTypeSetsPropertyOutsideOfConstructorViolation>()
                .And.Property("Node").EqualTo(builder.Model.GetNodeForType(typeof(SetPropertyOutsideOfCtor)))
                .And.Property("ViolatingMethod").EqualTo(builder.Model.GetNodeForMethod(typeof(SetPropertyOutsideOfCtor).GetMethod("SetProperty")))
                );
        }

        [Test]
        public void ShouldReportViolationOnPublicPropertySetter()
        {
            // arrange
            var builder = BuildCodeModel(typeof (PublicPropertySetter));

            // act
            Verify(builder);

            // assert
            Assert.That(this.VerificationContext.Violations, Has.Count.EqualTo(1));
            Assert.That(this.VerificationContext.Violations.ElementAt(0), Is
                .InstanceOf<ImmutableTypeHasNonPrivateSetterViolation>()
                .And.Property("Node").EqualTo(builder.Model.GetNodeForType(typeof(PublicPropertySetter)))
                .And.Property("ViolatingProperty").EqualTo(builder.Model.GetNodeForProperty(typeof(PublicPropertySetter).GetProperty("PublicSetter")))
                );
        }

        [Test]
        public void ShouldReportViolationOnWriteableField()
        {
            // arrange
            var builder = BuildCodeModel(typeof (WriteableField));

            // act
            Verify(builder);

            // assert
            Assert.That(this.VerificationContext.Violations, Has.Count.EqualTo(1));
            Assert.That(this.VerificationContext.Violations.ElementAt(0), Is
                .InstanceOf<ImmutableTypeHasWritableFieldViolation>()
                .And.Property("Node").EqualTo(builder.Model.GetNodeForType(typeof (WriteableField)))
                .And.Property("ViolatingField").EqualTo(builder.Model.GetNodeForField(typeof(WriteableField).GetField("field", All)))
                );
        }

        [Test]
        [TestCase(typeof(SetFieldInCtor))]
        [TestCase(typeof(SetPropertyInCtor))]
        [TestCase(typeof(ReadonlyField))]
        public void ShouldNotViolate(Type type)
        {
            // arrange
            var builder = BuildCodeModel(type);
                        
            // act
            this.Verify(builder);

            // assert
            Assert.That(this.VerificationContext.Violations, Is.Empty);
        }

        private CodeModelBuilder BuildCodeModel(Type type)
        {
            var builder = new CodeModelBuilder();

            builder.RegisterConventionsFrom(TestTarget.Conventions.Marker.Assembly);

            builder.Model.AddNode(new TypeNode(type));

            builder.RunMutator(new AddMethods(AddMethods.DefaultFlags | BindingFlags.NonPublic));
            builder.RunMutator<AddFields>();
            builder.RunMutator<AddProperties>();
            builder.RunMutator<LinkToContainer>();
            builder.RunMutator<LinkFieldAccess>();
            builder.RunMutator<LinkMethodCalls>();
            builder.RunMutator<LinkPropertyAccess>();
            builder.RunMutator<AnnonateImmutableTypes>();

            return builder;
        }
    }
}
