using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeModel;
using CodeModel.Builder;
using CodeModel.Extensions.AspNetMvc;
using CodeModel.Extensions.AspNetMvc.Mutators;
using CodeModel.Primitives.Mutators;
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

        [Test]
        public void ShouldRecognizeActions()
        {
            // arrange
            Builder.RunMutator(new AddAssemblies(typeof(Marker).Assembly));
            Builder.RunMutator<AddTypes>();
            Builder.RunMutator<AddMethods>();
            Builder.RunMutator<LinkToContainer>();
            Builder.RunMutator<DetectControllers>();

            // act            
            Builder.RunMutator<DetectActions>();

            // assert
            var actionNode = Builder.Model.GetNodeForMethod(Get.MethodOf<MyController1>(x => x.MyAction()));
            Assert.That(actionNode, Is.InstanceOf<ActionNode>());
        }

        [Test]
        public void ShouldNotRecognizeActionsInNotControllers()
        {
            // arrange
            Builder.RunMutator(new AddAssemblies(typeof(Marker).Assembly));
            Builder.RunMutator<AddTypes>();
            Builder.RunMutator<AddMethods>();
            Builder.RunMutator<LinkToContainer>();
            Builder.RunMutator<DetectControllers>();

            // act            
            Builder.RunMutator<DetectActions>();

            // assert
            var actionNode = Builder.Model.GetNodeForMethod(Get.MethodOf<NotAController>(x => x.MyAction()));
            Assert.That(actionNode, Is.Not.InstanceOf<ActionNode>());
        }
    }
}
