using CodeModel.Primitives;

namespace CodeModel.Conventions
{
    public interface IEntityConvention : IConvention
    {
        bool IsEntity(TypeNode node);
    }
}