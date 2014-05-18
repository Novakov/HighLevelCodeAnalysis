using System;
using System.Reflection;
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
        [Test]
        [TestCase(typeof(SetFieldOutsideOfCtor), TypeIsImmutable.SettingFieldOutsideOfConstructor)]
        [TestCase(typeof(SetPropertyOutsideOfCtor), TypeIsImmutable.SettingPropertyOutsideOfConstructor)]
        [TestCase(typeof(PublicPropertySetter), TypeIsImmutable.NonPrivatePropertySetter)]
        [TestCase(typeof(WriteableField), TypeIsImmutable.WritableField)]
        public void ShouldViolate(Type type, string category)
        {
            // arrange
            var builder = BuildCodeModel(type);

            // act            
            Verify(builder);

            // assert
            Assert.That(this.VerificationContext.Violations, Has
                .Exactly(1)
                .Property("Category").EqualTo(category)                
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
