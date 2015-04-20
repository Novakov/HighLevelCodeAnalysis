using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeModel.MonadicParser
{
    public static class ParserCombinators
    {
        public static Parser<TInput, TResult> Select<TInput, TValue, TResult>(this Parser<TInput, TValue> parser, Func<TValue, TResult> projection)
        {
            return input =>
            {
                var parseResult = parser(input);

                if (parseResult == null)
                {
                    return null;
                }
                else
                {
                    return Parsers.Result(projection(parseResult.Value), parseResult.Rest);
                }
            };
        }

        public static Parser<TInput, TResult> SelectMany<TInput, TValue1, TValue2, TResult>(this Parser<TInput, TValue1> first, Func<TValue1, Parser<TInput, TValue2>> second, Func<TValue1, TValue2, TResult> projection)
        {
            return input =>
            {
                var firstResult = first(input);

                if (firstResult == null) return null;

                var secondResult = second(firstResult.Value)(firstResult.Rest);

                if (secondResult == null) return null;

                return Parsers.Result(projection(firstResult.Value, secondResult.Value), secondResult.Rest);
            };
        }

        public static Parser<TInput, IEnumerable<TValue>> Many<TInput, TValue>(this Parser<TInput, TValue> parser)
        {
            return input =>
            {
                var values = new List<TValue>();
                TInput rest = input;

                Result<TInput, TValue> result;

                do
                {
                    result = parser(rest);
                    if (result != null)
                    {
                        rest = result.Rest;
                        values.Add(result.Value);
                    }

                } while (result != null);

                return Parsers.Result(values.AsEnumerable(), rest);
            };
        }

        public static Parser<TInput, IEnumerable<TValue>> Reverse<TInput, TValue>(this Parser<TInput, IEnumerable<TValue>> parser)
        {
            return parser.Wrap((v, r) => Parsers.Result(v.Reverse(), r));
        }

        public static Parser<TInput, TResult> Wrap<TInput, TValue, TResult>(this Parser<TInput, TValue> parser, Func<TValue, TInput, Result<TInput, TResult>> wrap)
        {
            return input =>
            {
                var result = parser(input);

                if (result == null)
                {
                    return null;
                }

                return wrap(result.Value, result.Rest);
            };
        }

        public static Parser<TInput, TValue> Optional<TInput, TValue>(this Parser<TInput, TValue> parser, TValue defaultValue = default(TValue))
        {
            return input =>
            {
                var result = parser(input);

                if (result == null) return Parsers.Result(defaultValue, input);

                return result;
            };
        }
    }    
}
