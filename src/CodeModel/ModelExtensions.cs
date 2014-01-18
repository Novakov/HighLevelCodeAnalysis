using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public static MethodNode GetNodeForMethod(this Graph model, MethodInfo method)
        {
            return model.Nodes.OfType<MethodNode>().SingleOrDefault(x => x.Method == method);
        }

        public static FieldNode GetNodeForField(this Graph model, FieldInfo field)
        {
            return model.Nodes.OfType<FieldNode>().SingleOrDefault(x => x.Field == field);
        }
    }
}
