using Microsoft.VisualStudio.GraphModel;

namespace CodeModel.Extensions.DgmlExport
{
    static internal class GraphSchemaExtensions
    {
        internal static GraphCategory GetOrCreateCategory(this GraphSchema schema, string categoryName)
        {
            return schema.FindCategory(categoryName) ?? schema.Categories.AddNewCategory(categoryName);
        }
    }
}