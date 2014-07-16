using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Mono.Reflection;

namespace CodeModel.FlowAnalysis
{
    public abstract class ResolvingInstructionVisitor<TState> : InstructionVisitor<TState>
    {
        protected override void RegisterHandlers(Dictionary<OpCode, Func<TState, Instruction, TState>> registry)
        {
            base.RegisterHandlers(registry);

            var body = new Lazy<MethodBody>(() => this.AnalyzedMethod.GetMethodBody());

            registry[OpCodes.Starg] = (s, i) => HandleStoreArgument(s, i, (ParameterInfo)i.Operand);
            registry[OpCodes.Starg_S] = (s, i) => HandleStoreArgument(s, i, (ParameterInfo)i.Operand);

            registry[OpCodes.Ldarg_1] = (s, i) => LoadArgByIndex(s, i, 1);
            registry[OpCodes.Ldarg_2] = (s, i) => LoadArgByIndex(s, i, 2);
            registry[OpCodes.Ldarg_3] = (s, i) => LoadArgByIndex(s, i, 3);
            registry[OpCodes.Ldarga] = (s, i) => HandleLoadArgument(s, i, (ParameterInfo)i.Operand);
            registry[OpCodes.Ldarga_S] = (s, i) => HandleLoadArgument(s, i, (ParameterInfo)i.Operand);

            registry[OpCodes.Stloc] = (s, i) => HandleStoreVariable(s, i, (LocalVariableInfo)i.Operand);
            registry[OpCodes.Stloc_S] = (s, i) => HandleStoreVariable(s, i, (LocalVariableInfo)i.Operand);
            registry[OpCodes.Stloc_0] = (s, i) => HandleStoreVariable(s, i, body.Value.LocalVariables[0]);
            registry[OpCodes.Stloc_1] = (s, i) => HandleStoreVariable(s, i, body.Value.LocalVariables[1]);
            registry[OpCodes.Stloc_2] = (s, i) => HandleStoreVariable(s, i, body.Value.LocalVariables[2]);
            registry[OpCodes.Stloc_3] = (s, i) => HandleStoreVariable(s, i, body.Value.LocalVariables[3]);

            registry[OpCodes.Ldloc] = (s, i) => HandleLoadVariable(s, i, (LocalVariableInfo)i.Operand);
            registry[OpCodes.Ldloc_S] = (s, i) => HandleLoadVariable(s, i, (LocalVariableInfo)i.Operand);
            registry[OpCodes.Ldloc_0] = (s, i) => HandleLoadVariable(s, i, body.Value.LocalVariables[0]);
            registry[OpCodes.Ldloc_1] = (s, i) => HandleLoadVariable(s, i, body.Value.LocalVariables[1]);
            registry[OpCodes.Ldloc_2] = (s, i) => HandleLoadVariable(s, i, body.Value.LocalVariables[2]);
            registry[OpCodes.Ldloc_3] = (s, i) => HandleLoadVariable(s, i, body.Value.LocalVariables[3]);

            registry[OpCodes.Ldc_I4] = (s, i) => HandleLoadInt32(s, i, (int)i.Operand);
            registry[OpCodes.Ldc_I4_S] = (s, i) => HandleLoadInt32(s, i, (sbyte)i.Operand);
            registry[OpCodes.Ldc_I4_M1] = (s, i) => HandleLoadInt32(s, i, -1);
            registry[OpCodes.Ldc_I4_0] = (s, i) => HandleLoadInt32(s, i, 0);
            registry[OpCodes.Ldc_I4_1] = (s, i) => HandleLoadInt32(s, i, 1);
            registry[OpCodes.Ldc_I4_2] = (s, i) => HandleLoadInt32(s, i, 2);
            registry[OpCodes.Ldc_I4_3] = (s, i) => HandleLoadInt32(s, i, 3);
            registry[OpCodes.Ldc_I4_4] = (s, i) => HandleLoadInt32(s, i, 4);
            registry[OpCodes.Ldc_I4_5] = (s, i) => HandleLoadInt32(s, i, 5);
            registry[OpCodes.Ldc_I4_6] = (s, i) => HandleLoadInt32(s, i, 6);
            registry[OpCodes.Ldc_I4_7] = (s, i) => HandleLoadInt32(s, i, 7);
            registry[OpCodes.Ldc_I4_8] = (s, i) => HandleLoadInt32(s, i, 8);

            registry[OpCodes.Ldc_R8] = (s, i) => HandleLoadDouble(s, i, (double)i.Operand);

            registry[OpCodes.Add] = (s, i) => HandleBinaryOperator(s, i, BinaryOperator.Add);
            registry[OpCodes.Sub] = (s, i) => HandleBinaryOperator(s, i, BinaryOperator.Subtract);
            registry[OpCodes.Mul] = (s, i) => HandleBinaryOperator(s, i, BinaryOperator.Multiply);
            registry[OpCodes.Div] = (s, i) => HandleBinaryOperator(s, i, BinaryOperator.Divide);
            registry[OpCodes.Rem] = (s, i) => HandleBinaryOperator(s, i, BinaryOperator.Remainder);
            registry[OpCodes.Cgt] = (s, i) => HandleBinaryOperator(s, i, BinaryOperator.GreaterThan);
            registry[OpCodes.Cgt_Un] = (s, i) => HandleBinaryOperator(s, i, BinaryOperator.GreaterThan);
            registry[OpCodes.Clt] = (s, i) => HandleBinaryOperator(s, i, BinaryOperator.LessThan);
            registry[OpCodes.Clt_Un] = (s, i) => HandleBinaryOperator(s, i, BinaryOperator.LessThan);
            registry[OpCodes.Ceq] = (s, i) => HandleBinaryOperator(s, i, BinaryOperator.Equal);

            registry[OpCodes.And] = (s, i) => HandleBinaryOperator(s, i, BinaryOperator.And);
            registry[OpCodes.Or] = (s, i) => HandleBinaryOperator(s, i, BinaryOperator.Or);
            registry[OpCodes.Xor] = (s, i) => HandleBinaryOperator(s, i, BinaryOperator.Xor);
            registry[OpCodes.Shl] = (s, i) => HandleBinaryOperator(s, i, BinaryOperator.ShiftLeft);
            registry[OpCodes.Shr] = (s, i) => HandleBinaryOperator(s, i, BinaryOperator.ShiftRight);
            registry[OpCodes.Shr_Un] = (s, i) => HandleBinaryOperator(s, i, BinaryOperator.ShiftRight);

            //conversions            
            registry[OpCodes.Conv_I] = (s, i) => HandleConversion(s, i, typeof(int));
            registry[OpCodes.Conv_I1] = (s, i) => HandleConversion(s, i, typeof(int));
            registry[OpCodes.Conv_I2] = (s, i) => HandleConversion(s, i, typeof(int));
            registry[OpCodes.Conv_I4] = (s, i) => HandleConversion(s, i, typeof(int));
            registry[OpCodes.Conv_I8] = (s, i) => HandleConversion(s, i, typeof(long));
            registry[OpCodes.Conv_Ovf_I] = (s, i) => HandleConversion(s, i, typeof(int));
            registry[OpCodes.Conv_Ovf_I1] = (s, i) => HandleConversion(s, i, typeof(int));
            registry[OpCodes.Conv_Ovf_I2] = (s, i) => HandleConversion(s, i, typeof(int));
            registry[OpCodes.Conv_Ovf_I4] = (s, i) => HandleConversion(s, i, typeof(int));
            registry[OpCodes.Conv_Ovf_I8] = (s, i) => HandleConversion(s, i, typeof(long));

            registry[OpCodes.Ldelem_I] = (s, i) => HandleLoadNumericArrayElement(s, i, typeof(int));
            registry[OpCodes.Ldelem_I1] = (s, i) => HandleLoadNumericArrayElement(s, i, typeof(int));
            registry[OpCodes.Ldelem_I2] = (s, i) => HandleLoadNumericArrayElement(s, i, typeof(int));
            registry[OpCodes.Ldelem_I4] = (s, i) => HandleLoadNumericArrayElement(s, i, typeof(int));
            registry[OpCodes.Ldelem_I8] = (s, i) => HandleLoadNumericArrayElement(s, i, typeof(long));
            registry[OpCodes.Ldelem_U1] = (s, i) => HandleLoadNumericArrayElement(s, i, typeof(uint));
            registry[OpCodes.Ldelem_U2] = (s, i) => HandleLoadNumericArrayElement(s, i, typeof(uint));
            registry[OpCodes.Ldelem_U4] = (s, i) => HandleLoadNumericArrayElement(s, i, typeof(uint));
            registry[OpCodes.Ldelem_R4] = (s, i) => HandleLoadNumericArrayElement(s, i, typeof(float));
            registry[OpCodes.Ldelem_R8] = (s, i) => HandleLoadNumericArrayElement(s, i, typeof(double));
        }

