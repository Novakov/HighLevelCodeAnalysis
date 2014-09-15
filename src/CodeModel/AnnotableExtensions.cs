using System.Linq;

namespace CodeModel
{
    public static class AnnotableExtensions
    {
        public static bool HasAnnotation<TAnnotation>(this IAnnotable @this)
        {
            return @this.Annotations.OfType<TAnnotation>().Any();
        }

        public static T CopyAnnotations<T>(this T target, IAnnotable source)
            where T : IAnnotable
        {
            foreach (var annotation in source.Annotations)
            {
                target.Annonate(annotation);
            }
            
            return target;
        }
    }
}
