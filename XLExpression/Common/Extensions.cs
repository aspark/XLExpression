using System;
using System.Collections.Generic;
using System.Text;

namespace XLExpression.Common
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
    }
}
