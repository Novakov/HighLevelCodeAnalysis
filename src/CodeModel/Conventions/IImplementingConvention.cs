using CodeModel.Primitives;

namespace CodeModel.Conventions
{
    public interface IImplementingConvention : IConvention
    {
        bool ShouldInlineImplementationsFor(TypeNode interfaceNode);
    }
}
