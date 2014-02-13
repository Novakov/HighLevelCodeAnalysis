using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using CodeModel;
using CodeModel.Builder;
using CodeModel.Links;
using CodeModel.Model;
using CodeModel.Mutators;
using NUnit.Framework;
using TestTarget;
using TestTarget.EventSourcing;
using Graph = Tests.Constraints.Graph;

namespace Tests.BuilderTests
{
    [TestFixture]
    public class BuilderTest : IHaveBuilder
    {
        private static readonly Assembly TargetAssembly = typeof(Marker).Assembly;

        public CodeModelBuilder Builder { get; private set; }

        [SetUp]
        public void Setup()
        {
            this.Builder = new CodeModelBuilder();
        }

        [Test]
        public void ShouldAddAssemblies()
        {
            // arrange            

            // act
            Builder.RunMutator(new AddAssemblies(TargetAssembly));

            // assert          
            Assert.That(Builder.Model, Graph.Has
                .Nodes<AssemblyNode>(exactly: 1, matches: x => x.Assembly == TargetAssembly));
        }

        [Test]
        public void ShouldAddTypes()
        {
            // arrange

            // act
            Builder.RunMutator(new AddAssemblies(TargetAssembly));
            Builder.RunMutator<AddTypes>();

            // assert
            Assert.That(Builder.Model, Graph.Has.Nodes<TypeNode>());
        }

        [Test]
        public void ShouldRemoveNodesMatchingCondition()
        {
            // arrange            
            Builder.RunMutator(new AddAssemblies(TargetAssembly));
            Builder.RunMutator<AddTypes>();

            // act
            Builder.RunMutator(new RemoveNode<TypeNode>(x => x.Type.Name == "ToBeRemovedFromGraph"));

            // assert
            Assert.That(Builder.Model, Graph.Has
                .Nodes<TypeNode>(exactly: 0, matches: n => n.Type.Name == "ToBeRemovedFromGraph"));
        }

        [Test]
        public void ShouldRecognizeEntity()
        {
            // arrange            
            Builder.RegisterConventionsFrom(typeof(TestTarget.Conventions.Marker).Assembly);

            Builder.RunMutator(new AddAssemblies(TargetAssembly));
            Builder.RunMutator<AddTypes>();

            // act
            Builder.RunMutator<DetectEntities>();

            // assert            
            Assert.That(Builder.Model, Graph.Has
                .NodeForType<Person>(Is.InstanceOf<EntityNode>()));
        }

        [Test]
        public void ShouldAddMethods()
        {
            // arrange            
            Builder.RunMutator(new AddAssemblies(TargetAssembly));
            Builder.RunMutator<AddTypes>();

            // act
            Builder.RunMutator<AddMethods>();

            // assert            
            Assert.That(Builder.Model, Graph.Has.Nodes<MethodNode>());
        }

        [Test]
        public void ShouldNotAddInheritedMethods()
        {
            // arrange
            Builder.RunMutator(new AddAssemblies(TargetAssembly));
            Builder.RunMutator<AddTypes>();
            Builder.RunMutator(new RemoveNode<TypeNode>(x => x.Type != typeof(Inherited)));

            // act            
            Builder.RunMutator<AddMethods>();

            // assert
            Assert.That(Builder.Model, Graph.Has
                .Nodes<MethodNode>(exactly: 0, matches: x => x.Method.Name == "Method"));
        }

        [Test]
        public void ShouldAddProperties()
        {
            // arrange
            Builder.RunMutator(new AddAssemblies(TargetAssembly));
            Builder.RunMutator<AddTypes>();

            // act
            Builder.RunMutator<AddProperties>();

            // assert
            Assert.That(Builder.Model, Graph.Has
                .Nodes<PropertyNode>());
        }

        [Test]
        public void ShouldAddFields()
        {
            // arrange
            Builder.RunMutator(new AddAssemblies(TargetAssembly));
            Builder.RunMutator<AddTypes>();

            // act
            Builder.RunMutator<AddFields>();

            // assert
            Assert.That(Builder.Model, Graph.Has
                .Nodes<FieldNode>());
        }

        [Test]
        public void ShouldNotAddPropertyBackingFields()
        {
            // arrange
            Builder.RunMutator(new AddAssemblies(TargetAssembly));
            Builder.RunMutator<AddTypes>();

            // act
            Builder.RunMutator<AddFields>();

            // assert
            Assert.That(Builder.Model, Graph.Has
                .Nodes<FieldNode>(exactly: 0, matches: x => x.Field.GetCustomAttribute<CompilerGeneratedAttribute>() != null));
        }

        [Test]
        public void ShouldLinkFieldAccess()
        {
            // arrange
            Builder.RunMutator(new AddAssemblies(TargetAssembly));
            Builder.RunMutator<AddTypes>();
            Builder.RunMutator<AddMethods>();
            Builder.RunMutator<AddFields>();

            // act
            Builder.RunMutator<LinkFieldAccess>();

            // assert
            var fieldNode = Builder.Model.GetNodeForField(Get.FieldOf<MemberAccess>(x => x.ThisField));
            Assert.That(Builder.Model, Graph.Has
                .Links<SetFieldLink>(to: fieldNode)
                .Links<GetFieldLink>(to: fieldNode)
                );
        }

        [Test]
        public void ShouldLinkPropertyAccess()
        {
            // arrange
            Builder.RunMutator(new AddAssemblies(TargetAssembly));
            Builder.RunMutator<AddTypes>();
            Builder.RunMutator<AddMethods>();
            Builder.RunMutator<AddProperties>();

            // act
            Builder.RunMutator<LinkPropertyAccess>();

            // assert
            var propertyNode = Builder.Model.GetNodeForProperty(Get.PropertyOf<MemberAccess>(x => x.ThisProperty));
            Assert.That(Builder.Model, Graph.Has
                .Links<SetPropertyLink>(to: propertyNode)
                .Links<GetPropertyLink>(to: propertyNode)
                );
        }

        [Test]
        public void ShouldLinkMemberToTypeAndTypeToAssembly()
        {
            // arrange
            Builder.RunMutator(new AddAssemblies(TargetAssembly));
            Builder.RunMutator<AddTypes>();
            Builder.RunMutator<AddMethods>();
            Builder.RunMutator<AddProperties>();
            Builder.RunMutator<AddFields>();

            // act
            Builder.RunMutator<LinkToContainer>();

            // assert
            var fieldNode = Builder.Model.GetNodeForField(Get.FieldOf<MemberAccess>(x => x.ThisField));
            var propertyNode = Builder.Model.GetNodeForProperty(Get.PropertyOf<MemberAccess>(x => x.ThisProperty));
            var methodNode = Builder.Model.GetNodeForMethod(Get.MethodOf<MemberAccess>(x => x.Access()));
            var typeNode = Builder.Model.GetNodeForType(typeof (MemberAccess));
            var assemblyNode = Builder.Model.Nodes.OfType<AssemblyNode>().Single();

            Assert.That(Builder.Model, Graph.Has
                .Links<ContainedInLink>(from:fieldNode, to:typeNode)
                .Links<ContainedInLink>(from:propertyNode, to:typeNode)
                .Links<ContainedInLink>(from:methodNode, to:typeNode)
                .Links<ContainedInLink>(from:typeNode, to:assemblyNode)
                );
        }
    }
}
