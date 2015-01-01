using System.Reflection;
using Mono.Reflection;

namespace CodeModel.FlowAnalysis
{
    public static class ExceptionHandlingClauseExtensions
    {
        public static bool BeginsOn(this ExceptionHandlingClause clause, Instruction instruction)
        {
            return clause.HandlerOffset == instruction.Offset;
        }

        public static bool IsFinally(this ExceptionHandlingClause clause)
        {
            return clause.Flags.HasFlag(ExceptionHandlingClauseOptions.Finally);
        }

        public static bool IsFault(this ExceptionHandlingClause clause)
        {
            return clause.Flags.HasFlag(ExceptionHandlingClauseOptions.Fault);
        }

        public static bool IsCatch(this ExceptionHandlingClause clause)
        {
            return !clause.IsFinally() && !clause.IsFault();
        }
    }
}