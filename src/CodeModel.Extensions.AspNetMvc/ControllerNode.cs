using System;
using CodeModel.Model;

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