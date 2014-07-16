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
    public abstract partial class InstructionVisitor<TState>
    {
        private Dictionary<OpCode, Func<TState, Instruction, TState>> handlers;

        protected MethodInfo AnalyzedMethod { get; private set; }

        public virtual void Initialize(MethodInfo method)
        {
            this.handlers = new Dictionary<OpCode, Func<TState, Instruction, TState>>();

            this.AnalyzedMethod = method;

            this.RegisterHandlers(handlers);
        }

        public virtual TState Visit(TState initialState, IEnumerable<Instruction> instructions)
        {
            var state = initialState;

            foreach (var instruction in instructions)
            {
                state = this.BeforeInstruction(state, instruction);

                state = this.handlers[instruction.OpCode].Invoke(state, instruction);

                state = this.AfterInstruction(state, instruction);
            }

            return state;
        }

        protected virtual TState HandleUnrecognized(TState state, Instruction instruction)
        {
            return state;
        }

        protected virtual TState BeforeInstruction(TState state, Instruction instruction)
        {
            return state;
        }

        protected virtual TState AfterInstruction(TState state, Instruction instruction)
        {
            return state;
        }
    }
}
