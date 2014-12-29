using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CodeModel.FlowAnalysis
{
    //TODO: Try to make this work on any graph not only CFG
    //TODO: Provide version with delegates (no need for inheritance)
    public abstract class BaseCfgWalker<TState>
        where TState : IEquatable<TState>
    {
        private IDictionary<BlockNode, WalkState> startPoints;
        private HashSet<TState> resultStates;
        private HashSet<ControlTransition> visitedTransitions;

        protected IEnumerable<TState> WalkCore(MethodInfo method, ControlFlowGraph cfg)
        {
            this.startPoints = new SortedDictionary<BlockNode, WalkState>(new BlockNodeComparer());            
            this.visitedTransitions = new HashSet<ControlTransition>();

            var initialState = new WalkState();
            initialState.States.Add(this.GetInitialState(method, cfg));

            this.startPoints.Add(cfg.EntryPoint, initialState);

            this.resultStates = new HashSet<TState>();

            while (this.startPoints.Any())
            {
                var startPoint = this.startPoints.First();

                this.startPoints.Remove(startPoint.Key);

                if (startPoint.Key.Equals(cfg.ExitPoint))
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

        private void WalkFrom(BlockNode startNode, TState state)
        {
            state = VisitBlock(state, startNode);

            foreach (var transition in startNode.OutboundLinks.OfType<ControlTransition>().Except(this.visitedTransitions))
            {
                var nextBlock = (BlockNode) transition.Target;

                if(transition.Kind == TransitionKind.Backward)
                {
                    this.visitedTransitions.Add(transition);
                }

                if (!this.startPoints.ContainsKey(nextBlock))
                {
                    this.startPoints.Add(nextBlock, new WalkState());
                }

                this.startPoints[nextBlock].States.Add(state);
            }
        }

        protected abstract TState VisitBlock(TState inputState, BlockNode block);

        protected abstract TState GetInitialState(MethodInfo method, ControlFlowGraph graph);

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