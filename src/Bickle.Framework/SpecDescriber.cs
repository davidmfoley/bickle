using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Bickle
{
    public static class SpecDescriber
    {
        public static string DescribeSpec(Expression<Func<bool>> spec)
        {
            if (!(spec.Body is BinaryExpression))
                return "(no more info)";

            var binary = spec.Body as BinaryExpression;
            var left = DescribeExpression(binary.Left);
            var right = DescribeExpression(binary.Right);
            return left + " " + ExtractOperator(binary) + " " + right;            
        }

        public static string DescribeFailure(Expression<Func<bool>> spec)
        {
            var binary = spec.Body as BinaryExpression;

            if (binary == null)
                return spec.ToString();

            var description = "Expected: " + DescribeSpec(spec);

            var left = DescribeValue(binary.Left);
            var right = DescribeValue(binary.Right);

            if (!string.IsNullOrEmpty(left))
            {
                description += ", " + left;
            }

            if (!string.IsNullOrEmpty(right))
            {
                description += ", " + right;
            }

            return description;
        }

        private static string DescribeValue(Expression exp)
        {
            if (exp is MemberExpression)
            {
                var memberExpression = (MemberExpression) exp;
                var daddy = memberExpression.Expression;
                
                if (daddy is ConstantExpression)
                {
                    var holder = ((ConstantExpression) daddy).Value;

                    var memberInfo = memberExpression.Member;
                    return memberInfo.Name + " was " +  Evaluate(holder, memberInfo);                   
                }                
            }

            return "";
        }

        private static string Evaluate(object holder, MemberInfo member)
        {
            if (member is FieldInfo)
            {
                return ((FieldInfo) member).GetValue(holder).ToString();
            }
            if (member is PropertyInfo)
            {
                return ((PropertyInfo) member).GetValue(holder, null).ToString();
            }
            return member.ToString();
        }

        private static string ExtractOperator(BinaryExpression binary)
        {
            string op = Regex.Match(binary.ToString(), " (=|!=|<=|>=|<|>) ").Groups[1].Value;
            
            if (op == "=")
                return "should equal";
            if (op == "!=")
                return "should not equal";
            if (op == "<=")
                return "should be less than or equal to";
            if (op == "<")
                return "should be less than";
            if (op == ">=")
                return "should be greater than or equal to";
            if (op == ">")
                return "should be greater than";

            return "should " + op;
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