        protected virtual TState HandleLoadNumericArrayElement(TState state, Instruction instruction, Type type)
        {
            return this.HandleUnrecognized(state, instruction);
        }

        protected virtual TState HandleLoadDouble(TState state, Instruction instruction, double value)
        {
            return this.HandleUnrecognized(state, instruction);
        }

        protected virtual TState HandleConversion(TState state, Instruction instruction, Type targetType)
        {
            return this.HandleUnrecognized(state, instruction);
        }

        protected virtual TState HandleBinaryOperator(TState state, Instruction instruction, BinaryOperator @operator)
        {
            return this.HandleUnrecognized(state, instruction);
        }

        protected virtual TState HandleStoreArgument(TState state, Instruction instruction, ParameterInfo parameter)
        {
            return this.HandleUnrecognized(state, instruction);
        }

        protected virtual TState HandleLoadInt32(TState state, Instruction instruction, int constant)
        {
            return this.HandleUnrecognized(state, instruction);
        }

        protected virtual TState HandleStoreVariable(TState state, Instruction instruction, LocalVariableInfo variable)
        {
            return this.HandleUnrecognized(state, instruction);
        }

        protected virtual TState HandleLoadVariable(TState state, Instruction instruction, LocalVariableInfo variable)
        {
            return this.HandleUnrecognized(state, instruction);
        }

