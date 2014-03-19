using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using Mono.Reflection;

namespace CodeModel.FlowAnalysis
{
    public class CallParameterTypesRecorder : ResolvingInstructionVisitor
    {
        public ReversableStack<PotentialType> Stack { get; private set; }

        private IDictionary<int, PotentialType> variableTypes;
        private IDictionary<int, PotentialType> parameterTypes;
        private MethodBody methodBody;

        public IDictionary<MethodBase, HashSet<PotentialType[]>> Calls { get; private set; }

        protected override void HandleUnrecognized(Instruction instruction)
        {
            var popped = instruction.PopedValuesCount(this.AnalyzedMethod);            
            var pushed = instruction.PushedValuesCount(this.AnalyzedMethod, this.methodBody);

            if (pushed != 0)
            {
                throw new InvalidOperationException("Unrecognized opcode that pushes on stack. Instruction: " + instruction);
            }

            this.Stack.PopMany(popped);
        }

        protected override void HandleNop(Instruction instruction)
        {
        }

        protected override void HandleRet(Instruction instruction)
        {
            if (this.AnalyzedMethod.ReturnType != typeof(void))
            {
                this.Stack.Pop();
            }
        }

        protected override void HandleLoadThis(Instruction instruction)
        {
            this.Stack.Push(PotentialType.FromType(this.AnalyzedMethod.ReflectedType));
        }

        protected override void HandleLoadArgument(Instruction instruction, ParameterInfo parameter)
        {
            this.Stack.Push(this.parameterTypes[parameter.Position]);
        }

        protected override void HandleStoreArgument(Instruction instruction, ParameterInfo parameter)
        {
            this.parameterTypes[parameter.Position] = this.Stack.Pop();
        }

        protected override void HandleLdstr(Instruction instruction)
        {
            this.Stack.Push(PotentialType.String);
        }

        protected override void HandleBox(Instruction instruction)
        {
            this.Stack.Push(this.Stack.Pop().Box((Type)instruction.Operand));
        }

        protected override void HandleLoadInt32(Instruction instruction, int constant)
        {
            this.Stack.Push(PotentialType.Numeric);
        }

        protected override void HandleLoadDouble(Instruction instruction, double value)
        {
            this.Stack.Push(PotentialType.Numeric);
        }

        protected override void HandleLdc_R4(Instruction instruction)
        {
            this.Stack.Push(PotentialType.FromType(typeof(float)));
        }

        protected override void HandleNewobj(Instruction instruction)
        {
            this.Stack.PopMany(instruction.PopedValuesCount(this.AnalyzedMethod));

            this.Stack.Push(PotentialType.FromType(((ConstructorInfo)instruction.Operand).ReflectedType));
        }

        protected override void HandleNewarr(Instruction instruction)
        {            
            this.Stack.Pop();
            this.Stack.Push(PotentialType.FromType(((Type)instruction.Operand).MakeArrayType()));
        }

        protected override void HandleCall(Instruction instruction)
        {
            var calledMethod = (MethodBase) instruction.Operand;
           
            var poppedCount = instruction.PopedValuesCount(this.AnalyzedMethod);

            if (!calledMethod.IsStatic)
            {
                poppedCount--;
            }

            var types = this.Stack.PopMany(poppedCount);

            if (!calledMethod.IsStatic)
            {
                this.Stack.Pop();
            }

            if (!this.Calls.ContainsKey(calledMethod))
            {
                this.Calls[calledMethod] = new HashSet<PotentialType[]>(new ArrayComparer<PotentialType>());
            }

            this.Calls[calledMethod].Add(types);

            var methodInfo = calledMethod as MethodInfo;

            if (methodInfo != null && methodInfo.ReturnType != typeof(void))
            {
                this.Stack.Push(PotentialType.FromType(methodInfo.ReturnType));                
            }            
        }

        protected override void HandleCallvirt(Instruction instruction)
        {
            //TODO: try to determine callee type
            this.HandleCall(instruction);            
        }

        protected override void HandleLdftn(Instruction instruction)
        {            
            this.Stack.Push(PotentialType.MethodHandle);
        }

