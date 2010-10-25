using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Bickle
{
    public static class SpecDescriber
    {
        public static string DescribeSpec(Expression<Func<bool>> spec)
        {
            if (!(spec.Body is BinaryExpression))
                return "(unknown)";

            var binary = spec.Body as BinaryExpression;
            var left = DescribeExpression(binary.Left);
            var right = DescribeExpression(binary.Right);
            return left + " " + ExtractOperator(binary) + " " + right;
            
        }

        private static string ExtractOperator(BinaryExpression binary)
        {
            string op = Regex.Match(binary.ToString(), " (=|!=|<=|>=|<|>) ").Groups[1].Value;
            
            if (op == "=")
                return "==";

            return op;
        }

        private static string DescribeExpression(Expression exp)
        {
            if (exp is MemberExpression)
            {
                return ((MemberExpression)exp).Member.Name;
            }
            if (exp is ConstantExpression)
            {
                return ((ConstantExpression)exp).Value.ToString();
            }

            return exp.ToString();
        }
    }
}