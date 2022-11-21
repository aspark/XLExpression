using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
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
    public interface IFunctionDataContext
    {
        object? this[string name] { get; }

        object? this[int row, int col] { get; }
    }

    public class FunctionDataContext : IFunctionDataContext
    {
        ConcurrentDictionary<int, FunctionDataRow> _rows = new ConcurrentDictionary<int, FunctionDataRow>();

        public FunctionDataContext(FunctionDataRow row)
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

        public FunctionDataContext(List<FunctionDataRow> rows)
        {
            for(var i = 0; i < rows.Count; i++)
            {
                _rows[i] = rows[i];
            }
        }

        public FunctionDataRow this[int row]
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

        public object? this[string name]
        {
            get 
            {
                if (name.Contains(':'))//引用的是区域/is range A2:C5
                {
                    var range = name.Split(':').Select(ExcelHelper.ConvertNameToPosition).ToArray();
                    var rowCount = range[1].row - range[0].row;//行数
                    var colCount = range[1].col - range[0].col;//列数
                    var datas = new object?[rowCount, colCount];
                    for(var rowIndex = 0; rowIndex < rowCount; rowIndex++)
                    {
                        var row = this[range[0].row + rowIndex];
                        for (var colIndex = 0; colIndex < colCount; colIndex++)
                        {
                            datas[rowIndex, colIndex] = row?[range[0].col + colIndex];
                        }
                    }

                    return datas;
                }
                else
                {
                    var index = ExcelHelper.ConvertNameToPosition(name);

                    return this[index.row, index.col];
                }
            }
            set
            {
                if (name.Contains(':'))//引用的是区域/is range A2:C5
                {
                    var range = name.Split(':').Select(ExcelHelper.ConvertNameToPosition).ToArray();
                    var rowCount = range[1].row - range[0].row;//行数
                    var colCount = range[1].col - range[0].col;//列数
                    for (var rowIndex = 0; rowIndex < rowCount; rowIndex++)
                    {
                        var row = this[range[0].row + rowIndex];
                        for (var colIndex = 0; colIndex < colCount; colIndex++)
                        {
                            row[range[0].col + colIndex] = value;
                        }
                    }
                }
                else
                {
                    var index = ExcelHelper.ConvertNameToPosition(name);

                    this[index.row, index.col] = value;
                }
            }
        }

    }

    /// <summary>
    /// 行数据，会保留添加列的顺序
    /// </summary>
    public class FunctionDataRow
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

        public bool Add(string header, object data)
        {
            var index = ExcelHelper.ConvertNameToPosition(header);
            return _data.TryAdd(index.col, data);
        }

        public bool Remove(string header)
        {
            var index = ExcelHelper.ConvertNameToPosition(header);
            return _data.TryRemove(index.col, out _);
        }

        public object? this[string name]
        {
            get
            {
                var index = ExcelHelper.ConvertNameToPosition(name);

                return this[index.col];
            }
            set
            {
                var index = ExcelHelper.ConvertNameToPosition(name);

                this[index.col] = value;
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
                _data.GetOrAdd(index, (i) => value);
            }
        }
    }
}
