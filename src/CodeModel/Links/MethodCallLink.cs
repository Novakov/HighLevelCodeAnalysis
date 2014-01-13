using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeModel.Graphs;

namespace CodeModel.Links
{
    public class MethodCallLink : Link
    {
        public Type[] GenericMethodArguments { get; private set; }

        public MethodCallLink(Type[] genericMethodArguments)
        {
            this.GenericMethodArguments = genericMethodArguments;
        }
    }
}
