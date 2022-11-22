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
