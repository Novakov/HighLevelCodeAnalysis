using System;

namespace CodeModel.Graphs
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ExportableAttribute : Attribute
    {
    }
}
