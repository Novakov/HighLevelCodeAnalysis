using System;
using CodeModel.Model;

namespace CodeModel.Extensions.Cqrs
{
    public class CommandNode : TypeNode
    {
        public CommandNode(Type type)
            : base(type)
        {
        }

        public override string DisplayLabel
        {
            get
            {
                return "Command:" + this.Type.FullName;
            }
        }
    }
}