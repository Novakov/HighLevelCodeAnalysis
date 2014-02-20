using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using Mono.Reflection;

namespace CodeModel.FlowAnalysis
{
    public class DetermineCallParameterTypes : ResolvingStackWalker
    {
        private Stack<PotentialType> stack;

        private IDictionary<int, PotentialType> variableTypes;
        private IDictionary<int, PotentialType> parameterTypes;

        public List<Tuple<Instruction, PotentialType[]>> Calls { get; private set; }

        protected override void HandleUnrecognized(Instruction instruction)
        {
            var popped = instruction.PopedValuesCount(this.AnalyzedMethod);
            var pushed = instruction.PushedValuesCount(this.AnalyzedMethod);

            if (pushed != 0)
            {
                throw new InvalidOperationException("Unrecognized opcode that pushes on stack. Instruction: " + instruction);
            }

            this.stack.PopMany(popped);
        }

        protected override void HandleNop(Instruction instruction)
        {
        }

        protected override void HandleRet(Instruction instruction)
        {
            if (this.AnalyzedMethod.ReturnType != typeof(void))
            {
                this.stack.Pop();
            }
        }

        protected override void HandleLoadThis(Instruction instruction)
        {
            this.stack.Push(PotentialType.FromType(this.AnalyzedMethod.ReflectedType));
        }

        protected override void HandleLoadArgument(Instruction instruction, ParameterInfo parameter)
        {
            this.stack.Push(this.parameterTypes[parameter.Position]);
        }

        protected override void HandleStoreArgument(Instruction instruction, ParameterInfo parameter)
        {
            this.parameterTypes[parameter.Position] = this.stack.Pop();
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
            this.stack.Push(PotentialType.Numeric);
        }

        protected override void HandleLdc_R4(Instruction instruction)
        {
            this.stack.Push(PotentialType.FromType(typeof(float)));
        }

        protected override void HandleNewobj(Instruction instruction)
        {
            this.stack.PopMany(instruction.PopedValuesCount(this.AnalyzedMethod));

            this.stack.Push(PotentialType.FromType(((ConstructorInfo)instruction.Operand).ReflectedType));
        }

        protected override void HandleNewarr(Instruction instruction)
        {
            // TODO: Test for newarr
            this.stack.Pop();
            this.stack.Push(PotentialType.FromType(((Type)instruction.Operand).MakeArrayType()));
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

            if (calledMethod.ReturnType != typeof(void))
            {
                this.stack.Push(PotentialType.FromType(calledMethod.ReturnType));                
            }            
        }

        protected override void HandleCallvirt(Instruction instruction)
        {
            //TODO: try to determine callee type
            this.HandleCall(instruction);            
        }

        protected override void HandleInitobj(Instruction instruction)
        {
            this.stack.PopMany(instruction.PopedValuesCount(this.AnalyzedMethod));            
        }

        protected override void HandleStoreVariable(Instruction instruction, LocalVariableInfo variable)
        {
            this.variableTypes[variable.LocalIndex] = this.stack.Pop();
        }

        protected override void HandleLoadVariable(Instruction instruction, LocalVariableInfo variable)
        {
            this.stack.Push(this.variableTypes[variable.LocalIndex]);
        }

        protected override void HandleBinaryOperator(Instruction instruction, BinaryOperator @operator)
        {
            switch (@operator)
            {
                case BinaryOperator.Add:
                case BinaryOperator.Subtract:
                case BinaryOperator.Multiply:
                case BinaryOperator.Divide:
                case BinaryOperator.Remainder:
                    this.stack.PopMany(2);
                    this.stack.Push(PotentialType.Numeric);
                    break;
                case BinaryOperator.And:
                case BinaryOperator.Or:
                case BinaryOperator.Xor:
                    this.stack.PopMany(2);
                    this.stack.Push(PotentialType.Numeric);
                    break;
                case BinaryOperator.ShiftLeft:
                case BinaryOperator.ShiftRight:
                    this.stack.PopMany(2);
                    this.stack.Push(PotentialType.Numeric);
                    break;
                case BinaryOperator.GreaterThan:
                case BinaryOperator.LessThan:
                case BinaryOperator.Equal:
                    this.stack.PopMany(2);
                    this.stack.Push(PotentialType.Boolean);
                    break;
                default:
                    base.HandleBinaryOperator(instruction, @operator);
                    break;
            }
        }

