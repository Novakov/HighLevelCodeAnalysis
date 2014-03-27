using System.Linq;

namespace CodeModel
{
    public static class AnnotableExtensions
    {
        public static bool HasAnnotation<TAnnotation>(this IAnnotable @this)
        {
            return @this.Annotations.OfType<TAnnotation>().Any();
        }
    }
}
