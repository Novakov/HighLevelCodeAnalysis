using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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
    public class ImmutabilityTest
    {
        [Test]
        [TestCase(typeof(SetFieldOutsideOfCtor), TypeIsImmutable.SettingFieldOutsideOfConstructor)]
        [TestCase(typeof(SetPropertyOutsideOfCtor), TypeIsImmutable.SettingPropertyOutsideOfConstructor)]
        [TestCase(typeof(PublicPropertySetter), TypeIsImmutable.NonPrivatePropertySetter)]
        [TestCase(typeof(WriteableField), TypeIsImmutable.WritableField)]
        public void ShouldViolate(Type type, string category)
        {
            // arrange
            var builder = new CodeModelBuilder();
            var typeNode = builder.Model.AddNode(new TypeNode(type));

            builder.RunMutator(new AddMethods(AddMethods.DefaultFlags | BindingFlags.NonPublic));
            builder.RunMutator<AddFields>();
            builder.RunMutator<AddProperties>();
            builder.RunMutator<LinkToContainer>();
            builder.RunMutator<LinkFieldAccess>();
            builder.RunMutator<LinkMethodCalls>();
            builder.RunMutator<LinkPropertyAccess>();

            var rule = new TypeIsImmutable();

            var ctx = new VerificationContext();

            // act
            rule.Verify(ctx, typeNode);

            // assert
            Assert.That(ctx.Violations, Has
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
            var builder = new CodeModelBuilder();
            var typeNode = builder.Model.AddNode(new TypeNode(type));

            builder.RunMutator<AddMethods>();
            builder.RunMutator<AddFields>();
            builder.RunMutator<LinkToContainer>();
            builder.RunMutator<LinkFieldAccess>();

            var rule = new TypeIsImmutable();

            var ctx = new VerificationContext();

            // act
            rule.Verify(ctx, typeNode);

            // assert
            Assert.That(ctx.Violations, Is.Empty);
        }
    }
}
