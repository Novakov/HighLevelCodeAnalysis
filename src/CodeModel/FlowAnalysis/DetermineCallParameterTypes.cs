using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Mono.Reflection;

namespace CodeModel.FlowAnalysis
{
    public class DetermineCallParameterTypes : ResolvingStackWalker
    {
        private Stack<PotentialType> stack;

        private IDictionary<int, PotentialType> variableTypes;

        public List<Tuple<Instruction, PotentialType[]>> Calls { get; private set; }                

        protected override void HandleUnrecognized(Instruction instruction)
        {
            throw new InvalidOperationException("Unrecognized opcode " + instruction);
        }

        protected override void HandleNop(Instruction instruction)
        {            
        }

        protected override void HandleRet(Instruction instruction)
        {            
        }

        protected override void HandleLoadThis(Instruction instruction)
        {
            this.stack.Push(PotentialType.Simple(this.AnalyzedMethod.ReflectedType));
        }      

        protected override void HandleLoadArgument(Instruction instruction, ParameterInfo parameter)
        {
            this.stack.Push(PotentialType.Simple(parameter.ParameterType));
        }

        protected override void HandleLdstr(Instruction instruction)
        {
            this.stack.Push(PotentialType.String);
        }

        protected override void HandleBox(Instruction instruction)
        {
            this.stack.Push(this.stack.Pop().Box((Type)instruction.Operand));
        }

        protected override void HandleLoadInt32(Instruction instruction, int constant)
        {
            this.stack.Push(PotentialType.Integer);
        }

        protected override void HandleLdc_R4(Instruction instruction)
        {
            this.stack.Push(PotentialType.Simple(typeof(float)));
        }

        protected override void HandleNewobj(Instruction instruction)
        {
            this.stack.PopMany(instruction.PopedValuesCount(this.AnalyzedMethod));

            this.stack.Push(PotentialType.Simple(((ConstructorInfo) instruction.Operand).ReflectedType));
        }

        protected override void HandleCall(Instruction instruction)
        {
            var calledMethod = (MethodInfo)instruction.Operand;

            var poppedCount = instruction.PopedValuesCount(this.AnalyzedMethod);

            if (!calledMethod.IsStatic)
            {
                poppedCount--;
            }

            var types = this.stack.PopMany(poppedCount);

            if (!calledMethod.IsStatic)
            {
                this.stack.Pop();
            }

            this.Calls.Add(Tuple.Create(instruction, types));
        }

        protected override void HandleStoreVariable(Instruction instruction, LocalVariableInfo variable)
        {
            this.variableTypes[variable.LocalIndex] = this.stack.Pop();
        }

        protected override void HandleLoadVariable(Instruction instruction, LocalVariableInfo variable)
        {
            this.stack.Push(this.variableTypes[variable.LocalIndex]);
        }

        public override void Walk(MethodInfo method, IEnumerable<InstructionNode> instructions)
        {
            this.stack = new Stack<PotentialType>();
            this.variableTypes = method.GetMethodBody().LocalVariables.ToDictionary(x => x.LocalIndex, x => PotentialType.Simple(x.LocalType));

            this.Calls = new List<Tuple<Instruction, PotentialType[]>>();

            base.Walk(method, instructions);
        }
    }
}