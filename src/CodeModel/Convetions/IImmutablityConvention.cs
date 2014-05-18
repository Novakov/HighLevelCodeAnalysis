using CodeModel.Model;

namespace CodeModel.Convetions
{
    public interface IImmutablityConvention : IConvention
    {
        bool IsImmutableType(TypeNode node);
    }
}