        protected override void HandleLdvirtftn(Instruction instruction)
        {
            //TODO: test for ldvirtftn
            this.Stack.Push(PotentialType.MethodHandle);
        }

        protected override void HandleInitobj(Instruction instruction)
        {
            this.Stack.PopMany(instruction.PopedValuesCount(this.AnalyzedMethod));            
        }

        protected override void HandleStoreVariable(Instruction instruction, LocalVariableInfo variable)
        {
            this.variableTypes[variable.LocalIndex] = this.Stack.Pop();
        }

        protected override void HandleLoadVariable(Instruction instruction, LocalVariableInfo variable)
        {
            this.Stack.Push(this.variableTypes[variable.LocalIndex]);
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
                    this.Stack.PopMany(2);
                    this.Stack.Push(PotentialType.Numeric);
                    break;
                case BinaryOperator.And:
                case BinaryOperator.Or:
                case BinaryOperator.Xor:
                    this.Stack.PopMany(2);
                    this.Stack.Push(PotentialType.Numeric);
                    break;
                case BinaryOperator.ShiftLeft:
                case BinaryOperator.ShiftRight:
                    this.Stack.PopMany(2);
                    this.Stack.Push(PotentialType.Numeric);
                    break;
                case BinaryOperator.GreaterThan:
                case BinaryOperator.LessThan:
                case BinaryOperator.Equal:
                    this.Stack.PopMany(2);
                    this.Stack.Push(PotentialType.Boolean);
                    break;
                default:
                    base.HandleBinaryOperator(instruction, @operator);
                    break;
            }
        }

        protected override void HandleNeg(Instruction instruction)
        {
            //TODO: test for neg   
        }

        protected override void HandleNot(Instruction instruction)
        {
            //TODO: test for not   
        }

        protected override void HandleConv_R8(Instruction instruction)
        {
            this.Stack.Pop();
            this.Stack.Push(PotentialType.Numeric);
        }

        protected override void HandleConv_R4(Instruction instruction)
        {
            this.Stack.Pop();
            this.Stack.Push(PotentialType.Numeric);
        }

        protected override void HandleConv_U8(Instruction instruction)
        {
            this.Stack.Pop();
            this.Stack.Push(PotentialType.Numeric);
        }

        protected override void HandleConv_I8(Instruction instruction)
        {
            this.Stack.Pop();
            this.Stack.Push(PotentialType.Numeric);
        }

        protected override void HandleConv_R_Un(Instruction instruction)
        {
            this.Stack.Pop();
            this.Stack.Push(PotentialType.Numeric);
        }

        protected override void HandleLdloca(Instruction instruction)
        {
            this.Stack.Push(this.variableTypes[((LocalVariableInfo)instruction.Operand).LocalIndex]);
        }

        protected override void HandleLdloca_S(Instruction instruction)
        {
            this.HandleLdloca(instruction);
        }

        protected override void HandleBrfalse(Instruction instruction)
        {
            this.Stack.Pop();
        }

        protected override void HandleBrfalse_S(Instruction instruction)
        {
            this.Stack.Pop();
        }

        protected override void HandleBrtrue(Instruction instruction)
        {
            this.Stack.Pop();
        }

        protected override void HandleBrtrue_S(Instruction instruction)
        {
            this.Stack.Pop();
        }

        protected override void HandleStfld(Instruction instruction)
        {
            var fieldInfo = (FieldInfo) instruction.Operand;

            this.Stack.Pop();

            if (!fieldInfo.IsStatic)
            {
                this.Stack.Pop();
            }
        }

        protected override void HandleLdfld(Instruction instruction)
        {
            var fieldInfo = (FieldInfo)instruction.Operand;

            this.Stack.Pop();

            this.Stack.Push(PotentialType.FromType(fieldInfo.FieldType));
        }

        protected override void HandleLdsfld(Instruction instruction)
        {            
            var fieldInfo = (FieldInfo)instruction.Operand;

            this.Stack.Push(PotentialType.FromType(fieldInfo.FieldType));
        }

        protected override void HandlePop(Instruction instruction)
        {            
            this.Stack.Pop();
        }

        protected override void HandleThrow(Instruction instruction)
        {
            this.Stack.Pop();
        }

