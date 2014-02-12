using System;
using CodeModel.Model;

namespace CodeModel.Extensions.Cqrs
{
    public class CommandNode : TypeNode
    {
        public CommandNode(Type type) : base(type)
        {
        }
    }
}