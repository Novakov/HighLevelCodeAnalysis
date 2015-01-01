using System;
using System.Collections.Generic;
using System.Reflection;
using CodeModel.Graphs;

namespace CodeModel.FlowAnalysis
{
    public class ControlFlowGraphWalker<TState>
        where TState : IEquatable<TState>
    {
        public TState InitialState { get; set; }
        public Func<TState, BlockNode, TState> VisitingBlock { get; set; }

        public IEnumerable<TState> WalkCore(MethodInfo method, ControlFlowGraph cfg)
        {
            var walk = new StateBasedBreadthFirstWalk<TState, BlockNode, ControlTransition>
            {
                NodeComparer = new BlockNodeComparer(),
                ShouldWalkOnlyOnce = t => t.Kind == TransitionKind.Backward,
                VisitNode = this.VisitingBlock
            };

            return walk.Walk(this.InitialState, cfg.EntryPoint, cfg.ExitPoint);
        }
    }
}