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
    public abstract partial class StackWalker
    {
        private Dictionary<OpCode, Action<Instruction>> handlers;

        protected MethodInfo AnalyzedMethod { get; private set; }

        public virtual void Walk(MethodInfo method, IEnumerable<InstructionNode> instructions)
        {
            this.handlers = new Dictionary<OpCode, Action<Instruction>>();

            this.AnalyzedMethod = method;

            this.RegisterHandlers(handlers);

            foreach (var instruction in instructions.Select(x => x.Instruction))
            {
                this.handlers[instruction.OpCode].Invoke(instruction);
            }
        }

        protected virtual void HandleUnrecognized(Instruction instruction)
        {
        }
    }

    public abstract class ResolvingStackWalker : StackWalker
    {
        protected override void RegisterHandlers(Dictionary<OpCode, Action<Instruction>> registry)
        {
            base.RegisterHandlers(registry);

            registry[OpCodes.Ldarg_1] = i => LoadArgByIndex(i, 1);
            registry[OpCodes.Ldarg_2] = i => LoadArgByIndex(i, 2);
            registry[OpCodes.Ldarg_3] = i => LoadArgByIndex(i, 3);
            
            registry[OpCodes.Ldc_I4] = i => HandleLoadInt32(i, (int)i.Operand);
            registry[OpCodes.Ldc_I4_S] = i => HandleLoadInt32(i, (int)i.Operand);
            registry[OpCodes.Ldc_I4_M1] = i => HandleLoadInt32(i, -1);
            registry[OpCodes.Ldc_I4_0] = i => HandleLoadInt32(i, 0);
            registry[OpCodes.Ldc_I4_1] = i => HandleLoadInt32(i, 1);
            registry[OpCodes.Ldc_I4_2] = i => HandleLoadInt32(i, 2);
            registry[OpCodes.Ldc_I4_3] = i => HandleLoadInt32(i, 3);
            registry[OpCodes.Ldc_I4_4] = i => HandleLoadInt32(i, 4);
            registry[OpCodes.Ldc_I4_5] = i => HandleLoadInt32(i, 5);
            registry[OpCodes.Ldc_I4_6] = i => HandleLoadInt32(i, 6);
            registry[OpCodes.Ldc_I4_7] = i => HandleLoadInt32(i, 7);
            registry[OpCodes.Ldc_I4_8] = i => HandleLoadInt32(i, 8);            
        }

        protected virtual void HandleLoadInt32(Instruction instruction, int constant)
        {
            this.HandleUnrecognized(instruction);
        }

        protected override void HandleLdarg(Instruction instruction)
        {
            var paramIndex = (int)instruction.Operand;

            if (paramIndex == 0 && !this.AnalyzedMethod.IsStatic)
            {
                this.HandleLoadThis(instruction);
            }
            else
            {
                this.HandleLoadArgument(instruction, this.AnalyzedMethod.GetParameters()[paramIndex]);
            }            
        }

        protected override void HandleLdarg_0(Instruction instruction)
        {
            if (this.AnalyzedMethod.IsStatic)
            {
                this.HandleLoadArgument(instruction, this.AnalyzedMethod.GetParameters()[0]);
            }
            else
            {
                this.HandleLoadThis(instruction);
            }
        }

        private void LoadArgByIndex(Instruction instruction, int index)
        {
            if (this.AnalyzedMethod.IsStatic)
            {
                this.HandleLoadArgument(instruction, this.AnalyzedMethod.GetParameters()[index]);
            }
            else
            {
                this.HandleLoadArgument(instruction, this.AnalyzedMethod.GetParameters()[index - 1]);
            }
        }

        protected override void HandleLdarg_S(Instruction instruction)
        {
            this.HandleLoadArgument(instruction, (ParameterInfo)instruction.Operand);
        }

        protected virtual void HandleLoadThis(Instruction instruction)
        {            
        }

        protected virtual void HandleLoadArgument(Instruction instruction, ParameterInfo parameter)
        {
            
        }
    }
}
