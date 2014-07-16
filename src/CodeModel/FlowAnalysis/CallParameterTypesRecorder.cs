using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using Mono.Reflection;

namespace CodeModel.FlowAnalysis
{
    public class CallParameterTypesRecorder : ResolvingInstructionVisitor<TypeAnalysisState>
    {        
        private MethodBody methodBody;

        public IDictionary<MethodBase, HashSet<PotentialType[]>> Calls { get; private set; }

        protected override TypeAnalysisState HandleUnrecognized(TypeAnalysisState state, Instruction instruction)
        {
            var popped = instruction.PopedValuesCount(this.AnalyzedMethod);            
            var pushed = instruction.PushedValuesCount(this.AnalyzedMethod, this.methodBody);

            if (pushed != 0)
            {
                throw new InvalidOperationException("Unrecognized opcode that pushes on stack. Instruction: " + instruction);
            }

            PotentialType[] values;

            return state.PopMany(popped, out values);            
        }

        protected override TypeAnalysisState HandleNop(TypeAnalysisState state, Instruction instruction)
        {
            return state;
        }

        protected override TypeAnalysisState HandleRet(TypeAnalysisState state, Instruction instruction)
        {
            if (this.AnalyzedMethod.ReturnType != typeof(void))
            {
                PotentialType value;
                return state.Pop(out value);
            }

            return state;
        }

        protected override TypeAnalysisState HandleLoadThis(TypeAnalysisState state, Instruction instruction)
        {
            return state.Push(PotentialType.FromType(this.AnalyzedMethod.ReflectedType));
        }

        protected override TypeAnalysisState HandleLoadArgument(TypeAnalysisState state, Instruction instruction, ParameterInfo parameter)
        {
            return state.Push(state.Param(parameter.Position));
        }

        protected override TypeAnalysisState HandleStoreArgument(TypeAnalysisState state, Instruction instruction, ParameterInfo parameter)
        {
            PotentialType type;
            return state.Pop(out type)
                .WithParam(parameter.Position, type);
        }

        protected override TypeAnalysisState HandleLdstr(TypeAnalysisState state, Instruction instruction)
        {
            return state.Push(PotentialType.String);
        }

        protected override TypeAnalysisState HandleBox(TypeAnalysisState state, Instruction instruction)
        {
            PotentialType type;
            return state.Pop(out type)
                .Push(type.Box((Type) instruction.Operand));                        
        }

        protected override TypeAnalysisState HandleLoadInt32(TypeAnalysisState state, Instruction instruction, int constant)
        {
            return state.Push(PotentialType.Numeric);
        }

        protected override TypeAnalysisState HandleLoadDouble(TypeAnalysisState state, Instruction instruction, double value)
        {
            return state.Push(PotentialType.Numeric);
        }

        protected override TypeAnalysisState HandleLdc_R4(TypeAnalysisState state, Instruction instruction)
        {
            return state.Push(PotentialType.FromType(typeof(float)));
        }

        protected override TypeAnalysisState HandleNewobj(TypeAnalysisState state, Instruction instruction)
        {
            return state
                .Drop(instruction.PopedValuesCount(this.AnalyzedMethod))
                .Push(PotentialType.FromType(((ConstructorInfo)instruction.Operand).ReflectedType));
        }

        protected override TypeAnalysisState HandleNewarr(TypeAnalysisState state, Instruction instruction)
        {            
            return state
                .Drop(1)
                .Push(PotentialType.FromType(((Type)instruction.Operand).MakeArrayType()));
        }

        protected override TypeAnalysisState HandleCall(TypeAnalysisState state, Instruction instruction)
        {
            var calledMethod = (MethodBase) instruction.Operand;
           
            var poppedCount = instruction.PopedValuesCount(this.AnalyzedMethod);

            if (!calledMethod.IsStatic)
            {
                poppedCount--;
            }

            PotentialType[] types;
            state = state.PopMany(poppedCount, out types);

            if (!calledMethod.IsStatic)
            {
                state = state.Drop(1);
            }

            if (!this.Calls.ContainsKey(calledMethod))
            {
                this.Calls[calledMethod] = new HashSet<PotentialType[]>(new ArrayComparer<PotentialType>());
            }

            this.Calls[calledMethod].Add(types);

            var methodInfo = calledMethod as MethodInfo;

            if (methodInfo != null && methodInfo.ReturnType != typeof(void))
            {
                state = state.Push(PotentialType.FromType(methodInfo.ReturnType));                
            }

            return state;
        }

        protected override TypeAnalysisState HandleCallvirt(TypeAnalysisState state, Instruction instruction)
        {
            //TODO: try to determine callee type
            return this.HandleCall(state, instruction);            
        }

        protected override TypeAnalysisState HandleLdftn(TypeAnalysisState state, Instruction instruction)
        {
            return state.Push(PotentialType.MethodHandle);
        }

        protected override TypeAnalysisState HandleLdvirtftn(TypeAnalysisState state, Instruction instruction)
        {
            return state.Drop(1)
                .Push(PotentialType.MethodHandle);
        }

