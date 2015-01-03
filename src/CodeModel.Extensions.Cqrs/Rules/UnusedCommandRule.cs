using System.Collections.Generic;
using System.Linq;
using CodeModel.Dependencies;
using CodeModel.Graphs;
using CodeModel.Primitives;
using CodeModel.RuleEngine;

namespace CodeModel.Extensions.Cqrs.Rules
{
    [Need(CqrsResources.CommandExecutionLinks, CqrsResources.Commands, Resources.EntryPoint, Resources.LinksToEntryPoints, Resources.MethodCallLinks)]
    public class UnusedCommandRule : IGraphRule
    {
        public IEnumerable<Violation> Verify(VerificationContext context, Graph graph)
        {
            var entryPoint = graph.LookupNode<ApplicationEntryPoint>(ApplicationEntryPoint.NodeId);

            var unusedCommands = new HashSet<CommandNode>(graph.Nodes.OfType<CommandNode>());

            var bfs = new LambdaBreadthFirstSearch<Node, Link>
            {
                HandlingNode = (node, links) =>
                {
                    var commandNode = node as CommandNode;
                    if (commandNode != null && links.OfType<ExecuteCommandLink>().Any())
                    {
                        unusedCommands.Remove(commandNode);
                    }
                }
            };

            bfs.Walk(graph, entryPoint);

            foreach (var unusedCommand in unusedCommands)
            {
                yield return new UnusedCommandViolation(unusedCommand);
            }
        }
    }
}