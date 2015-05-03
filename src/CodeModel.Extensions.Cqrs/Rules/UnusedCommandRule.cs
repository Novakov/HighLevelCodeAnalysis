using System.Collections.Generic;
using System.Linq;
using CodeModel.Dependencies;
using CodeModel.Graphs;
using CodeModel.Primitives;
using CodeModel.RuleEngine;

namespace CodeModel.Extensions.Cqrs.Rules
{
    [Need(CqrsResources.CommandExecutionLinks, CqrsResources.Commands, Resources.EntryPoint, Resources.LinksToEntryPoints, Resources.MethodCallLinks)]
    public class UnusedCommandRule : IRuleWithBootstrap, INodeRule
    {
        private HashSet<CommandNode> unusedCommands;

        public void Initialize(VerificationContext context, Graph graph)
        {
            var entryPoint = graph.LookupNode<ApplicationEntryPoint>(ApplicationEntryPoint.NodeId);

            this.unusedCommands = new HashSet<CommandNode>(graph.Nodes.OfType<CommandNode>());

            var bfs = new LambdaBreadthFirstSearch<Node, Link>
            {
                HandlingNode = (node, links) =>
                {
                    var commandNode = node as CommandNode;
                    if (commandNode != null && links.OfType<ExecuteCommandLink>().Any())
                    {
                        this.unusedCommands.Remove(commandNode);
                    }
                }
            };

            bfs.Walk(entryPoint);
        }     

        public IEnumerable<Violation> Verify(VerificationContext context, Node node)
        {
            if(this.unusedCommands.Contains((CommandNode)node))
            {
                yield return new UnusedCommandViolation((CommandNode) node);
            }
        }

        public bool IsApplicableTo(Node node)
        {
            return node is CommandNode;
        }
    }
}