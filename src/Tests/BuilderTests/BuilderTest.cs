using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CodeModel.Builder;
using CodeModel.Model;
using CodeModel.Mutators;
using NUnit.Framework;
using TestTarget;

namespace Tests.BuilderTests
{
    [TestFixture]
    public class BuilderTest
    {
        private static readonly Assembly TargetAssembly = typeof(Marker).Assembly;

        [Test]
        public void ShouldAddAssemblies()
        {
            // arrange
            var builder = new CodeModelBuilder();

            // act
            builder.RunMutators(new AddAssemblies(TargetAssembly));

            // assert
            Assert.That(builder.Model.Nodes, Has
                .Exactly(1)
                .InstanceOf<AssemblyNode>()
                .And.Matches<AssemblyNode>(o => o.Assembly == TargetAssembly));
        }

        [Test]
        public void ShouldAddTypes()
        {
            // arrange
            var builder = new CodeModelBuilder();

            // act
            builder.RunMutators(new AddAssemblies(TargetAssembly), new AddTypes());

            // Assert
            Assert.That(builder.Model.Nodes, Has.Some.InstanceOf<TypeNode>());

        }
    }
}
