using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeModel.Builder;
using CodeModel.Extensions.AspNetMvc;
using CodeModel.Mutators;
using NUnit.Framework;
using Tests.Constraints;
using TestTarget;
using TestTarget.AspNetMvc;

namespace Tests.ExtensionsTests
{
    [TestFixture]
    public class AspNetMvcTest : IHaveBuilder
    {
        public CodeModelBuilder Builder { get; private set; }

        [SetUp]
        public void Setup()
        {
            this.Builder = new CodeModelBuilder();
            Builder.RegisterConventionsFrom(typeof(TestTarget.Conventions.Marker).Assembly);
        }

        [Test]
        public void ShouldRecognizeControllers()
        {
            // arrange
            Builder.RunMutator(new AddAssemblies(typeof(Marker).Assembly));
            Builder.RunMutator<AddTypes>();

            // act
            Builder.RunMutator<DetectControllers>();

            // assert
            Assert.That(Builder.Model, Graph.Has
                .NodeForType<MyController1>(Is.InstanceOf<ControllerNode>()));
        }
    }
}
