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
            return ExtractMemberInfo<FieldInfo>(fieldAccess.Body);
        }

        public static PropertyInfo PropertyOf<T>(Expression<Func<T, object>> propertyAccess)
        {
            return ExtractMemberInfo<PropertyInfo>(propertyAccess.Body);
        }

        private static TMemberInfo ExtractMemberInfo<TMemberInfo>(Expression body)
            where TMemberInfo : MemberInfo
        {
            var memberExpression = (MemberExpression)body;

            return (TMemberInfo)memberExpression.Member;
        }
    }
}
