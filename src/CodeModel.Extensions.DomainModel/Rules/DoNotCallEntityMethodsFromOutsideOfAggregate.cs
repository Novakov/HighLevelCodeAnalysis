using System;
using System.Collections.Generic;
using System.Linq;
using CodeModel.Graphs;
using CodeModel.Primitives;
using CodeModel.RuleEngine;

namespace CodeModel.Extensions.DomainModel.Rules
{
    public class DoNotCallEntityMethodsFromOutsideOfAggregate : INodeRule, IRuleWithBootstrap
    {
        private Dictionary<MethodNode, AggregateNode> aggregateForMethod;

        public void Initialize(VerificationContext context, Graph graph)
        {
            this.aggregateForMethod = new Dictionary<MethodNode, AggregateNode>();

            var bfs = new LambdaBreadthFirstSearch<Node, Link>
            {
                AvailableTargets = n => n.OutboundLinks
                    .Where(l => l is HasOneEntityLink || l is HasManyEntityLink)
                    .GroupBy(x => x.Target)
                    .Union(n.InboundLinks.Where(l => l is ContainedInLink && l.Target is EntityNode && l.Source is MethodNode).GroupBy(x => x.Source))
            };

            var aggregates = graph.Nodes.OfType<AggregateNode>();

            foreach (var node in aggregates)
            {
                bfs.HandlingNode = (n, _) =>
                {
                    var methodNode = n as MethodNode;
                    if (methodNode != null && !methodNode.Method.IsSpecialName)
                    {
                        this.aggregateForMethod[methodNode] = node;
                    }
                };

                bfs.Walk(node);
            }
        }

        public bool IsApplicableTo(Node node)
        {
            return node is MethodNode;
        }

        public IEnumerable<Violation> Verify(VerificationContext context, Node node)
        {
            var calledMethods = node.OutboundLinks.OfType<MethodCallLink>().Select(x => (MethodNode)x.Target);

            foreach (var calledMethod in calledMethods)
            {
                if (IsInvalidCall((MethodNode)node, calledMethod))
                {
                    yield return new DoNotCallEntityMethodsFromOutsideOfAggregateViolation((MethodNode)node, calledMethod);
                }
            }
        }

        private bool IsInvalidCall(MethodNode callingMethod, MethodNode calledMethod)
        {
            var aggregateOfCalledMethod = this.aggregateForMethod.GetOrDefault(calledMethod);
            var aggregateOfCallingMethod = this.aggregateForMethod.GetOrDefault(callingMethod);

            return PatternMatch.For<Tuple<AggregateNode, AggregateNode>, bool>()
                .When(x => x.Item1 == x.Item2).Return(_ => false)
                .When(x => x.Item1 == null && x.Item2 == null).Return(_ => false)
                .When(x => x.Item1 != null && x.Item2 == null).Return(_ => false)
                .When(x => x.Item1 == null && x.Item2 != null).Return(t => calledMethod.GetContainer() != aggregateOfCalledMethod)
                .Default(_ => true)
                .Match(Tuple.Create(aggregateOfCallingMethod, aggregateOfCalledMethod));
        }
    }
}