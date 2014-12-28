using System;
using CodeModel.Primitives;

namespace CodeModel.Extensions.AspNetMvc
{
    public class ControllerNode : TypeNode
    {
        public ControllerNode(Type controllerType)
            : base(controllerType)
        {

        }
    }
}