﻿using System.Reflection;
using CodeModel;
using CodeModel.Builder;
using CodeModel.Extensions.Cqrs;
using CodeModel.Extensions.EventSourcing.Mutators;
using CodeModel.Extensions.EventSourcing.Nodes;
using CodeModel.Mutators;
using NUnit.Framework;
using Tests.Constraints;
using TestTarget;
using TestTarget.Cqrs;

namespace Tests.ExtensionsTests
{
    public class CqrsTest : IHaveBuilder
    {
        public CodeModelBuilder Builder { get; private set; }

        [SetUp]
        public void Setup()
        {
            this.Builder = new CodeModelBuilder();
            Builder.RegisterConventionsFrom(typeof(TestTarget.Conventions.Marker).Assembly);
        }

        [Test]
        public void ShouldRecognizeQueryTypes()
        {
            // arrange            
            Builder.RunMutator(new AddAssemblies(typeof(Marker).Assembly));
            Builder.RunMutator<AddTypes>();

            // act
            Builder.RunMutator<DetectQueries>();

            // assert

            Assert.That(Builder.Model, Graph.Has
                .NodeForType<GetUser>(Is.InstanceOf<QueryNode>()));
        }

        [Test]
        public void ShouldLinkQueryExecutions()
        {
            // arrange            
            Builder.RunMutator(new AddAssemblies(typeof(Marker).Assembly));
            Builder.RunMutator<AddTypes>();
            Builder.RunMutator<AddMethods>();
            Builder.RunMutator<LinkMethodCalls>();
            Builder.RunMutator<DetectQueries>();

            // act
            Builder.RunMutator<LinkQueryExecutions>();

            // assert
            var origin = Builder.Model.GetNodeForMethod(Get.MethodOf<CallOrigin>(x => x.CallQuery()));
            var query = Builder.Model.GetNodeForType(typeof(GetUser));

            Assert.That(Builder.Model, Graph.Has
                .Links<QueryExecutionLink>(exactly: 1, from: origin, to: query));
        }

        [Test]
        public void ShouldDetectCommandHandlerMethods()
        {
            // arrange
            Builder.RunMutator(new AddAssemblies(typeof(Marker).Assembly));
            Builder.RunMutator<AddTypes>();
            Builder.RunMutator<AddMethods>();

            // act
            Builder.RunMutator<DetectCommandHandlers>();

            // assert
            var handlerMethod = Builder.Model.GetNodeForMethod(Get.MethodOf<CommandHandlers>(x => x.Execute((RegisterUser)null)));
            Assert.That(handlerMethod, Is.InstanceOf<CommandHandlerNode>()
                .And.Property("HandledCommand").EqualTo(typeof(RegisterUser)));
        }

        [Test]
        public void ShouldDetectCommands()
        {
            // arrange
            Builder.RunMutator(new AddAssemblies(typeof(Marker).Assembly));
            Builder.RunMutator<AddTypes>();

            // act
            Builder.RunMutator<DetectCommands>();

            // assert           
            Assert.That(Builder.Model, Graph.Has
                .NodeForType<RegisterUser>(Is.InstanceOf<CommandNode>()));
        }

        [Test]
        public void ShouldLinkCommandsToHandlers()
        {
            // arrange
            Builder.RunMutator(new AddAssemblies(typeof(Marker).Assembly));
            Builder.RunMutator<AddTypes>();
            Builder.RunMutator<AddMethods>();
            Builder.RunMutator<DetectCommandHandlers>();
            Builder.RunMutator<DetectCommands>();

            // act
            Builder.RunMutator<LinkCommandsToHandlers>();

            // assert
            var handlerMethod = Builder.Model.GetNodeForMethod(Get.MethodOf<CommandHandlers>(x => x.Execute((RegisterUser)null)));
            var commandNode = Builder.Model.GetNodeForType(typeof(RegisterUser));
            Assert.That(Builder.Model, Graph.Has
                .Links<ExecutedByLink>(exactly: 1, from: commandNode, to: handlerMethod));
        }
    }
}
