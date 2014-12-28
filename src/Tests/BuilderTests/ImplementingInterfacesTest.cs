using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeModel;
using CodeModel.Builder;
using CodeModel.Primitives;
using CodeModel.Primitives.Mutators;
using NUnit.Framework;
using Tests.Constraints;
using TestTarget.Implementing;

namespace Tests.BuilderTests
{
    [TestFixture]
    public class ImplementingInterfacesTest : IHaveBuilder
    {
        public CodeModelBuilder Builder { get; private set; }

        [SetUp]
        public void SetUp()
        {
            this.Builder = new CodeModelBuilder();            
            this.Builder.RegisterConventionsFrom(typeof(TestTarget.Conventions.Marker).Assembly);
        }

        [Test]
        public void ShouldConnectTypeToInterfacesThatItImplements()
        {
            // arrange
            this.Builder.RunMutator(new AddAssemblies(typeof(TestTarget.Marker).Assembly));
            this.Builder.RunMutator<AddTypes>();
            
            // act
            this.Builder.RunMutator<LinkTypesToImplementedInterfaces>();

            // assert
            var implementingType = this.Builder.Model.GetNodeForType(typeof (ImplementingType));
            var interfaceType = this.Builder.Model.GetNodeForType(typeof (IInterface));

            Assert.That(this.Builder.Model, Graph.Has
                .Links<ImplementsLink>(exactly: 1, from: implementingType, to: interfaceType));
        }

        [Test]
        public void ShouldReplaceMethodFromInterfaceWithMethodFromImplementingType()
        {
            // arrange
            this.Builder.RunMutator(new AddAssemblies(typeof(TestTarget.Marker).Assembly));
            this.Builder.RunMutator<AddTypes>();
            this.Builder.RunMutator<AddMethods>();
            this.Builder.RunMutator<LinkMethodCalls>();
            this.Builder.RunMutator<LinkTypesToImplementedInterfaces>();
            this.Builder.RunMutator<LinkToContainer>();

            // act
            this.Builder.RunMutator<ReplaceInterfaceWithImplementation>();

            // assert       
            var callSource = this.Builder.Model.GetNodeForMethod(Get.MethodOf<CallSource>(x => x.Call(null)));
            var implicitMethod = this.Builder.Model.GetNodeForMethod(Get.MethodOf<ImplementingType>(x => x.ImplicitMethod()));

            Assert.That(this.Builder.Model, Graph.Has
                .Links<MethodCallLink>(exactly: 1, from: callSource, to: implicitMethod));

            var interfaceNode = this.Builder.Model.GetNodeForType(typeof (IInterface));
            Assert.That(interfaceNode, Is.Null, "Interface node not replaced");
        }
    }
}
