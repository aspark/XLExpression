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
using XLExpression.Common;

namespace XLExpression.Functions
{
    /// <summary>
    /// 数据平面
    /// </summary>
    public interface IDataContext
    {
        object? this[string name] { get; }

        object? this[int row, int col] { get; }

        int RowCount { get; }

        int ColCount { get; }

        object?[,] this[int rowStart, int rowCount, int colStart, int colCount] { get; }
    }

    public class FunctionDataContext : IDataContext
    {
        ConcurrentDictionary<int, FunctionDataRow> _rows = new ConcurrentDictionary<int, FunctionDataRow>();

        public FunctionDataContext()
        {

        }

        internal FunctionDataContext(FunctionDataRow row)
        {
            _rows[0] = row;
        }

        public FunctionDataContext(IDictionary<string, object> values)
        {
            //_rows[0] = new FunctionDataRow(values);
            foreach(var pair in values)
            {
                this[pair.Key] = pair.Value;
            }
        }

        internal FunctionDataContext(List<FunctionDataRow> rows)
        {
            for(var i = 0; i < rows.Count; i++)
            {
                _rows[i] = rows[i];
            }
        }

        public int RowCount => _rows.Any() ? _rows.Max(m => m.Key) + 1 : 0;

        public int ColCount => _rows.Max(m => m.Value.ColCount);

        internal FunctionDataRow this[int row]
        {
            get
            {
                return _rows.GetOrAdd(row, () => new FunctionDataRow());
            }
        }

        public object? this[int row, int col]
        {
            get
            {
                var rowData = this[row];
                if(rowData != null)
                {
                    return rowData[col];
                }

                return null;
            }
            set
            {
                var rowData = _rows.GetOrAdd(row, (k) => new FunctionDataRow());
                rowData[col] = value;
            }
        }

        public object?[,] this[int rowStart, int rowCount, int colStart, int colCount]
        {
            get
            {
                var datas = new object?[rowCount, colCount];
                for (var rowIndex = 0; rowIndex < rowCount; rowIndex++)
                {
                    var row = this[rowStart + rowIndex];
                    for (var colIndex = 0; colIndex < colCount; colIndex++)
                    {
                        datas[rowIndex, colIndex] = row?[colStart + colIndex];
                    }
                }

                return datas;
            }
        }

        public object? this[string name]
        {
            get 
            {
                if (name.Contains(':'))//引用的是区域/is range A2:C5
                {
                    var range = name.Split(':').Select(ExcelHelper.ConvertNameToPosition).ToArray();
                    var rowCount = range[1].Row.HasValue && range[0].Row.HasValue ? range[1].Row.Value - range[0].Row.Value + 1 : RowCount;//行数
                    var colCount = range[1].Col.HasValue && range[0].Col.HasValue ? range[1].Col.Value - range[0].Col.Value + 1 : ColCount;//列数

                    return this[range[0].Row ?? 0, rowCount, range[0].Col ?? 0, colCount];
                }
                else
                {
                    var index = ExcelHelper.ConvertNameToPosition(name);
                    if(index.Row.HasValue && index.Col.HasValue)
                    {
                        return this[index.Row.Value, index.Col.Value];
                    }
                    else if (index.Row.HasValue)
                    {
                        var row = this[index.Row.Value];

                        var allData = row.AllData;

                        return allData.Dimensional();
                        
                    }
                    else if (index.Col.HasValue)
                    {
                        var colData = new object?[RowCount, 1];
                        for (var rowIndex = 0; rowIndex < colData.GetLength(0); rowIndex++)
                        {
                            colData[rowIndex, 0] = this[rowIndex]?[index.Col.Value];
                        }
                    }

                    return null;
                }
            }
            set
            {
                if (name.Contains(':'))//引用的是区域/is range A2:C5
                {
                    var range = name.Split(':').Select(ExcelHelper.ConvertNameToPosition).ToArray();
                    var rowCount = range[1].Row.HasValue && range[0].Row.HasValue ? range[1].Row.Value - range[0].Row.Value + 1 : this.RowCount;//行数
                    var colCount = range[1].Col.HasValue && range[0].Col.HasValue ? range[1].Col.Value - range[0].Col.Value + 1 : this.ColCount;//列数

                    var rowStart = range[0].Row ?? 0;
                    var colStart = range[0].Col ?? 0;

                    for (var rowIndex = 0; rowIndex < rowCount; rowIndex++)
                    {
                        var row = this[rowStart + rowIndex];
                        for (var colIndex = 0; colIndex < colCount; colIndex++)
                        {
                            row[colStart + colIndex] = value;
                        }
                    }
                }
                else
                {
                    var index = ExcelHelper.ConvertNameToPosition(name);
                    if(index.Row.HasValue && index.Col.HasValue)
                    {
                        this[index.Row.Value, index.Col.Value] = value;
                    }
                    else if (index.Row.HasValue)
                    {
                        var row = this[index.Row.Value];
                        for (var colIndex = 0; colIndex < ColCount; colIndex++)
                        {
                            row[colIndex] = value;
                        }
                    }
                    else if (index.Col.HasValue)
                    {
                        for (var rowIndex = 0; rowIndex < RowCount; rowIndex++)
                        {
                            this[rowIndex][index.Col.Value] = value;
                        }
                    }
                }
            }
        }

    }

    /// <summary>
    /// 行数据，会保留添加列的顺序
    /// </summary>
    internal class FunctionDataRow
    {
        //public FunctionDataRow(IDictionary<string, object> values)
        //{
        //    if (values != null)
        //    {
        //        foreach (var pair in values)
        //        {
        //            Add(pair.Key, pair.Value);
        //        }
        //    }
        //}

        private ConcurrentDictionary<int, object?> _data = new ConcurrentDictionary<int, object?>();

        public int ColCount { get => _data.Any() ? _data.Max(k => k.Key) + 1 : 0 ; }

        //public bool Add(string header, object data)
        //{
        //    var index = ExcelHelper.ConvertNameToPosition(header);
        //    if(index.Col.HasValue)
        //        return _data.TryAdd(index.Col.Value, data);

        //    for (var i = 0; i < this.ColCount; i++){
        //        _data.TryAdd(i, data);
        //    }

        //    return true;
        //}

        //public bool Remove(string header)
        //{
        //    var index = ExcelHelper.ConvertNameToPosition(header);

        //    if(index.Col.HasValue)
        //        return _data.TryRemove(index.Col.Value, out _);

        //    _data.Clear();

        //    return true;
        //}

        //public object[] ToArray()
        //{
        //    return 
        //}

        public object?[] AllData
        {
            get
            {
                var rowData = new object?[this.ColCount];
                for (var colIndex = 0; colIndex < rowData.Length; colIndex++)
                {
                    rowData[colIndex] = this[colIndex];
                }

                return rowData;
            }
        }

        public object? this[string name]
        {
            get
            {
                var index = ExcelHelper.ConvertNameToPosition(name);

                if(index.Col.HasValue)
                    return this[index.Col.Value];

                return AllData;
            }
            set
            {
                var index = ExcelHelper.ConvertNameToPosition(name);
                
                this[index.Col.Value] = value;
            }
        }

        public object? this[int index]
        {
            get
            {
                if(_data.ContainsKey(index))
                    return _data[index];

                return null;
            }
            set
            {
                _data.AddOrUpdate(index, (i) => value, (i, o) => value);
            }
        }
    }
}
