using CodeModel.Primitives;

namespace CodeModel.Conventions
{
    public interface IImmutablityConvention : IConvention
    {
        bool IsImmutableType(TypeNode node);
    }
}