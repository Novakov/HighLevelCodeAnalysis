using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    internal static class Get
    {
        public static FieldInfo FieldOf<T>(Expression<Func<T, object>> fieldAccess)
        {
            var memberExpression = (MemberExpression)fieldAccess.Body;

            return (FieldInfo) memberExpression.Member;
        }
    }
}