        protected override TypeAnalysisState HandleInitobj(TypeAnalysisState state, Instruction instruction)
        {
            PotentialType[] values;
            return state.PopMany(instruction.PopedValuesCount(this.AnalyzedMethod), out values);
        }

        protected override TypeAnalysisState HandleStoreVariable(TypeAnalysisState state, Instruction instruction, LocalVariableInfo variable)
        {
            PotentialType type;
            return state
                .Pop(out type)
                .WithVariable(variable.LocalIndex, type);              
        }

        protected override TypeAnalysisState HandleLoadVariable(TypeAnalysisState state, Instruction instruction, LocalVariableInfo variable)
        {
            return state.Push(state.Variable(variable.LocalIndex));
        }

        protected override TypeAnalysisState HandleBinaryOperator(TypeAnalysisState state, Instruction instruction, BinaryOperator @operator)
        {
            switch (@operator)
            {
                case BinaryOperator.Add:
                case BinaryOperator.Subtract:
                case BinaryOperator.Multiply:
                case BinaryOperator.Divide:
                case BinaryOperator.Remainder:
                    return state.Drop(2).Push(PotentialType.Numeric);                    
                case BinaryOperator.And:
                case BinaryOperator.Or:
                case BinaryOperator.Xor:
                    return state.Drop(2).Push(PotentialType.Numeric);
                case BinaryOperator.ShiftLeft:
                case BinaryOperator.ShiftRight:
                    return state.Drop(2).Push(PotentialType.Numeric);
                case BinaryOperator.GreaterThan:
                case BinaryOperator.LessThan:
                case BinaryOperator.Equal:
                    return state.Drop(2).Push(PotentialType.Boolean);
                default:
                    return base.HandleBinaryOperator(state, instruction, @operator);
            }
        }

        protected override TypeAnalysisState HandleNeg(TypeAnalysisState state, Instruction instruction)
        {
            return state;
        }

        protected override TypeAnalysisState HandleNot(TypeAnalysisState state, Instruction instruction)
        {
            return state;
        }

        protected override TypeAnalysisState HandleConv_R8(TypeAnalysisState state, Instruction instruction)
        {
            return state
                .Drop(1)
                .Push(PotentialType.Numeric);                            
        }

        protected override TypeAnalysisState HandleConv_R4(TypeAnalysisState state, Instruction instruction)
        {
            return state
                .Drop(1)
                .Push(PotentialType.Numeric);      
        }

        protected override TypeAnalysisState HandleConv_U8(TypeAnalysisState state, Instruction instruction)
        {
            return state
                .Drop(1)
                .Push(PotentialType.Numeric);      
        }

        protected override TypeAnalysisState HandleConv_I8(TypeAnalysisState state, Instruction instruction)
        {
            return state
                 .Drop(1)
                 .Push(PotentialType.Numeric);      
        }

        protected override TypeAnalysisState HandleConv_R_Un(TypeAnalysisState state, Instruction instruction)
        {
            return state
                 .Drop(1)
                 .Push(PotentialType.Numeric);      
        }

        protected override TypeAnalysisState HandleLdloca(TypeAnalysisState state, Instruction instruction)
        {
            return state.Push(state.Variable(((LocalVariableInfo)instruction.Operand).LocalIndex));
        }

        protected override TypeAnalysisState HandleLdloca_S(TypeAnalysisState state, Instruction instruction)
        {
            return this.HandleLdloca(state, instruction);
        }

        protected override TypeAnalysisState HandleBrfalse(TypeAnalysisState state, Instruction instruction)
        {
            PotentialType value;
            return state.Pop(out value);
        }

        protected override TypeAnalysisState HandleBrfalse_S(TypeAnalysisState state, Instruction instruction)
        {
            PotentialType value;
            return state.Pop(out value);
        }

        protected override TypeAnalysisState HandleBrtrue(TypeAnalysisState state, Instruction instruction)
        {
            PotentialType value;
            return state.Pop(out value);
        }

        protected override TypeAnalysisState HandleBrtrue_S(TypeAnalysisState state, Instruction instruction)
        {
            PotentialType value;
            return state.Pop(out value);
        }

        protected override TypeAnalysisState HandleStfld(TypeAnalysisState state, Instruction instruction)
        {
            var fieldInfo = (FieldInfo) instruction.Operand;

            state = state.Drop(1);

            if (!fieldInfo.IsStatic)
            {
                state = state.Drop(1);                
            }

            return state;
        }

        protected override TypeAnalysisState HandleLdfld(TypeAnalysisState state, Instruction instruction)
        {
            var fieldInfo = (FieldInfo)instruction.Operand;

            return state
                .Drop(1)
                .Push(PotentialType.FromType(fieldInfo.FieldType));      
        }

        protected override TypeAnalysisState HandleLdsfld(TypeAnalysisState state, Instruction instruction)
        {            
            var fieldInfo = (FieldInfo)instruction.Operand;

            return state.Push(PotentialType.FromType(fieldInfo.FieldType));
        }

