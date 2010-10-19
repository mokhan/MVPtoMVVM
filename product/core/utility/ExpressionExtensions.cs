using System;
using System.Linq.Expressions;
using System.Reflection;

namespace MVPtoMVVM.utility
{
    public static class ExpressionExtensions
    {
        public static PropertyInfo pick_property<T>(this Expression<Func<T, object>> expression)
        {
            return (PropertyInfo) member_expression(expression).Member;
        }

        static MemberExpression member_expression<T>(Expression<Func<T, object>> expression)
        {
            if (expression.Body.NodeType == ExpressionType.Convert)
                return ((UnaryExpression) expression.Body).Operand as MemberExpression;
            if (expression.Body.NodeType == ExpressionType.MemberAccess)
                return expression.Body as MemberExpression;
            throw new NotImplementedException();
        }
    }
}