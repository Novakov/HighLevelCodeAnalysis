using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeModel.Graphs;
using CodeModel.Model;

namespace CodeModel
{
    public static class ModelExtensions
    {
        public static TypeNode GetNodeForType(this Graph model, Type type)
        {
            return model.Nodes.OfType<TypeNode>().SingleOrDefault(x => x.Type == type);
        }
    }
}
