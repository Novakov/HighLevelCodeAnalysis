using System;
using System.Collections.Generic;

namespace CodeModel.Graphs
{
    public class LambdaDepthFirstSearch : DepthFirstSearch
    {
        public Action<Node, IEnumerable<Link>> EnteringNode { get; set; }
        public Action<Node, IEnumerable<Link>> LeavingNode { get; set; }

        protected override void EnterNode(Node node, IEnumerable<Link> availableThrough)
        {
            if (this.EnteringNode != null)
            {
                this.EnteringNode(node, availableThrough);
            }
        }

        protected override void LeaveNode(Node node, IEnumerable<Link> availableThrough)
        {
            if (this.LeavingNode != null)
            {
                this.LeavingNode(node, availableThrough);
            }
        }
    }
}