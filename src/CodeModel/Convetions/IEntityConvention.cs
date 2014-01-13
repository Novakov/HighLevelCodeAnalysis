using CodeModel.Model;

namespace CodeModel.Convetions
{
    public interface IEntityConvention : IConvention
    {
        bool IsEntity(TypeNode node);
    }
}