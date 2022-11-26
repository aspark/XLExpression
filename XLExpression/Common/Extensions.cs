using System;
using System.Collections.Generic;
using System.Text;

namespace XLExpression
{
    internal static class Extensions
    {
        public static V GetOrAdd<K, V>(this IDictionary<K, V> dic, K key, Func<V> value)
        {
            if (!dic.ContainsKey(key))
            {
                dic[key] = value();
            }

            return dic[key];
        }

        public static int? Compare(this object a, object b)
        {
            if (a == null && b == null)
                return 0;

            if (a == null)
                return -1;

            if (b == null)
                return 1;

            if (a is DateTime || b is DateTime)
            {
                var at = a is DateTime ? (DateTime)a : DateTime.Parse(a.ToString());
                var bt = b is DateTime ? (DateTime)b : DateTime.Parse(b.ToString());

                return DateTime.Compare(at, bt);
            }

            //if(a is null)

            if (a is decimal || b is decimal)
            {
                return decimal.Compare(a.TryToDecimal(), b.TryToDecimal());
            }

            return string.Compare(a.ToString(), b.ToString());
        }

        public static string ToStringOrEmpty(this object? obj)
        {
            return obj?.ToString() ?? "";
        }

        public static bool TryToBool(this object? obj)
        {
            if (obj == null)
                return false;

            if (obj is bool)
                return (bool)obj;

            if (obj.TryToNullableInt() == 0)//不能转为数字说明有值，也为true
                return false;

            return true;
        }

        public static int TryToInt(this object? obj)
        {
            if (obj == null)
                return 0;

            if (obj.GetType() == typeof(int))
                return (int)obj;

            if (int.TryParse(obj.ToString(), out int result))
                return result;

            return 0;
        }

        public static int? TryToNullableInt(this object? obj)
        {
            if (obj == null)
                return null;

            if (obj.GetType() == typeof(int))
                return (int)obj;

            if (int.TryParse(obj.ToString(), out int result))
                return result;

            return null;
        }

        public static decimal TryToDecimal(this object? obj)
        {
            if (obj == null)
                return 0;

            if (obj.GetType() == typeof(decimal))
                return (decimal)obj;

            decimal.TryParse(obj.ToString(), out decimal result);

            return result;
        }

        public static double TryToDouble(this object? obj)
        {
            if (obj == null)
                return 0;

            if (obj.GetType() == typeof(double))
                return (double)obj;

            double.TryParse(obj.ToString(), out double result);

            return result;
        }
    }
}
