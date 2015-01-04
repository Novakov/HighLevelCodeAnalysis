using CodeModel.Conventions;
using CodeModel.Primitives;

namespace CodeModel.Extensions.DomainModel.Conventions
{
    public interface IDomainModelConvention : IConvention
    {
        bool IsEntity(TypeNode node);
        bool IsAggregate(TypeNode node);
    }
}