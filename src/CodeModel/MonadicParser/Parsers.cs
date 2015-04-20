using System.Linq;

namespace CodeModel.MonadicParser
{
    public delegate Result<TInput, TValue> Parser<TInput, TValue>(TInput input);

    public class Result<TInput, TValue>
    {
        public TInput Rest { get; private set; }
        public TValue Value { get; private set; }

        public Result(TInput rest, TValue value)
        {
            Rest = rest;
            Value = value;
        }
    }

    public class Parsers
    {
        public static Parser<string, char> AnyChar()
        {
            return input =>
            {
                if (input.Length > 0)
                {
                    return Result(input[0], input.Substring(1));
                }
                else
                {
                    return null;
                }
            };
        }

        public static Parser<string, char> Char(char expectedChar)
        {
            return input =>
            {
                if (input.Length == 0) return null;
                if (input[0] != expectedChar) return null;
                return Result(expectedChar, input.Substring(1));
            };
        }

        public static Result<TInput, TValue> Result<TInput, TValue>(TValue value, TInput rest)
        {
            return new Result<TInput, TValue>(rest, value);
        }

        public static Parser<TInput, TValue> AnyOf<TInput, TValue>(params Parser<TInput, TValue>[] parsers)
        {
            return input =>
            {
                var result = parsers.Select(x => x(input)).FirstOrDefault(x => x != null);
                return result;
            };
        }
    }
}