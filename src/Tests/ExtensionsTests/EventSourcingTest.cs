using System.Reflection;
using CodeModel;
using CodeModel.Builder;
using CodeModel.Extensions.EventSourcing.Links;
using CodeModel.Extensions.EventSourcing.Mutators;
using CodeModel.Extensions.EventSourcing.Nodes;
using CodeModel.Mutators;
using NUnit.Framework;
using Tests.Constraints;
using TestTarget;
using TestTarget.EventSourcing;

namespace Tests.ExtensionsTests
{
    [TestFixture]
    public class EventSourcingTest : IHaveBuilder
    {
        public CodeModelBuilder Builder { get; private set; }

        [SetUp]
        public void SetUp()
        {
            this.Builder = new CodeModelBuilder();
        }

        [Test]
        public void ShouldConvertMethodCallLinkToApplyEventLink()
        {
            // arrange
            Builder.RegisterConventionsFrom(typeof(TestTarget.Conventions.Marker).Assembly);

            Builder.RunMutator(new AddAssemblies(typeof(Marker).Assembly));
            Builder.RunMutator<AddTypes>();
            Builder.RunMutator(new AddMethods(AddMethods.DefaultFlags | BindingFlags.NonPublic));
            Builder.RunMutator<DetectEntities>();      
            Builder.RunMutator<LinkMethodCalls>();

            // act
            Builder.RunMutator<DetectApplyEvent>();

            // assert
            var methodNode = Builder.Model.GetNodeForMethod(typeof (Person).GetMethod("ChangeSurname"));
            var eventNode = Builder.Model.GetNodeForType(typeof (SurnameChanged));
            
            Assert.That(Builder.Model, Graph.Has
                .Links<ApplyEventLink>(exactly: 1, from: methodNode, to: eventNode));
        }

        [Test]
        public void ShouldDetectEventApplyMethods()
        {
            // arrange
            Builder.RegisterConventionsFrom(typeof(TestTarget.Conventions.Marker).Assembly);

            Builder.RunMutator(new AddAssemblies(typeof(Marker).Assembly));
            Builder.RunMutator<AddTypes>();
            Builder.RunMutator(new AddMethods(AddMethods.DefaultFlags | BindingFlags.NonPublic));            

            // act            
            Builder.RunMutator<DetectApplyEventMethods>();

            // assert
            var methodNode = Builder.Model.GetNodeForMethod(typeof (Person).GetMethod("On", BindingFlags.Instance | BindingFlags.NonPublic, null, new[] {typeof (SurnameChanged)}, null));

            Assert.That(methodNode, Is.InstanceOf<ApplyEventMethod>());
            Assert.That(((ApplyEventMethod)methodNode).AppliedEventType, Is.EqualTo(typeof(SurnameChanged)));            
        }
    }
}
