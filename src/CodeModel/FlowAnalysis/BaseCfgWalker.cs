using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeModel.FlowAnalysis
{
    public abstract class BaseCfgWalker<TState>
        where TState : IEquatable<TState>
    {
        private Dictionary<BlockNode, WalkState> startPoints;
        private HashSet<TState> resultStates;
        private HashSet<ControlTransition> visitedTransitions;

        protected IEnumerable<TState> WalkCore(ControlFlowGraph cfg)
        {
            this.startPoints = new Dictionary<BlockNode, WalkState>();
            this.visitedTransitions = new HashSet<ControlTransition>();

            var initialState = new WalkState();
            initialState.States.Add(this.GetInitialState());

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

        protected abstract TState GetInitialState();

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