using System;
using System.Collections.Generic;

namespace CodeModel
{
    internal class StackMark<T>
    {
        private readonly List<Action<ReversableStack<T>>> revertActions;

        public StackMark()
        {
            this.revertActions = new List<Action<ReversableStack<T>>>();
        }

        public void AddRevertAction(Action<ReversableStack<T>> action)
        {
            this.revertActions.Add(action);
        }

        public void Revert(ReversableStack<T> target)
        {
            for (int i = this.revertActions.Count - 1; i >= 0; i--)
            {
                this.revertActions[i](target);
            }
        }
    }
}