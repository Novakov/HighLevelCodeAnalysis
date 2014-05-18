using CodeModel.Annotations;
using CodeModel.Builder;
using CodeModel.Model;
using CodeModel.Mutators;
using NUnit.Framework;
using Tests.Constraints;
using TestTarget.Rules.Immutability;

namespace Tests.BuilderTests
{
    [TestFixture]
    public class AnnonateImmutableTypesTest 
    {
        [Test]
        public void ShouldAnnonateImmutableTypes()
        {
            // arrange
            var builder = new CodeModelBuilder();

            builder.RegisterConventionsFrom(TestTarget.Conventions.Marker.Assembly);

            builder.Model.AddNode(new TypeNode(typeof (SetPropertyInCtor)));

            // act
            builder.RunMutator<AnnonateImmutableTypes>();
            

            // assert
            Assert.That(builder.Model, Graph.Has
                .NodeForType<SetPropertyInCtor>(Annonated.With<Immutable>()));
        }
    }
}