        protected override void HandleConv_R8(Instruction instruction)
        {
            this.stack.Pop();
            this.stack.Push(PotentialType.Numeric);
        }

        protected override void HandleConv_R4(Instruction instruction)
        {
            this.stack.Pop();
            this.stack.Push(PotentialType.Numeric);
        }

        protected override void HandleConv_U8(Instruction instruction)
        {
            this.stack.Pop();
            this.stack.Push(PotentialType.Numeric);
        }

        protected override void HandleConv_I8(Instruction instruction)
        {
            this.stack.Pop();
            this.stack.Push(PotentialType.Numeric);
        }

        protected override void HandleConv_R_Un(Instruction instruction)
        {
            this.stack.Pop();
            this.stack.Push(PotentialType.Numeric);
        }

        protected override void HandleLdloca(Instruction instruction)
        {
            this.stack.Push(this.variableTypes[((LocalVariableInfo)instruction.Operand).LocalIndex]);
        }

        protected override void HandleLdloca_S(Instruction instruction)
        {
            this.HandleLdloca(instruction);
        }

        protected override void HandleBrfalse(Instruction instruction)
        {
            this.stack.Pop();
        }

        protected override void HandleBrfalse_S(Instruction instruction)
        {
            this.stack.Pop();
        }

        protected override void HandleBrtrue(Instruction instruction)
        {
            this.stack.Pop();
        }

        protected override void HandleBrtrue_S(Instruction instruction)
        {
            this.stack.Pop();
        }

        protected override void HandleStfld(Instruction instruction)
        {
            var fieldInfo = (FieldInfo) instruction.Operand;

            this.stack.Pop();

            if (!fieldInfo.IsStatic)
            {
                this.stack.Pop();
            }
        }

        protected override void HandleLdfld(Instruction instruction)
        {
            var fieldInfo = (FieldInfo)instruction.Operand;

            this.stack.Pop();

            this.stack.Push(PotentialType.FromType(fieldInfo.FieldType));
        }

        protected override void HandleLdsfld(Instruction instruction)
        {
            //TODO: test for accessing static field
            var fieldInfo = (FieldInfo)instruction.Operand;

            this.stack.Push(PotentialType.FromType(fieldInfo.FieldType));
        }

        protected override void HandlePop(Instruction instruction)
        {            
            this.stack.Pop();
        }

        protected override void HandleThrow(Instruction instruction)
        {
            this.stack.Pop();
        }

        protected override void HandleLdnull(Instruction instruction)
        {
            this.stack.Push(PotentialType.FromType(typeof(object)));
        }

        protected override void HandleLdlen(Instruction instruction)
        {
            //TODO: test for ldlen
            this.stack.Pop();
            this.stack.Push(PotentialType.Numeric);                
        }

        protected override void HandleLdelem_Ref(Instruction instruction)
        {
            //TODO: for ldelem.ref
            var index = stack.Pop();
            var arrayType = this.stack.Pop();

            this.stack.Push(arrayType.GetArrayElement());
        }

        protected override void HandleLdtoken(Instruction instruction)
        {
            //TODO: test for ldtoken
            this.stack.Push(PotentialType.Token);
        }

        protected override void HandleConversion(Instruction instruction, Type targetType)
        {
            //TODO: test for conversion
            this.stack.Pop();
            this.stack.Push(PotentialType.Numeric);
        }        

        protected override void BeforeInstruction(Instruction instruction)
        {
            var clause = this.AnalyzedMethod.GetMethodBody().ExceptionHandlingClauses.SingleOrDefault(x => x.HandlerOffset == instruction.Offset);

            if (clause != null && clause.Flags != ExceptionHandlingClauseOptions.Finally)
            {
                this.stack.Push(PotentialType.FromType(clause.CatchType));
            }
        }

        public override void Walk(MethodInfo method, IEnumerable<InstructionNode> instructions)
        {
            this.stack = new Stack<PotentialType>();
            this.variableTypes = method.GetMethodBody().LocalVariables.ToDictionary(x => x.LocalIndex, x => PotentialType.FromType(x.LocalType));
            this.parameterTypes = method.GetParameters().ToDictionary(x => x.Position, x => PotentialType.FromType(x.ParameterType));

            this.Calls = new List<Tuple<Instruction, PotentialType[]>>();

            base.Walk(method, instructions);           

            if (this.stack.Count > 0)
            {
                throw new InvalidOperationException("Stack at the end of walking was not empty!");
            }
        }
    }
}