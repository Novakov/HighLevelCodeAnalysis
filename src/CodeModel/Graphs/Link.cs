using System.Collections.Generic;
using System.Linq;

namespace CodeModel.Graphs
{
    public abstract class Link : IAnnotable
    {
        private readonly List<object> annotations;

        public Node Source { get; private set; }
        public Node Target { get; private set; }

        public IEnumerable<object> Annotations { get { return this.annotations; } }

        protected Link()
        {
            this.annotations = new List<object>();
        }

        internal void SetUpConnection(Node source, Node target)
        {
            this.Source = source;
            this.Target = target;            
        }

        public override string ToString()
        {
            return this.Source + " -> " + this.Target;
        }

        public void Annonate(object annotation)
        {
            this.annotations.Add(annotation);
        }
        
        public void RemoveAnnotation(object annotation)
        {
            this.annotations.Remove(annotation);
        }

        public TAnnotation Annotation<TAnnotation>()
        {
            return this.annotations.OfType<TAnnotation>().SingleOrDefault();
        }
    }
}