using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using CodeModel.FlowAnalysis;
using CodeModel.MonadicParser;
using Mono.Reflection;

namespace Tests.Extensions.Nancy
{
    public static class IlParser
    {
        public static Parser<IEnumerable<Instruction>, Instruction> OpCode(OpCode opcode)
        {
            return input =>
            {
                var l = input.ToList();
                if (l.Count >= 1 && l[0].OpCode == opcode)
                {
                    return Parsers.Result(l[0], l.Skip(1));
                }
                return null;
            };
        }

        public static Parser<IEnumerable<Instruction>, Instruction> Nop()
        {
            return OpCode(OpCodes.Nop);
        }

        public static Parser<IEnumerable<Instruction>, Instruction> Any()
        {
            return input =>
            {
                var l = input.ToList();
                if (l.Count >= 1)
                {
                    return Parsers.Result(l[0], l.Skip(1));
                }
                return null;
            };
        }

        public static Parser<IEnumerable<Instruction>, Instruction> LoadLocal()
        {
            return AnyOfOpcode(Parsers.AnyOf(InstructionExtensions.LoadVariableOpCodes.Select(IlParser.OpCode).ToArray()));
        }

        public static Parser<IEnumerable<Instruction>, Instruction> StoreLocal()
        {
            return AnyOfOpcode(Parsers.AnyOf(InstructionExtensions.StoreVariableOpCodes.Select(IlParser.OpCode).ToArray()));
        }

        public static Parser<IEnumerable<Instruction>, Instruction> AnyOfOpcode(Parser<IEnumerable<Instruction>, Instruction> opcodes)
        {
            return opcodes;
        }
    }
}