﻿using System;
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

    public abstract class ResolvingInstructionVisitor : InstructionVisitor
    {
        protected override void RegisterHandlers(Dictionary<OpCode, Action<Instruction>> registry)
        {
            base.RegisterHandlers(registry);

            var body = new Lazy<MethodBody>(() => this.AnalyzedMethod.GetMethodBody());

            registry[OpCodes.Starg] = i => HandleStoreArgument(i, (ParameterInfo)i.Operand);
            registry[OpCodes.Starg_S] = i => HandleStoreArgument(i, (ParameterInfo)i.Operand);

            registry[OpCodes.Ldarg_1] = i => LoadArgByIndex(i, 1);
            registry[OpCodes.Ldarg_2] = i => LoadArgByIndex(i, 2);
            registry[OpCodes.Ldarg_3] = i => LoadArgByIndex(i, 3);
            registry[OpCodes.Ldarga] = i => HandleLoadArgument(i, (ParameterInfo)i.Operand);
            registry[OpCodes.Ldarga_S] = i => HandleLoadArgument(i, (ParameterInfo)i.Operand);

            registry[OpCodes.Stloc] = i => HandleStoreVariable(i, (LocalVariableInfo)i.Operand);
            registry[OpCodes.Stloc_S] = i => HandleStoreVariable(i, (LocalVariableInfo)i.Operand);
            registry[OpCodes.Stloc_0] = i => HandleStoreVariable(i, body.Value.LocalVariables[0]);
            registry[OpCodes.Stloc_1] = i => HandleStoreVariable(i, body.Value.LocalVariables[1]);
            registry[OpCodes.Stloc_2] = i => HandleStoreVariable(i, body.Value.LocalVariables[2]);
            registry[OpCodes.Stloc_3] = i => HandleStoreVariable(i, body.Value.LocalVariables[3]);

            registry[OpCodes.Ldloc] = i => HandleLoadVariable(i, (LocalVariableInfo)i.Operand);
            registry[OpCodes.Ldloc_S] = i => HandleLoadVariable(i, (LocalVariableInfo)i.Operand);
            registry[OpCodes.Ldloc_0] = i => HandleLoadVariable(i, body.Value.LocalVariables[0]);
            registry[OpCodes.Ldloc_1] = i => HandleLoadVariable(i, body.Value.LocalVariables[1]);
            registry[OpCodes.Ldloc_2] = i => HandleLoadVariable(i, body.Value.LocalVariables[2]);
            registry[OpCodes.Ldloc_3] = i => HandleLoadVariable(i, body.Value.LocalVariables[3]);

            registry[OpCodes.Ldc_I4] = i => HandleLoadInt32(i, (int)i.Operand);
            registry[OpCodes.Ldc_I4_S] = i => HandleLoadInt32(i, (sbyte)i.Operand);
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

            registry[OpCodes.Ldc_R8] = i => HandleLoadDouble(i, (double) i.Operand);

            registry[OpCodes.Add] = i => HandleBinaryOperator(i, BinaryOperator.Add);
            registry[OpCodes.Sub] = i => HandleBinaryOperator(i, BinaryOperator.Subtract);
            registry[OpCodes.Mul] = i => HandleBinaryOperator(i, BinaryOperator.Multiply);
            registry[OpCodes.Div] = i => HandleBinaryOperator(i, BinaryOperator.Divide);
            registry[OpCodes.Rem] = i => HandleBinaryOperator(i, BinaryOperator.Remainder);
            registry[OpCodes.Cgt] = i => HandleBinaryOperator(i, BinaryOperator.GreaterThan);
            registry[OpCodes.Cgt_Un] = i => HandleBinaryOperator(i, BinaryOperator.GreaterThan);
            registry[OpCodes.Clt] = i => HandleBinaryOperator(i, BinaryOperator.LessThan);
            registry[OpCodes.Clt_Un] = i => HandleBinaryOperator(i, BinaryOperator.LessThan);
            registry[OpCodes.Ceq] = i => HandleBinaryOperator(i, BinaryOperator.Equal);

            registry[OpCodes.And] = i => HandleBinaryOperator(i, BinaryOperator.And);
            registry[OpCodes.Or] = i => HandleBinaryOperator(i, BinaryOperator.Or);
            registry[OpCodes.Xor] = i => HandleBinaryOperator(i, BinaryOperator.Xor);
            registry[OpCodes.Shl] = i => HandleBinaryOperator(i, BinaryOperator.ShiftLeft);
            registry[OpCodes.Shr] = i => HandleBinaryOperator(i, BinaryOperator.ShiftRight);
            registry[OpCodes.Shr_Un] = i => HandleBinaryOperator(i, BinaryOperator.ShiftRight);

            //conversions            
            registry[OpCodes.Conv_I] = i => HandleConversion(i, typeof(int));
            registry[OpCodes.Conv_I1] = i => HandleConversion(i, typeof(int));
            registry[OpCodes.Conv_I2] = i => HandleConversion(i, typeof(int));
            registry[OpCodes.Conv_I4] = i => HandleConversion(i, typeof(int));
            registry[OpCodes.Conv_I8] = i => HandleConversion(i, typeof(long));            
            registry[OpCodes.Conv_Ovf_I] = i => HandleConversion(i, typeof(int));
            registry[OpCodes.Conv_Ovf_I1] = i => HandleConversion(i, typeof(int));
            registry[OpCodes.Conv_Ovf_I2] = i => HandleConversion(i, typeof(int));
            registry[OpCodes.Conv_Ovf_I4] = i => HandleConversion(i, typeof(int));            
            registry[OpCodes.Conv_Ovf_I8] = i => HandleConversion(i, typeof(long));

            registry[OpCodes.Ldelem_I] = i => HandleLoadNumericArrayElement(i, typeof (int));
            registry[OpCodes.Ldelem_I1] = i => HandleLoadNumericArrayElement(i, typeof (int));
            registry[OpCodes.Ldelem_I2] = i => HandleLoadNumericArrayElement(i, typeof (int));
            registry[OpCodes.Ldelem_I4] = i => HandleLoadNumericArrayElement(i, typeof (int));
            registry[OpCodes.Ldelem_I8] = i => HandleLoadNumericArrayElement(i, typeof (long));
            registry[OpCodes.Ldelem_U1] = i => HandleLoadNumericArrayElement(i, typeof (uint));
            registry[OpCodes.Ldelem_U2] = i => HandleLoadNumericArrayElement(i, typeof (uint));
            registry[OpCodes.Ldelem_U4] = i => HandleLoadNumericArrayElement(i, typeof (uint));            
            registry[OpCodes.Ldelem_R4] = i => HandleLoadNumericArrayElement(i, typeof (float));
            registry[OpCodes.Ldelem_R8] = i => HandleLoadNumericArrayElement(i, typeof (double));
        }

        protected virtual void HandleLoadNumericArrayElement(Instruction instruction, Type type)
        {
            this.HandleUnrecognized(instruction);
        }

        protected virtual void HandleLoadDouble(Instruction instruction, double value)
        {
            this.HandleUnrecognized(instruction);
        }

        protected virtual void HandleConversion(Instruction instruction, Type targetType)
        {
            this.HandleUnrecognized(instruction);
        }

        protected virtual void HandleBinaryOperator(Instruction instruction, BinaryOperator @operator)
        {
            this.HandleUnrecognized(instruction);
        }

        protected virtual void HandleStoreArgument(Instruction instruction, ParameterInfo parameter)
        {
            this.HandleUnrecognized(instruction);
        }

        protected virtual void HandleLoadInt32(Instruction instruction, int constant)
        {
            this.HandleUnrecognized(instruction);
        }

        protected virtual void HandleStoreVariable(Instruction instruction, LocalVariableInfo variable)
        {
            this.HandleUnrecognized(instruction);
        }

        protected virtual void HandleLoadVariable(Instruction instruction, LocalVariableInfo variable)
        {
            this.HandleUnrecognized(instruction);
        }

        protected override void HandleLdarg(Instruction instruction)
        {
            var param = (ParameterInfo) instruction.Operand;            

            if (param.Position == 0 && !this.AnalyzedMethod.IsStatic)
            {
                this.HandleLoadThis(instruction);
            }
            else
            {
                this.HandleLoadArgument(instruction, param);
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
            this.HandleUnrecognized(instruction);
        }

        protected virtual void HandleLoadArgument(Instruction instruction, ParameterInfo parameter)
        {
            this.HandleUnrecognized(instruction);
        }
    }

    public enum BinaryOperator
    {
        Add,
        Subtract,
        Multiply,
        Divide,
        Remainder,
        GreaterThan,
        LessThan,
        Equal,
        And,
        ShiftLeft,
        ShiftRight,
        Or,
        Xor
    }
}