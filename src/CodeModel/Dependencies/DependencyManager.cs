using System;
using System.Collections.Generic;
using System.Linq;
using CodeModel.Graphs;

namespace CodeModel.Dependencies
{
    public class DependencyManager<TElement>
    {
        private readonly Func<TElement, IEnumerable<string>> provided;
        private readonly Func<TElement, IEnumerable<string>> needed;
        private readonly Func<TElement, IEnumerable<string>> optionalNeeded;

        private readonly Dictionary<TElement, ElementNode> elements;

        private readonly Dictionary<string, TElement> providers;

        private readonly HashSet<TElement> requiredElements;

        private int elementId;

        public DependencyManager(Func<TElement, IEnumerable<string>> provided, Func<TElement, IEnumerable<string>> needed, Func<TElement, IEnumerable<string>> optionalNeeded)
        {
            this.provided = provided;
            this.needed = needed;
            this.optionalNeeded = optionalNeeded;

            this.elements = new Dictionary<TElement, ElementNode>();
            this.providers = new Dictionary<string, TElement>();
            this.elementId = 0;
            this.requiredElements = new HashSet<TElement>();
        }

        public void Add(TElement element)
        {
            this.elements.Add(element, new ElementNode(this.elementId.ToString(), element));
            this.elementId++;

            foreach (var providedResource in this.provided(element))
            {
                this.providers[providedResource] = element;
            }
        }

        public void AddRange(params TElement[] newElements)
        {
            this.AddRange((IEnumerable<TElement>)newElements);
        }

        public void AddRange(IEnumerable<TElement> newElements)
        {
            foreach (var element in newElements)
            {
                this.Add(element);
            }
        }

        public void RequireAllElements()
        {
            this.requiredElements.Clear();
            this.requiredElements.UnionWith(this.elements.Keys);
        }

        public void RequireElements(IEnumerable<TElement> required)
        {
            this.requiredElements.UnionWith(required);
        }

        public void RequireElements(params TElement[] required)
        {
            this.RequireElements((IEnumerable<TElement>)required);
        }

        public RunList<TElement> CalculateRunList()
        {
            var graph = BuildDependencyGraph();

            EliminateNotRequiredElements(graph);

            try
            {
                var sorted = TopologySort.SortGraph(graph);

                return new RunList<TElement>(sorted.Select(x => x.Element));
            }
            catch (CannotSortGraphException e)
            {
                throw new UnableToBuildRunListException(e);
            }
        }

        private Graph<ElementNode, ProvidesResource> BuildDependencyGraph()
        {
            var graph = new Graph<ElementNode, ProvidesResource>();

            foreach (var elementNode in this.elements)
            {
                graph.AddNode(elementNode.Value);
            }

            foreach (var elementNode in this.elements)
            {
                foreach (var neededResource in this.needed(elementNode.Key))
                {
                    if (this.providers.ContainsKey(neededResource))
                    {
                        var provider = this.providers[neededResource];

                        var node = this.elements[provider];

                        graph.AddLink(node, elementNode.Value, new ProvidesResource(true));
                    }
                    else
                    {
                        throw new NeedsNotSatisfiedException(neededResource);
                    }
                }

                foreach (var optionalNeededResource in this.optionalNeeded(elementNode.Key))
                {
                    if (this.providers.ContainsKey(optionalNeededResource))
                    {
                        var provider = this.providers[optionalNeededResource];
                        var node = this.elements[provider];

                        graph.AddLink(node, elementNode.Value, new ProvidesResource(false));
                    }
                }
            }

            return graph;
        }

        private void EliminateNotRequiredElements(Graph<ElementNode, ProvidesResource> graph)
        {
            var walker = new WalkAndAnnotate<ElementNode, ProvidesResource>
            {
                NodeAnnotation = _ => new Mark(),
                AvailableNodes = n => n
                                    .InboundLinks.OfType<ProvidesResource>()
                                    .Where(x => x.IsRequired)
                                    .GroupBy(x => (ElementNode)x.Source)
            };

            foreach (var requiredElement in this.requiredElements)
            {
                walker.Walk(graph, this.elements[requiredElement]);
            }

            var unusedNodes = graph.Nodes.Where(x => !x.HasAnnotation<Mark>()).ToList();

            foreach (var unusedNode in unusedNodes)
            {
                graph.RemoveNode(unusedNode);
            }
        }

        private class ElementNode : Node
        {
            public TElement Element { get; private set; }

            public ElementNode(string nodeId, TElement element)
                : base(nodeId)
            {
                this.Element = element;
            }

            public override string DisplayLabel
            {
                get { return this.Id + ": " + this.Element.ToString(); }
            }
        }

        private class ProvidesResource : Link
        {
            public bool IsRequired { get; private set; }

            public ProvidesResource(bool isRequired)
            {
                IsRequired = isRequired;
            }
        }

        private class Mark
        {
        }
    }

    public class NeedsNotSatisfiedException : Exception
    {
        public string MissingResource { get; private set; }

        public NeedsNotSatisfiedException(string missingResource)
            : base(string.Format("Resource {0} not satisifed", missingResource))
        {
            MissingResource = missingResource;
        }
    }

    public class UnableToBuildRunListException : Exception
    {
        public UnableToBuildRunListException(Exception innerException)
            : base("Unable to build runlist: " + innerException.Message, innerException)
        {

        }
    }
}