        protected override TypeAnalysisState HandlePop(TypeAnalysisState state, Instruction instruction)
        {
            PotentialType value;
            return state.Pop(out value);
        }

        protected override TypeAnalysisState HandleThrow(TypeAnalysisState state, Instruction instruction)
        {
            PotentialType value;
            return state.Pop(out value);
        }

        protected override TypeAnalysisState HandleLdnull(TypeAnalysisState state, Instruction instruction)
        {
            return state.Push(PotentialType.FromType(typeof(object)));
        }

        protected override TypeAnalysisState HandleLdlen(TypeAnalysisState state, Instruction instruction)
        {
            return state
                 .Drop(1)
                 .Push(PotentialType.Numeric);                
        }

        protected override TypeAnalysisState HandleLdelem_Ref(TypeAnalysisState state, Instruction instruction)
        {
            PotentialType index;
            PotentialType arrayType;

            return state
                .Pop(out index)
                .Pop(out arrayType)
                .Push(arrayType.GetArrayElement());
        }

        protected override TypeAnalysisState HandleLoadNumericArrayElement(TypeAnalysisState state, Instruction instruction, Type type)
        {
            return state
                .Drop(2)
                .Push(PotentialType.Numeric);            
        }

        protected override TypeAnalysisState HandleLdtoken(TypeAnalysisState state, Instruction instruction)
        {            
            return state.Push(PotentialType.Token);
        }

        protected override TypeAnalysisState HandleLdelema(TypeAnalysisState state, Instruction instruction)
        {
            PotentialType index;
            PotentialType arrayType;

            return state
                .Pop(out index)
                .Pop(out arrayType)
                .Push(arrayType.GetArrayElement());
        }

        protected override TypeAnalysisState HandleDup(TypeAnalysisState state, Instruction instruction)
        {
            PotentialType type;
            return state
                .Pop(out type)
                .Push(type)
                .Push(type);
        }

        protected override TypeAnalysisState HandleCastclass(TypeAnalysisState state, Instruction instruction)
        {
            return state
                 .Drop(1)
                 .Push(PotentialType.FromType((Type)instruction.Operand));   
        }

        protected override TypeAnalysisState HandleLdobj(TypeAnalysisState state, Instruction instruction)
        {
            //TODO: test for ldobj
            return state
                 .Drop(1)
                 .Push(PotentialType.FromType((Type)instruction.Operand));   
        }

        protected override TypeAnalysisState HandleIsinst(TypeAnalysisState state, Instruction instruction)
        {            
            //TODO: need better logic
            return state
                 .Drop(1)
                 .Push(PotentialType.FromType((Type)instruction.Operand));   
        }

        protected override TypeAnalysisState HandleLdflda(TypeAnalysisState state, Instruction instruction)
        {                       
            var field = (FieldInfo) instruction.Operand;

            return state
                 .Drop(1)
                 .Push(PotentialType.FromType(field.FieldType));   
        }

        protected override TypeAnalysisState HandleUnbox_Any(TypeAnalysisState state, Instruction instruction)
        {
            return state
                 .Drop(1)
                 .Push(PotentialType.FromType((Type)instruction.Operand));   
        }

        protected override TypeAnalysisState HandleUnbox(TypeAnalysisState state, Instruction instruction)
        {
            return state
                  .Drop(1)
                  .Push(PotentialType.FromType((Type)instruction.Operand));   
        }

        protected override TypeAnalysisState HandleLdind_Ref(TypeAnalysisState state, Instruction instruction)
        {
            //TODO: test for ldind.ref
            
            PotentialType tip;

            return state                
                .Pop(out tip)
                .Push(tip.Type.GetElementType());
        }

        protected override TypeAnalysisState HandleLdind_I4(TypeAnalysisState state, Instruction instruction)
        {
            //TODO: test for ldind.i4

            return state
                  .Drop(1)
                  .Push(PotentialType.Numeric);   
        }

        protected override TypeAnalysisState HandleConversion(TypeAnalysisState state, Instruction instruction, Type targetType)
        {
            //TODO: test for conversion
            return state
                  .Drop(1)
                  .Push(PotentialType.Numeric);   
        }        

        protected override TypeAnalysisState BeforeInstruction(TypeAnalysisState state, Instruction instruction)
        {
            var clause = this.methodBody.ExceptionHandlingClauses.SingleOrDefault(x => x.HandlerOffset == instruction.Offset);

            if (clause != null && clause.Flags != ExceptionHandlingClauseOptions.Finally)
            {
                return state.Push(PotentialType.FromType(clause.CatchType));
            }

            return state;
        }

        public override void Initialize(MethodInfo method)
        {
            base.Initialize(method);            

            this.methodBody = method.GetMethodBody();                       

            this.Calls = new Dictionary<MethodBase, HashSet<PotentialType[]>>();               
        }
    }
}