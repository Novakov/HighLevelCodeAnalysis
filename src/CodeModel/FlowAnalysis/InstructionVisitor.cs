using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Mono.Reflection;

namespace CodeModel.FlowAnalysis
{
    public abstract partial class InstructionVisitor
    {
        private Dictionary<OpCode, Action<Instruction>> handlers;

        protected MethodInfo AnalyzedMethod { get; private set; }

        public virtual void Initialize(MethodInfo method)
        {
            this.handlers = new Dictionary<OpCode, Action<Instruction>>();

            this.AnalyzedMethod = method;

            this.RegisterHandlers(handlers);
        }

        public virtual void Visit(IEnumerable<Instruction> instructions)
        {                      
            foreach (var instruction in instructions)
            {
                this.BeforeInstruction(instruction);

                this.handlers[instruction.OpCode].Invoke(instruction);

                this.AfterInstruction(instruction);
            }
        }

        protected virtual void HandleUnrecognized(Instruction instruction)
        {
        }

        protected virtual void BeforeInstruction(Instruction instruction)
        {
        }

        protected virtual void AfterInstruction(Instruction instruction)
        {
        }
    }
}
