using System;
using System.Collections.Generic;
using System.Linq;
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
        [Test]
        public void ShouldAddAssemblies()
        {
            // arrange
            var builder = new CodeModelBuilder();

            // act
            builder.RunMutators(new AddAssemblies(typeof(Marker).Assembly));

            // assert
            Assert.That(builder.Model.Nodes, Has
                .Exactly(1)
                .InstanceOf<AssemblyNode>()
                .And.Matches<AssemblyNode>(o => o.Assembly == typeof (Marker).Assembly));
        }
    }
}
