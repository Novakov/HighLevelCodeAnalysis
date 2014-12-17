﻿using System;
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

            this.dependencyManager.RequireAllElements();

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

            this.dependencyManager.RequireAllElements();

            // act
            var runList = this.dependencyManager.CalculateRunList();

            // assert
            Assert.That(runList, new RunlistConstraint()
                .IsValid()
                .And.After(needAssemblyAndType, provideAssembly, provideType)         
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

            // act
            var runList = this.dependencyManager.CalculateRunList();

            // assert
            Assert.That(runList, new RunlistConstraint().IsNotValid()
                .And.HasMissing("Type"));
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

            // act
            var runList = this.dependencyManager.CalculateRunList();

            // assert
            Assert.That(runList, new RunlistConstraint()
                .IsNotValid());
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
                .IsValid()
                .And.Not.Contains(provideA)
                .And.Not.Contains(needA));
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
