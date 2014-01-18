using System.Reflection;
using CodeModel;
using CodeModel.Builder;
using CodeModel.Extensions.EventSourcing.Links;
using CodeModel.Extensions.EventSourcing.Mutators;
using CodeModel.Extensions.EventSourcing.Nodes;
using CodeModel.Mutators;
using NUnit.Framework;
using TestTarget;
using TestTarget.EventSourcing;

namespace Tests.ExtensionsTests
{
    [TestFixture]
    public class EventSourcingTest
    {
        [Test]
        public void ShouldConvertMethodCallLinkToApplyEventLink()
        {
            // arrange
            var builder = new CodeModelBuilder();

            builder.RegisterConventionsFrom(typeof(TestTarget.Conventions.Marker).Assembly);

            builder.RunMutator(new AddAssemblies(typeof(Marker).Assembly));
            builder.RunMutator<AddTypes>();
            builder.RunMutator(new AddMethods(AddMethods.DefaultFlags | BindingFlags.NonPublic));
            builder.RunMutator<DetectEntities>();      
            builder.RunMutator<LinkMethodCalls>();

            // act
            builder.RunMutator<DetectApplyEvent>();

            // assert
            var methodNode = builder.Model.GetNodeForMethod(typeof (Person).GetMethod("ChangeSurname"));
            var eventNode = builder.Model.GetNodeForType(typeof (SurnameChanged));

            Assert.That(methodNode.OutboundLinks, Has
                .Exactly(1).InstanceOf<ApplyEventLink>()
                .And.Matches<ApplyEventLink>(x => x.Target.Equals(eventNode)));
        }

        [Test]
        public void ShouldDetectEventApplyMethods()
        {
            // arrange
            var builder = new CodeModelBuilder();

            builder.RegisterConventionsFrom(typeof(TestTarget.Conventions.Marker).Assembly);

            builder.RunMutator(new AddAssemblies(typeof(Marker).Assembly));
            builder.RunMutator<AddTypes>();
            builder.RunMutator(new AddMethods(AddMethods.DefaultFlags | BindingFlags.NonPublic));            

            // act            
            builder.RunMutator<DetectApplyEventMethods>();

            // assert
            var methodNode = builder.Model.GetNodeForMethod(typeof (Person).GetMethod("On", BindingFlags.Instance | BindingFlags.NonPublic, null, new[] {typeof (SurnameChanged)}, null));

            Assert.That(methodNode, Is.InstanceOf<ApplyEventMethod>());
            Assert.That(((ApplyEventMethod)methodNode).AppliedEventType, Is.EqualTo(typeof(SurnameChanged)));
        }
    }
}
