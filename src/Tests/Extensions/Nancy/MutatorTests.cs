using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using CodeModel.Builder;
using CodeModel.Extensions.Nancy;
using CodeModel.Extensions.Nancy.Mutators;
using CodeModel.MonadicParser;
using CodeModel.Primitives;
using CodeModel.Primitives.Mutators;
using Mono.Reflection;
using NUnit.Framework;
using Tests.Constraints;
using TestTarget.NancyLib;

namespace Tests.Extensions.Nancy
{
    public class MutatorTests : IHaveBuilder
    {
        public CodeModelBuilder Builder { get; set; }

        [SetUp]
        public void SetUp()
        {
            this.Builder = new CodeModelBuilder();
        }

        [Test]
        public void ShouldDetectNancyModules()
        {
            // arrange
            this.Builder.Model.AddNode(new TypeNode(typeof(MyModule)));

            // act
            this.Builder.RunMutator<DetectNancyModules>();

            // assert
            Assert.That(this.Builder.Model, Graph.Has
                .Nodes<NancyModuleNode>(exactly: 1, matches: x => x.Type == typeof(MyModule))
                );
        }

        [Test]
        public void ShouldDetectRoutesInModuleConstructor()
        {
            // arrange
            this.Builder.Model.AddNode(new TypeNode(typeof(MyModule)));
            foreach (var nestedType in typeof(MyModule).GetNestedTypes(BindingFlags.NonPublic))
            {
                this.Builder.Model.AddNode(new TypeNode(nestedType));
            }
            this.Builder.RunMutator<DetectNancyModules>();
            this.Builder.RunMutator<AddMethods>();

            // act
            this.Builder.RunMutator<DetectNancyRoutes>();

            // assert
            Assert.That(this.Builder.Model, Graph.Has
                .Nodes<NancyRouteNode>(exactly: 1, matches: x => x.Path == "/bare_string" && x.BuilderName == "Get")
                .Nodes<NancyRouteNode>(exactly: 1, matches: x => x.Path == "/use_this" && x.BuilderName == "Post")
                .Nodes<NancyRouteNode>(exactly: 1, matches: x => x.Path == "/closure_no_this" && x.BuilderName == "Get")
                .Nodes<NancyRouteNode>(exactly: 1, matches: x => x.Path == "/closure_with_this" && x.BuilderName == "Get")
                );
        }
    }
}
