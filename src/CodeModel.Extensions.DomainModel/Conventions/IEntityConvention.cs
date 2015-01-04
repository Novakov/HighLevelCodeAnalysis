using CodeModel.Conventions;
using CodeModel.Primitives;

namespace CodeModel.Extensions.DomainModel.Conventions
{
    public interface IEntityConvention : IConvention
    {
        bool IsEntity(TypeNode node);
    }
}