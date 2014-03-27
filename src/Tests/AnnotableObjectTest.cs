using CodeModel;
using CodeModel.Graphs;
using NUnit.Framework;

namespace Tests
{
    [TestFixture(TypeArgs = new[] { typeof(AnnotableTestSampleNode)})]
    [TestFixture(TypeArgs = new[] { typeof(AnnotableTestSampleLink)})]
    public class AnnotableObjectTest<TTarget>
        where TTarget : IAnnotable, new()
    {
        private IAnnotable target;

        [SetUp]
        public void Setup()
        {
            this.target = new TTarget();
        }

        [Test]
        public void ShouldAddAnnotation()
        {
            // arrange
            var annotation = "Annotation";

            // act
            this.target.Annonate(annotation);

            // assert
            Assert.That(this.target.Annotations, Has.Member(annotation));
        }

        [Test]
        public void ShouldRemoveAnnotation()
        {
            // arrange
            var annotation = "Annotation";
            this.target.Annonate(annotation);

            // act     
            this.target.RemoveAnnotation(annotation);

            // assert
            Assert.That(this.target.Annotations, Has.No.Member(annotation));
        }

        [Test]
        public void ShouldGetAnnotationByType()
        {
            // arrange
            var annotation = "Annotation";
            this.target.Annonate(annotation);

            // act     
            var actual = this.target.Annotation<string>();

            // assert
            Assert.That(actual, Is.EqualTo(annotation));
        }

        [Test]
        public void ShouldReturnNullWhenNoAnnotation()
        {
            // arrange                        

            // act     
            var actual = this.target.Annotation<string>();

            // assert
            Assert.That(actual, Is.Null);
        }        
    }

    internal class AnnotableTestSampleNode : Node
    {
        public AnnotableTestSampleNode()
            : base("Node")
        {
        }
    }

    internal class AnnotableTestSampleLink : Link
    {
    }
}
