using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using XLExpression.Common;

namespace XLExpression.Functions
{
    /// <summary>
    /// 调用上下文
    /// </summary>
    internal interface IInvokeContext
    {
        void SetCache(string key, object value);

        object? GetCache(string key);

        void MapResultToRange(object? result, string range);

        string? GetResultRange(object? result);
    }

    internal class DefaultInvokeContext : IInvokeContext, IDisposable
    {
        public DefaultInvokeContext()
        {
            _context.Value = this;
        }

        private static AsyncLocal<IInvokeContext> _context = new AsyncLocal<IInvokeContext>();

        public static IInvokeContext Current => _context.Value;

        private ConcurrentDictionary<string, object> _valueMapper = new ConcurrentDictionary<string, object>();
        public void SetCache(string key, object value)
        {
            _valueMapper.TryAdd(key, value);
        }

        public object? GetCache(string key)
        {
            _valueMapper.TryGetValue(key, out var value);

            return value;
        }

        public void Dispose()
        {
            _context.Value = null;
        }

        public void MapResultToRange(object? result, string range)
        {
            if (result == null)
                return;

            SetCache("range." + result.GetHashCode(), range);
        }

        public string? GetResultRange(object? result)
        {
            if (result == null)
                return null;

            return GetCache("range." + result.GetHashCode())?.ToString();
        }
    }
}
