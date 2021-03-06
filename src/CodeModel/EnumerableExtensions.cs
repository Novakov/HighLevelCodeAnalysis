﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeModel.Graphs;

namespace CodeModel
{
    static class EnumerableExtensions
    {
        public static IEnumerable<T> OfType<T>(this IEnumerable<T> @this, Type expectedType)
        {
            return @this.Where(x => expectedType.IsInstanceOfType(x));
        }

        public static IEnumerable<T> Except<T>(this IEnumerable<T> @this, params T[] elements)
        {
            return @this.Except((IEnumerable<T>) elements);
        }
    }
}
