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
    public interface IFunctionDataContext
    {
        object? this[string name] { get; }

        object? this[int row, int col] { get; }
    }

    public class FunctionDataContext : IFunctionDataContext
    {
        ConcurrentDictionary<int, FunctionDataRow> _rows = new ConcurrentDictionary<int, FunctionDataRow>();

        public FunctionDataContext()
        {

        }

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

        public int RowCount => _rows.Any() ? _rows.Max(m => m.Key) + 1 : 0;

        public int ColCount => _rows.Max(m => m.Value.ColCount);

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
                    var rowCount = range[1].row.HasValue && range[0].row.HasValue ? range[1].row.Value - range[0].row.Value + 1 : RowCount;//行数
                    var colCount = range[1].col.HasValue && range[0].col.HasValue ? range[1].col.Value - range[0].col.Value + 1 : ColCount;//列数
                    var datas = new object?[rowCount, colCount];
                    var rowTotalCount = datas.GetLength(0);
                    var colTotalCount = datas.GetLength(1);
                    for (var rowIndex = 0; rowIndex < rowCount; rowIndex++)
                    {
                        var row = this[(range[0].row ?? 0) + rowIndex];
                        for (var colIndex = 0; colIndex < colCount; colIndex++)
                        {
                            datas[rowIndex, colIndex] = row?[(range[0].col ?? 0) + colIndex];
                        }
                    }

                    return datas;
                }
                else
                {
                    var index = ExcelHelper.ConvertNameToPosition(name);
                    if(index.row.HasValue && index.col.HasValue)
                    {
                        return this[index.row.Value, index.col.Value];
                    }
                    else if (index.row.HasValue)
                    {
                        var row = this[index.row.Value];

                        var allData = row.AllData;

                        var rowData = new object?[0, allData.Length];

                        for(var i = 0; i < allData.Length; i++)
                        {
                            rowData[0, i] = allData[i];
                        }

                        return rowData;
                        
                    }
                    else if (index.col.HasValue)
                    {
                        var colData = new object?[RowCount, 1];
                        for (var rowIndex = 0; rowIndex < colData.GetLength(0); rowIndex++)
                        {
                            colData[rowIndex, 0] = this[rowIndex]?[index.col.Value];
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
                    var rowCount = range[1].row.HasValue && range[0].row.HasValue ? range[1].row - range[0].row + 1 : this.RowCount;//行数
                    var colCount = range[1].col.HasValue && range[0].col.HasValue ? range[1].col - range[0].col + 1 : this.ColCount;//列数
                    for (var rowIndex = 0; rowIndex < rowCount; rowIndex++)
                    {
                        var row = this[(range[0].row??0) + rowIndex];
                        for (var colIndex = 0; colIndex < colCount; colIndex++)
                        {
                            row[(range[0].col ?? 0) + colIndex] = value;
                        }
                    }
                }
                else
                {
                    var index = ExcelHelper.ConvertNameToPosition(name);
                    if(index.row.HasValue && index.col.HasValue)
                    {
                        this[index.row.Value, index.col.Value] = value;
                    }
                    else if (index.row.HasValue)
                    {
                        var row = this[index.row.Value];
                        for (var colIndex = 0; colIndex < ColCount; colIndex++)
                        {
                            row[colIndex] = value;
                        }
                    }
                    else if (index.col.HasValue)
                    {
                        for (var rowIndex = 0; rowIndex < RowCount; rowIndex++)
                        {
                            this[rowIndex][index.col.Value] = value;
                        }
                    }
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

        public int ColCount { get => _data.Any() ? _data.Max(k => k.Key) + 1 : 0 ; }

        //public bool Add(string header, object data)
        //{
        //    var index = ExcelHelper.ConvertNameToPosition(header);
        //    if(index.col.HasValue)
        //        return _data.TryAdd(index.col.Value, data);

        //    for (var i = 0; i < this.ColCount; i++){
        //        _data.TryAdd(i, data);
        //    }

        //    return true;
        //}

        //public bool Remove(string header)
        //{
        //    var index = ExcelHelper.ConvertNameToPosition(header);

        //    if(index.col.HasValue)
        //        return _data.TryRemove(index.col.Value, out _);

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

                if(index.col.HasValue)
                    return this[index.col.Value];

                return AllData;
            }
            set
            {
                var index = ExcelHelper.ConvertNameToPosition(name);
                
                this[index.col.Value] = value;
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
