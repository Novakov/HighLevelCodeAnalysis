using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeModel
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ExportableAttribute : Attribute
    {
    }
}
