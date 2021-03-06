﻿using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Mono.Reflection;

namespace CodeModel.FlowAnalysis
{
    public static class InstructionExtensions
    {
        public static readonly OpCode[] StoreVariableOpCodes = {OpCodes.Stloc, OpCodes.Stloc_0, OpCodes.Stloc_1, OpCodes.Stloc_2, OpCodes.Stloc_3, OpCodes.Stloc_S};
        public static readonly OpCode[] LoadVariableOpCodes = {OpCodes.Ldloc, OpCodes.Ldloc_0, OpCodes.Ldloc_1, OpCodes.Ldloc_2, OpCodes.Ldloc_3, OpCodes.Ldloc_S};
        public static readonly OpCode[] CallOpCodes = new[] {OpCodes.Call, OpCodes.Calli, OpCodes.Callvirt};

        public static int PushedValuesCount(this Instruction instruction, MethodInfo analyzedMethod, MethodBody methodBody)
        {
            int pushedCount = instruction.PushedValuesCountByOpCode();

            if (methodBody.ExceptionHandlingClauses.Any(x => x.BeginsOn(instruction) && x.IsCatch()))
            {
                pushedCount++;
            }

            return pushedCount;
        }

        public static int PushedValuesCountByOpCode(this Instruction instruction)
        {
            switch (instruction.OpCode.StackBehaviourPush)
            {
                case StackBehaviour.Push0:
                    return 0;
                case StackBehaviour.Push1_push1:
                    return 2;
                case StackBehaviour.Push1:
                case StackBehaviour.Pushi:
                case StackBehaviour.Pushi8:
                case StackBehaviour.Pushr4:
                case StackBehaviour.Pushr8:
                case StackBehaviour.Pushref:
                    return 1;
                case StackBehaviour.Varpush:
                    if (instruction.OpCode == OpCodes.Call || instruction.OpCode == OpCodes.Callvirt)
                    {
                        var method = (MethodBase)instruction.Operand;
                        if (method.IsConstructor || ((MethodInfo)method).ReturnType == typeof(void))
                        {
                            return 0;
                        }
                        else
                        {
                            return 1;
                        }
                    }

                    if (instruction.OpCode == OpCodes.Calli)
                    {
                        var b = (byte[])instruction.Operand;
                        var returnType = b[2];

                        if (returnType == 0x1)
                        {
                            return 0;
                        }
                        else
                        {
                            return 1;
                        }                        
                    }

                    throw new InvalidOperationException("Cannot determine varpush for instruction " + instruction.ToString());
                default:
                    throw new InvalidOperationException("(Push) Not implemented " + instruction.ToString());
            }
        }

        public static int PopedValuesCount(this Instruction instruction, MethodInfo analyzedMethod)
        {
            switch (instruction.OpCode.StackBehaviourPop)
            {
                case StackBehaviour.Pop0:
                    return 0;

                case StackBehaviour.Pop1:
                case StackBehaviour.Popi:
                case StackBehaviour.Popref:
                    return 1;
                case StackBehaviour.Popi_popi_popi:
                case StackBehaviour.Popref_popi_popi:
                case StackBehaviour.Popref_popi_pop1:
                case StackBehaviour.Popref_popi_popi8:
                case StackBehaviour.Popref_popi_popr4:
                case StackBehaviour.Popref_popi_popr8:
                case StackBehaviour.Popref_popi_popref:
                    return 3;
                case StackBehaviour.Pop1_pop1:
                case StackBehaviour.Popi_pop1:
                case StackBehaviour.Popi_popi:
                case StackBehaviour.Popi_popi8:
                case StackBehaviour.Popi_popr4:
                case StackBehaviour.Popi_popr8:
                case StackBehaviour.Popref_pop1:
                case StackBehaviour.Popref_popi:
                    return 2;
                case StackBehaviour.Varpop:
                    if (instruction.OpCode == OpCodes.Initobj || instruction.OpCode == OpCodes.Newobj)
                    {
                        var method = ((ConstructorInfo)instruction.Operand);
                        return method.GetParameters().Count();
                    }

                    if (instruction.OpCode == OpCodes.Call || instruction.OpCode == OpCodes.Callvirt)
                    {
                        var method = ((MethodBase)instruction.Operand);
                        var parametersCount = method.GetParameters().Count();

                        if (!method.IsStatic)
                        {
                            parametersCount++;
                        }

                        return parametersCount;
                    }

                    if (instruction.OpCode == OpCodes.Ret)
                    {
                        if (analyzedMethod.ReturnType == typeof(void))
                        {
                            return 0;
                        }
                        else
                        {
                            return 1;
                        }
                    }

                    if (instruction.OpCode == OpCodes.Calli)
                    {                       
                        var b = (byte[]) instruction.Operand;
                        
                        var parameterCount = b[1];
                        
                        return parameterCount + 1;                        
                    }

                    throw new InvalidOperationException("Cannot determine varpop for instruction " + instruction.ToString());
                default:
                    throw new InvalidOperationException("(Pop: " + instruction.OpCode.StackBehaviourPop + ") Not implemented " + instruction.ToString());
            }
        }

        public static bool IsStoreVariable(this OpCode opCode)
        {
            return StoreVariableOpCodes.Contains(opCode);
        }

        public static bool IsCall(this Instruction instruction)
        {
            return instruction.OpCode.IsCall();
        }

        public static bool IsCall(this OpCode opCode)
        {
            return CallOpCodes.Contains(opCode);
        }
    }
}