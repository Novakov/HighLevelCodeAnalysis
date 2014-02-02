using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CodeModel;
using CodeModel.Builder;
using CodeModel.FlowAnalysis;
using CodeModel.Links;
using CodeModel.Model;
using CodeModel.Mutators;
using NUnit.Framework;
using Tests.Constraints;
using TestTarget;

namespace Tests.BuilderTests
{
    [TestFixture]
    public class LinkCallsTests : IHaveBuilder
    {
        private static readonly Assembly TargetAssembly = typeof(Marker).Assembly;

        public CodeModelBuilder Builder { get; private set; }

        [SetUp]
        public void Setup()
        {
            this.Builder = new CodeModelBuilder();
        }

        [Test]
        public void Basic()
        {
            // arrange
            Builder.RunMutator(new AddAssemblies(TargetAssembly));
            Builder.RunMutator<AddTypes>();
            Builder.RunMutator(new RemoveNode<TypeNode>(x => x.Type != typeof(LinkCalls)));
            Builder.RunMutator<AddMethods>();

            // act      
            Builder.RunMutator<LinkMethodCalls>();

            // assert            
            var source = Builder.Model.GetNodeForMethod(typeof(LinkCalls).GetMethod("Source"));
            var normalCall = Builder.Model.GetNodeForMethod(typeof(LinkCalls).GetMethod("NormalCall"));
            var genericMethodCall = Builder.Model.GetNodeForMethod(typeof(LinkCalls).GetMethod("GenericMethodCall"));

            Assert.That(Builder.Model, Graph.Has
                .Links<MethodCallLink>(exactly: 1, from: source, to: normalCall)
                .Links<MethodCallLink>(exactly: 1, from: source, to: genericMethodCall, matches: x => x.GenericMethodArguments.Length == 1 && x.GenericMethodArguments[0] == typeof(int)));
        }

        [Test]
        public void CheckDetermineParametersForSingleBranchMethod()
        {
            // arrange
            Builder.RunMutator(new AddAssemblies(TargetAssembly));
            Builder.RunMutator<AddTypes>();
            Builder.RunMutator(new RemoveNode<TypeNode>(x => x.Type != typeof(LinkCalls)));
            Builder.RunMutator<AddMethods>();

            // act      
            Builder.RunMutator<LinkMethodCalls>();

            // assert            
            var source = Builder.Model.GetNodeForMethod(Get.MethodOf<LinkCalls>(x => x.IndirectParameterTypesInSingleBranch()));

            var call = source.OutboundLinks.OfType<MethodCallLink>().Single();

            Assert.That(call.ActualParameterTypes.Single(), Is.EqualTo(new[] {PotentialType.FromType(typeof (Inherited))}));
        }

        [Test]
        public void CheckDetermineTheSameParametersInMultipleBranches()
        {
            // arrange
            Builder.RunMutator(new AddAssemblies(TargetAssembly));
            Builder.RunMutator<AddTypes>();
            Builder.RunMutator(new RemoveNode<TypeNode>(x => x.Type != typeof(LinkCalls)));
            Builder.RunMutator<AddMethods>();

            // act      
            Builder.RunMutator<LinkMethodCalls>();

            // assert            
            var source = Builder.Model.GetNodeForMethod(Get.MethodOf<LinkCalls>(x => x.IndirectParameterTypesInMultipleBranches()));

            var call = source.OutboundLinks.OfType<MethodCallLink>().Single();

            Assert.That(call.ActualParameterTypes.Single(), Is.EqualTo(new[] { PotentialType.FromType(typeof(Inherited)) }));
        }

        [Test]
        public void CheckDetermineDifferentParametersInMultipleBranches()
        {
            // arrange
            Builder.RunMutator(new AddAssemblies(TargetAssembly));
            Builder.RunMutator<AddTypes>();
            Builder.RunMutator(new RemoveNode<TypeNode>(x => x.Type != typeof(LinkCalls)));
            Builder.RunMutator<AddMethods>();

            // act      
            Builder.RunMutator<LinkMethodCalls>();

            // assert            
            var source = Builder.Model.GetNodeForMethod(Get.MethodOf<LinkCalls>(x => x.IndirectDifferentParameterTypesInMultipleBranches()));

            var call = source.OutboundLinks.OfType<MethodCallLink>().Single();

            Assert.That(call.ActualParameterTypes, Has
                .Exactly(1).EqualTo(new[] { PotentialType.FromType(typeof(Inherited)) })
                .And.Exactly(1).EqualTo(new[] { PotentialType.FromType(typeof(SecondInherited)) }));
        }

        [Test]
        public void CheckDetermineTheSameParametersInMultipleCallsInSingleBranch()
        {
            // arrange
            Builder.RunMutator(new AddAssemblies(TargetAssembly));
            Builder.RunMutator<AddTypes>();
            Builder.RunMutator(new RemoveNode<TypeNode>(x => x.Type != typeof(LinkCalls)));
            Builder.RunMutator<AddMethods>();

            // act      
            Builder.RunMutator<LinkMethodCalls>();

            // assert            
            var source = Builder.Model.GetNodeForMethod(Get.MethodOf<LinkCalls>(x => x.MultipleCallsWithTheSameParameters()));

            var call = source.OutboundLinks.OfType<MethodCallLink>().Single();

            Assert.That(call.ActualParameterTypes.Single(), Is.EqualTo(new[] { PotentialType.FromType(typeof(Inherited)) }));
        }

        [Test]
        public void CheckDetermineTheDifferentParametersInMultipleCallsInSingleBranch()
        {
            // arrange
            Builder.RunMutator(new AddAssemblies(TargetAssembly));
            Builder.RunMutator<AddTypes>();
            Builder.RunMutator(new RemoveNode<TypeNode>(x => x.Type != typeof(LinkCalls)));
            Builder.RunMutator<AddMethods>();

            // act      
            Builder.RunMutator<LinkMethodCalls>();

            // assert            
            var source = Builder.Model.GetNodeForMethod(Get.MethodOf<LinkCalls>(x => x.MultipleCallsWithDifferentParameters()));

            var call = source.OutboundLinks.OfType<MethodCallLink>().Single();

            Assert.That(call.ActualParameterTypes, Has
                .Exactly(1).EqualTo(new[] { PotentialType.FromType(typeof(Inherited)) })
                .And.Exactly(1).EqualTo(new[] { PotentialType.FromType(typeof(SecondInherited)) })
                );
        }
    }
}
