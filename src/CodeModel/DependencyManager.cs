using System;
using System.Collections.Generic;
using System.Linq;
using CodeModel.Graphs;

namespace CodeModel
{
    public class DependencyManager<TElement>
    {
        private readonly Func<TElement, IEnumerable<string>> provided;
        private readonly Func<TElement, IEnumerable<string>> needed;

        private readonly Dictionary<TElement, ElementNode> elements;

        private readonly Dictionary<string, TElement> providers;

        private int elementId;

        public DependencyManager(Func<TElement, IEnumerable<string>> provided, Func<TElement, IEnumerable<string>> needed)
        {
            this.provided = provided;
            this.needed = needed;
            this.elements = new Dictionary<TElement, ElementNode>();
            this.providers = new Dictionary<string, TElement>();
            this.elementId = 0;
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

        public RunList<TElement> CalculateRunList()
        {
            var graph = new Graph<ElementNode, Link>();

            foreach (var elementNode in this.elements)
            {
                graph.AddNode(elementNode.Value);
            }

            foreach (var elementNode in this.elements)
            {
                foreach (var neededResource in this.needed(elementNode.Key))
                {
                    var provider = this.providers[neededResource];

                    var node = this.elements[provider];

                    graph.AddLink(node, elementNode.Value, new ProvidesResource());
                }
            }

            var sorted = TopologySort.SortGraph(graph);

            return new RunList<TElement>(sorted.Select(x => x.Element), new string[0]);
        }

        private class ElementNode : Node
        {
            public TElement Element { get; private set; }            

            public ElementNode(string nodeId, TElement element)
                : base(nodeId)
            {
                this.Element = element;
            }
        }

        private class ProvidesResource : Link
        {
        }
    }
}