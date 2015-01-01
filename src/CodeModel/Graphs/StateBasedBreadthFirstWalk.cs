using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeModel.Graphs
{    
    public class StateBasedBreadthFirstWalk<TState, TNode, TLink>
        where TState : IEquatable<TState> 
        where TNode : Node 
        where TLink : Link
    {
        private IDictionary<TNode, WalkState> startPoints;
        private HashSet<TState> resultStates;
        private HashSet<TLink> visitedLinks;
        
        public Func<TLink, bool> ShouldWalkOnlyOnce { get; set; }
        public IComparer<TNode> NodeComparer { get; set; }
        public Func<TState, TNode, TState> VisitNode { get; set; }

        public IEnumerable<TState> Walk(TState initialState, TNode startFrom, TNode finishAt)
        {
            if (this.VisitNode == null)
            {
                throw new InvalidOperationException("VisitNode is empty");
            }

            this.startPoints = new SortedDictionary<TNode, WalkState>(this.NodeComparer ?? Comparer<TNode>.Default);
            this.visitedLinks = new HashSet<TLink>();           

            this.startPoints.Add(startFrom, new WalkState
            {
                States = { initialState}
            });

            this.resultStates = new HashSet<TState>();

            while (this.startPoints.Any())
            {
                var startPoint = this.startPoints.First();

                this.startPoints.Remove(startPoint.Key);

                if (startPoint.Key.Equals(finishAt))
                {
                    this.resultStates.UnionWith(startPoint.Value.States);
                }
                else
                {
                    foreach (var state in startPoint.Value.States)
                    {
                        WalkFrom(startPoint.Key, state);
                    }
                }
            }

            return this.resultStates;
        }

        private void WalkFrom(TNode startNode, TState state)
        {
            state = VisitNode(state, startNode);

            foreach (var link in startNode.OutboundLinks.OfType<TLink>().Except(this.visitedLinks))
            {
                var nextNode = (TNode) link.Target;

                if(this.ShouldWalkOnlyOnce(link))
                {
                    this.visitedLinks.Add(link);
                }

                if (!this.startPoints.ContainsKey(nextNode))
                {
                    this.startPoints.Add(nextNode, new WalkState());
                }

                this.startPoints[nextNode].States.Add(state);
            }
        }       

        private class WalkState
        {
            public HashSet<TState> States { get; private set; }

            public WalkState()
            {
                this.States = new HashSet<TState>();
            }
        }
    }
}