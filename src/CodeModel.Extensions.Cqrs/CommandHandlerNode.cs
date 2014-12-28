using System;
using System.Reflection;
using CodeModel.Primitives;

namespace CodeModel.Extensions.Cqrs
{
    public class CommandHandlerNode : MethodNode
    {
        public Type HandledCommand { get; private set; }

        public CommandHandlerNode(MethodInfo method, Type handledCommand) : base(method)
        {
            this.HandledCommand = handledCommand;
        }
    }
}