using System;
using System.Linq;
using System.Text;
using CodeModel;
using CodeModel.Dependencies;
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
            this.dependencyManager = new DependencyManager<Element>(x => x.Provide, x => x.Need, x => x.OptionalNeed);
        }

        [Test]
        public void ShouldCalculateValidRunlistFromTwoElements()
        {
            // arrange
            var needAssembly = new Element("", "Assembly");
            var provideAssembly = new Element("Assembly", "");

            this.dependencyManager.Add(needAssembly);
            this.dependencyManager.Add(provideAssembly);

            this.dependencyManager.RequireAllElements();

            // act
            var runList = this.dependencyManager.CalculateRunList();

            // assert
            Assert.That(runList.Elements.ToArray(), Is.EqualTo(new[] { provideAssembly, needAssembly }));  
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

            this.dependencyManager.RequireAllElements();

            // act
            var runList = this.dependencyManager.CalculateRunList();

            // assert
            Assert.That(runList, new RunlistConstraint()
                .After(needAssemblyAndType, provideAssembly, provideType)
                );
        }

        [Test]
        public void ShouldCalculateNotValidRunListWhenDependenciesCannotBeSatisfied()
        {
            // arrange
            var provideAssembly = new Element("Assembly", "");
            var needType = new Element("", "Type");

            this.dependencyManager.Add(provideAssembly);
            this.dependencyManager.Add(needType);

            this.dependencyManager.RequireAllElements();

            // act & assert
            Assert.That(()=>this.dependencyManager.CalculateRunList(), Throws
                .InstanceOf<NeedsNotSatisfiedException>()
                .And.Property("MissingResource").EqualTo("Type")
            );            
        }

        [Test]
        public void ShouldCalculateNotValidRunListWhenDependenciesHaveCycleAndOneStartingNode()
        {
            // arrange
            var provideStartup = new Element("Startup", "");
            var provideAssemblyAndNeedType = new Element("Assembly", "Type,Startup");
            var provideTypeAndNeedAssembly = new Element("Type", "Assembly,Startup");

            var needTypeAndAssembly = new Element("", "Assembly,Type");

            this.dependencyManager.Add(provideStartup);
            this.dependencyManager.Add(provideAssemblyAndNeedType);
            this.dependencyManager.Add(provideTypeAndNeedAssembly);
            this.dependencyManager.Add(needTypeAndAssembly);

            this.dependencyManager.RequireAllElements();

            // act & assert
            Assert.That(() => this.dependencyManager.CalculateRunList(), Throws
                .InstanceOf<UnableToBuildRunListException>()
                );            
        }

        [Test]
        public void ShouldBuildRunListWithOnlyElementsRequiredToRunRequiredElements()
        {
            // arrange
            var provideA = new Element("A", "");
            var needA = new Element("", "A");

            var provideB = new Element("B", "");
            var provideCneedB = new Element("C", "B");

            var needC = new Element("", "C");

            this.dependencyManager.Add(provideA);
            this.dependencyManager.Add(needA);
            this.dependencyManager.Add(provideB);
            this.dependencyManager.Add(provideCneedB);
            this.dependencyManager.Add(needC);

            this.dependencyManager.RequireElements(needC);

            // act
            var runList = this.dependencyManager.CalculateRunList();

            // assert
            Assert.That(runList, new RunlistConstraint()
                .Not.Contains(provideA)
                .And.Not.Contains(needA));
        }

        [Test]
        public void ShouldBuildRunListWhenSomeDependenciesAreOptional()
        {
            // arrange
            var provideTypes = new Element("Types", "");
            var provideMethods = new Element("Methods", "");
            var provideCommands = new Element("Commands", "");

            var linking = new Element("Links", "", "Types,Methods,Commands");

            var useLinking = new Element("", "Types,Methods,Links");

            this.dependencyManager.AddRange(provideTypes, provideMethods, provideCommands, linking, useLinking);

            this.dependencyManager.RequireElements(useLinking);

            // act
            var runList = this.dependencyManager.CalculateRunList();

            // assert
            Assert.That(runList, new RunlistConstraint()
                .Contains(provideTypes)
                .And.Contains(provideMethods)
                .And.Contains(linking)
                .And.Contains(useLinking)
                .And.Not.Contains(provideCommands)
                .And.After(linking, provideTypes, provideMethods)
            );
        }

        private class Element
        {
            public string[] Provide { get; private set; }

            public string[] Need { get; private set; }

            public string[] OptionalNeed { get; private set; }

            public Element(string provide, string need, string optionalNeed = "")
            {
                this.Provide = provide.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                this.Need = need.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                this.OptionalNeed = optionalNeed.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }

            public override string ToString()
            {
                return "Provide: " + string.Join(",", this.Provide) + " Need: " + string.Join(",", this.Need);
            }
        }
    }
}
