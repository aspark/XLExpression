using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

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

            //if (a is decimal || b is decimal)
            //{
            //    return decimal.Compare(a.TryToDecimal(), b.TryToDecimal());
            //}

            if(IsNumber(a, out decimal ia) && IsNumber(b, out decimal ib))
            {
                return decimal.Compare(ia, ib);
            }

            return string.Compare(a.ToString(), b.ToString());
        }

        private static Regex _regMatchOp = new Regex("(?<op>[><=]{1,2})(?<val>.+)");

        public static bool IsMatch(this object a, string b)
        {
            var op = "=";
            var m = _regMatchOp.Match(b);
            if (m.Success)
            {
                op = m.Groups["op"].Value;
                b = m.Groups["val"].Value;
            }

            var compare = a.Compare(b);

            switch (op)
            {
                case ">":
                    return compare > 0;
                case ">=":
                    return compare >= 0;
                case "<":
                    return compare < 0;
                case "<=":
                    return compare <= 0;
                default:
                    return compare == 0;
            }
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

            if (obj.TryToNullableInt() == 0)//如果返回null,说明有值，但不能转为数字，也作为true
                return false;

            return true;
        }

        public static bool IsNumber(this object? obj)
        {
            return double.TryParse(obj?.ToString() ?? "", out _);
        }

        public static bool IsNumber(this object? obj, out decimal a)
        {
            a = 0;

            try
            {
                a = Convert.ToDecimal(obj);

                return true;
            }
            catch { }

            return false;
        }

        public static int TryToInt(this object? obj, bool isAllowNaN = true)
        {
            if (obj == null)
                return isAllowNaN ? 0 : throw new ValueError("not allowed null");

            if (obj?.GetType() == typeof(int))
                return (int)obj;

            if (int.TryParse(obj.ToString(), out int result))
                return result;
            else if (!isAllowNaN)
                throw new ArgumentException("not allowed: " + obj);

            return 0;
        }

        public static int? TryToNullableInt(this object? obj)
        {
            if (obj == null)
                return null;

            if (obj?.GetType() == typeof(int))
                return (int)obj;

            if (int.TryParse(obj.ToString(), out int result))
                return result;

            return null;
        }

        public static decimal TryToDecimal(this object? obj, bool isAllowNaN = true)
        {
            if (obj == null)
                return isAllowNaN ? 0 : throw new ValueError("not allowed null");

            if (obj?.GetType() == typeof(decimal))
                return (decimal)obj;

            var str = obj.ToString();

            if(decimal.TryParse(str.TrimEnd('%'), out decimal result))
            {
                if (str.EndsWith('%'))
                {
                    result /= 100;
                }
            }
            else if (!isAllowNaN)
                throw new ArgumentException("not allowed: " + obj);


            return result;
        }

        public static decimal? TryToNullableDecimal(this object? obj)
        {
            if (obj == null)
                return null;

            if (obj?.GetType() == typeof(decimal))
                return (decimal)obj;

            var str = obj.ToString();

            if (decimal.TryParse(str.TrimEnd('%'), out decimal result))
            {
                if (str.EndsWith('%'))
                {
                    result /= 100;
                }

                return result;
            }

            return null;
        }

        public static double TryToDouble(this object? obj, bool isAllowNaN = true)
        {
            if (obj == null)
                return isAllowNaN ? 0 : throw new ValueError("not allowed null");

            if (obj?.GetType() == typeof(double))
                return (double)obj;

            var str = obj.ToString();

            if(double.TryParse(str.TrimEnd('%'), out double result))
            {
                if (str.EndsWith('%'))
                {
                    result /= 100;
                }
            }
            else if (!isAllowNaN)
                throw new ValueError("not allowed: " + obj);

            return result;
        }

        public static R[] Flat<T, R>(this T[,] array, Func<T, R> convert)
        {
            var result = new R[array.Length];
            var col = array.GetLength(1);
            for (var i = 0;i < array.GetLength(0); i++)
            {
                for (var j = 0; j < col; j++)
                {
                    result[i * col + j] = convert(array[i, j]);
                }
            }


            return result;
        }

        public static T[] Flat<T>(this T[,] array)
        {
            return array.Flat(a => a);
        }


        /// <summary>
        /// 将一维数组转为两维
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="colMaxCount">列宽，默认与数组等宽，即：[1, array.Length]</param>
        /// <returns></returns>
        public static T[,] Dimensional<T>(this T[] array, int? colMaxCount = null)
        {
            var colCount = colMaxCount ?? array.Length;
            var rowCount = (int)Math.Ceiling(array.Length * 1.0 / colCount);

            var result = new T[rowCount, colCount];
            for(var i = 0; i < array.Length; i++)
            {
                result[i / colCount, i % colCount] = array[i];
            }

            return result;
        }


        public static void Visit<T>(this T[,] array, Action<T, (int Row, int Col)> callback)
        {
            array.Visit((v, i) =>
            {
                callback(v, i);
                return Enum2DVisitResult.GoOn;
            });
        }

        public static void Visit<T>(this T[,] array, Func<T, (int Row, int Col), Enum2DVisitResult> callback)
        {
            if (array != null)
            {
                for (var i = 0; i < array.GetLength(0); i++)
                {
                    for (var j = 0; j < array.GetLength(1); j++)
                    {
                        switch(callback(array[i, j], (i, j)))
                        {
                            case Enum2DVisitResult.Return:
                                return;
                            case Enum2DVisitResult.GotoNextLine:
                                goto nextLine;
                            default:
                                continue;
                        }
                    }

                nextLine:;
                }
            }
        }
    }

    internal enum Enum2DVisitResult
    {
        GoOn = 0,
        GotoNextLine,
        Return
    }
}