        protected override void HandleLdnull(Instruction instruction)
        {
            this.Stack.Push(PotentialType.FromType(typeof(object)));
        }

        protected override void HandleLdlen(Instruction instruction)
        {            
            this.Stack.Pop();
            this.Stack.Push(PotentialType.Numeric);                
        }

        protected override void HandleLdelem_Ref(Instruction instruction)
        {            
            var index = this.Stack.Pop();
            var arrayType = this.Stack.Pop();

            this.Stack.Push(arrayType.GetArrayElement());
        }

        protected override void HandleLoadNumericArrayElement(Instruction instruction, Type type)
        {
            var index = this.Stack.Pop();
            var arrayType = this.Stack.Pop();

            this.Stack.Push(PotentialType.Numeric);
        }

        protected override void HandleLdtoken(Instruction instruction)
        {            
            this.Stack.Push(PotentialType.Token);
        }

        protected override void HandleLdelema(Instruction instruction)
        {            
            var index = this.Stack.Pop();
            var arrayType = this.Stack.Pop();

            this.Stack.Push(arrayType.GetArrayElement());
        }

        protected override void HandleDup(Instruction instruction)
        {
            var type = this.Stack.Pop();
            this.Stack.Push(type);
            this.Stack.Push(type);
        }

        protected override void HandleCastclass(Instruction instruction)
        {
            this.Stack.Pop();
            this.Stack.Push(PotentialType.FromType((Type) instruction.Operand));
        }

        protected override void HandleLdobj(Instruction instruction)
        {
            //TODO: test for ldobj
            this.Stack.Pop();
            this.Stack.Push(PotentialType.FromType((Type)instruction.Operand));
        }

        protected override void HandleIsinst(Instruction instruction)
        {
            //TODO: test for isinst
            //TODO: need better logic
            this.Stack.Pop();
            this.Stack.Push(PotentialType.FromType((Type)instruction.Operand));
        }

        protected override void HandleLdflda(Instruction instruction)
        {
            //TODO: test for ldflda

            this.Stack.Pop();

            var field = (FieldInfo) instruction.Operand;

            this.Stack.Push(PotentialType.FromType(field.FieldType));
        }

        protected override void HandleUnbox_Any(Instruction instruction)
        {
            //TODO: test for unbox.any

            this.Stack.Pop();
            this.Stack.Push(PotentialType.FromType((Type)instruction.Operand));
        }

        protected override void HandleUnbox(Instruction instruction)
        {
            //TODO: test for unbox

            this.Stack.Pop();
            this.Stack.Push(PotentialType.FromType((Type)instruction.Operand));
        }

        protected override void HandleLdind_Ref(Instruction instruction)
        {
            //TODO: test for ldind.ref

            var tip = this.Stack.Pop();

            this.Stack.Push(tip.Type.GetElementType());
        }

        protected override void HandleLdind_I4(Instruction instruction)
        {
            //TODO: test for ldind.i4

            var tip = this.Stack.Pop();

            this.Stack.Push(PotentialType.Numeric);
        }

        protected override void HandleConversion(Instruction instruction, Type targetType)
        {
            //TODO: test for conversion
            this.Stack.Pop();
            this.Stack.Push(PotentialType.Numeric);
        }        

        protected override void BeforeInstruction(Instruction instruction)
        {
            var clause = this.methodBody.ExceptionHandlingClauses.SingleOrDefault(x => x.HandlerOffset == instruction.Offset);

            if (clause != null && clause.Flags != ExceptionHandlingClauseOptions.Finally)
            {
                this.Stack.Push(PotentialType.FromType(clause.CatchType));
            }
        }

        public override void Initialize(MethodInfo method)
        {
            base.Initialize(method);

            this.Stack = new ReversableStack<PotentialType>();

            this.methodBody = method.GetMethodBody();

            this.variableTypes = this.methodBody.LocalVariables.ToDictionary(x => x.LocalIndex, x => PotentialType.FromType(x.LocalType));
            this.parameterTypes = method.GetParameters().ToDictionary(x => x.Position, x => PotentialType.FromType(x.ParameterType));

            this.Calls = new Dictionary<MethodBase, HashSet<PotentialType[]>>();               
        }
    }
}