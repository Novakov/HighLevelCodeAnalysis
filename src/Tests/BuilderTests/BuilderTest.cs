using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CodeModel;
using CodeModel.Builder;
using CodeModel.Graphs;
using CodeModel.Links;
using CodeModel.Model;
using CodeModel.Mutators;
using NUnit.Framework;
using TestTarget;
using TestTarget.EventSourcing;

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
            builder.RunMutator(new AddAssemblies(TargetAssembly));

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
            builder.RunMutator(new AddAssemblies(TargetAssembly));
            builder.RunMutator<AddTypes>();

            // assert
            Assert.That(builder.Model.Nodes, Has.Some.InstanceOf<TypeNode>());
        }

        [Test]
        public void ShouldRemoveNodesMatchingCondition()
        {
            // arrange
            var builder = new CodeModelBuilder();
            builder.RunMutator(new AddAssemblies(TargetAssembly));
            builder.RunMutator<AddTypes>();

            // act
            builder.RunMutator(new RemoveNode<TypeNode>(x => x.Type.Name == "ToBeRemovedFromGraph"));

            // assert
            Assert.That(builder.Model.Nodes, Has.None.Matches<object>(n => n is TypeNode && ((TypeNode)n).Type.Name == "ToBeRemovedFromGraph"));
        }

        [Test]
        public void ShouldRecognizeEntity()
        {
            // arrange
            var builder = new CodeModelBuilder();

            builder.RegisterConventionsFrom(typeof(TestTarget.Conventions.Marker).Assembly);

            builder.RunMutator(new AddAssemblies(TargetAssembly));
            builder.RunMutator<AddTypes>();

            // act
            builder.RunMutator<DetectEntities>();

            // assert
            var entityNode = builder.Model.GetNodeForType(typeof(Person));
            Assert.That(entityNode, Is.InstanceOf<EntityNode>());
        }

        [Test]
        public void ShouldAddMethods()
        {
            // arrange
            var builder = new CodeModelBuilder();

            builder.RunMutator(new AddAssemblies(TargetAssembly));
            builder.RunMutator<AddTypes>();

            // act
            builder.RunMutator<AddMethods>();

            // assert            
            Assert.That(builder.Model.Nodes, Has.Some.InstanceOf<MethodNode>());
        }

        [Test]
        public void ShouldLinkCalls()
        {
            // arrange
            var builder = new CodeModelBuilder();

            builder.RunMutator(new AddAssemblies(TargetAssembly));
            builder.RunMutator<AddTypes>();
            builder.RunMutator<AddMethods>();

            // act      
            builder.RunMutator<LinkMethodCalls>();

            // assert            
            var source = builder.Model.GetNodeForMethod(typeof(LinkCalls).GetMethod("Source"));
            var normalCall = builder.Model.GetNodeForMethod(typeof(LinkCalls).GetMethod("NormalCall"));
            var genericMethodCall = builder.Model.GetNodeForMethod(typeof(LinkCalls).GetMethod("GenericMethodCall"));

            Assert.That(normalCall.InboundLinks, Has.Exactly(1)
                .InstanceOf<MethodCallLink>()
                .And.Matches<MethodCallLink>(x => x.Source.Equals(source)),
                "Normal method call not linked");

            var genericMethodCallLink = genericMethodCall.InboundLinks.OfType<MethodCallLink>().SingleOrDefault();

            Assert.That(genericMethodCallLink, Is.Not.Null, "Generic method call linked");
            Assert.That(genericMethodCallLink.Source, Is.SameAs(source));
            Assert.That(genericMethodCallLink.GenericMethodArguments, Is.EqualTo(new[] { typeof(int) }), "Generic method arguments mismatch");           
        }        
    }
}