        protected override TState HandleLdarg(TState state, Instruction instruction)
        {
            var param = (ParameterInfo) instruction.Operand;
            
            return this.HandleLoadArgument(state, instruction, param);
        }

        protected override TState HandleLdarg_0(TState state, Instruction instruction)
        {
            if (this.AnalyzedMethod.IsStatic)
            {
                return this.HandleLoadArgument(state, instruction, this.AnalyzedMethod.GetParameters()[0]);
            }
            else
            {
                return this.HandleLoadThis(state, instruction);
            }
        }

        private TState LoadArgByIndex(TState state, Instruction instruction, int index)
        {
            if (this.AnalyzedMethod.IsStatic)
            {
                return this.HandleLoadArgument(state, instruction, this.AnalyzedMethod.GetParameters()[index]);
            }
            else
            {
                return this.HandleLoadArgument(state, instruction, this.AnalyzedMethod.GetParameters()[index - 1]);
            }
        }

        protected override TState HandleLdarg_S(TState state, Instruction instruction)
        {
            return this.HandleLoadArgument(state, instruction, (ParameterInfo)instruction.Operand);
        }

        protected virtual TState HandleLoadThis(TState state, Instruction instruction)
        {
            return this.HandleUnrecognized(state, instruction);
        }

        protected virtual TState HandleLoadArgument(TState state, Instruction instruction, ParameterInfo parameter)
        {
            return this.HandleUnrecognized(state, instruction);
        }
    }
}