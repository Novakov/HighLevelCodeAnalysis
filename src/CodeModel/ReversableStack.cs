using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeModel
{
    public class ReversableStack<T> : IEnumerable<T>
    {
        private StackMark<T> currentMark;
        private Stack<StackMark<T>> marks; 

        private readonly Stack<T> innerStack;

        public ReversableStack()
        {
            this.innerStack = new Stack<T>();
            this.marks = new Stack<StackMark<T>>();
            this.currentMark = null;
        }

        public void Push(T value)
        {
            this.innerStack.Push(value);
            if (currentMark != null)
            {
                currentMark.AddRevertAction(s => s.Pop());
            }
        }

        public void PushMany(IEnumerable<T> values)
        {
            foreach (var value in values)
            {
                this.Push(value);
            }
        }

        public void PushMany(params T[] values)
        {
            this.PushMany((IEnumerable<T>)values);
        }

        public T Pop()
        {
            return this.innerStack.Pop();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.innerStack.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Mark()
        {
            this.currentMark = new StackMark<T>();
            this.marks.Push(this.currentMark);
        }

        public void Revert()
        {
            this.currentMark.Revert(this);
        }
    }
}
