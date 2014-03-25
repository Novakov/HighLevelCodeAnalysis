using System.Collections.Generic;

namespace CodeModel
{
    public interface IAnnotable
    {
        void Annonate(object annotation);
        IEnumerable<object> Annotations { get; }
        void RemoveAnnotation(object annotation);
        TAnnotation Annotation<TAnnotation>();
    }
}