﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CodeModel;
using CodeModel.Builder;
using CodeModel.Links;
using CodeModel.Model;
using CodeModel.Mutators;
using NUnit.Framework;
using Tests.Constraints;
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
            Assert.That(builder.Model, Graph.Has
                .Nodes<AssemblyNode>(exactly: 1, matches: x => x.Assembly == TargetAssembly));
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
            Assert.That(builder.Model, Graph.Has.Nodes<TypeNode>());
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
            Assert.That(builder.Model, Graph.Has
                .Nodes<TypeNode>(exactly: 0, matches: n => n.Type.Name == "ToBeRemovedFromGraph"));
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
            Assert.That(builder.Model, Graph.Has
                .NodeForType<Person>(Is.InstanceOf<EntityNode>()));
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
            Assert.That(builder.Model, Graph.Has.Nodes<MethodNode>());
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
         
            Assert.That(builder.Model, Graph.Has
                .Links<MethodCallLink>(exactly: 1, from: source, to: normalCall)
                .Links<MethodCallLink>(exactly: 1, from: source, to: genericMethodCall, matches: x => x.GenericMethodArguments.Length == 1 && x.GenericMethodArguments[0] == typeof (int)));
        }        
    }
}
