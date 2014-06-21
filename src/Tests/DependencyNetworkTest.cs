using System;
using System.Linq;
using System.Text;
using CodeModel;
using NUnit.Framework;
using Tests.Constraints;

namespace Tests
{
    [TestFixture]
    public class DependencyNetworkTest
    {
        private DependencyManager<Element> dependencyManager;

        [SetUp]
        public void SetUp()
        {
            this.dependencyManager = new DependencyManager<Element>(x => x.Provide, x => x.Need);
        }

        [Test]
        public void ShouldCalculateValidRunlistFromTwoElements()
        {
            // arrange
            var needAssembly = new Element("", "Assembly");
            var provideAssembly = new Element("Assembly", "");            

            this.dependencyManager.Add(needAssembly);
            this.dependencyManager.Add(provideAssembly);

            // act
            var runList = this.dependencyManager.CalculateRunList();

            // assert
            Assert.That(runList.Elements.ToArray(), Is.EqualTo(new[] {provideAssembly, needAssembly}));
            Assert.That(runList.IsValid, Is.True, "Runlist should be valid");
        }

        [Test]
        public void ShouldCalculateValidRunlistFromFourElementsOneIndependentAndOneDependingOnTwo()
        {
            // arrange
            var independent = new Element("", "");
            var provideAssembly = new Element("Assembly", "");
            var provideType = new Element("Type", "");

            var needAssemblyAndType = new Element("", "Assembly,Type");

            this.dependencyManager.Add(independent);
            this.dependencyManager.Add(provideAssembly);
            this.dependencyManager.Add(provideType);
            this.dependencyManager.Add(needAssemblyAndType);

            // act
            var runList = this.dependencyManager.CalculateRunList();

            // assert
            Assert.That(runList, new RunlistConstraint()
                .IsValid()
                .And.After(needAssemblyAndType, provideAssembly, provideType)         
                );            
        }

        private class Element
        {
            public string[] Provide { get; private set; }

            public string[] Need { get; private set; }

            public Element(string provide, string need)
            {
                this.Provide = provide.Split(new []{','}, StringSplitOptions.RemoveEmptyEntries);
                this.Need = need.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
            }

            public override string ToString()
            {
                return "Provide: " + string.Join(",", this.Provide) + " Need: " + string.Join(",", this.Need);
            }
        }
    }
}
