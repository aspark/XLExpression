using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using XLExpression.Common;
using XLExpression.Excel.Model;
using XLExpression.Functions;

namespace XLExpression.Excel
{
    internal class ExcelDataContext: IDataContext, IDisposable
    {
        private ExcelModel _excel = null;

        private SheetModel _defaultSheet = null;

        public ExcelDataContext(string fileName)
        {
            var _excel = new ExcelModel(fileName);
        }

        public ExcelDataContext(ExcelModel excel)
        {
            _excel = excel;
                throw new ArgumentNullException("cannot find sheet");
        }

        public ExcelDataContext(ExcelModel excel, int defaultSheetIndex)
        {
            _excel = excel;

            if (defaultSheetIndex > _excel.Sheets.Count)
                throw new IndexOutOfRangeException("sheetIndex");

            _defaultSheet = _excel.Sheets[defaultSheetIndex];
        }

        private SheetModel GetSheet()
        {
            return GetSheet(null, null);
        }

        private Dictionary<string, SheetModel> _sheetNameMapper = new Dictionary<string, SheetModel>();
        private SheetModel GetSheet(string? file, string? sheetName)
        {
            if (!string.IsNullOrWhiteSpace(file))
            {
                throw new NotSupportedException("not support excel file reference");
            }

            if (!string.IsNullOrEmpty(sheetName))
            {
                return _sheetNameMapper.GetOrAdd(sheetName, () => {
                    return _excel.Sheets.First(s => string.Equals(s.Name, sheetName, StringComparison.InvariantCultureIgnoreCase));//必须要有一个匹配的
                });
            }

            if(_defaultSheet == null)
            {
                _defaultSheet = _excel.Sheets.FirstOrDefault(s => s.IsSelected);
                if (_defaultSheet == null)
                {
                    _defaultSheet = _excel.Sheets.FirstOrDefault();
                }

                if (_defaultSheet == null)
                    throw new ArgumentNullException("cannot find sheet");
            }

            return _defaultSheet;
        }

        public object? this[string name]
        {
            get
            {
                if (name.Contains(':'))//引用的是区域/is range A2:C5
                {
                    var range = name.Split(':').Select(ExcelHelper.ConvertNameToPosition).ToArray();
                    var rowCount = range[1].Row.HasValue && range[0].Row.HasValue ? range[1].Row!.Value - range[0].Row!.Value + 1 : RowCount;//行数
                    var colCount = range[1].Col.HasValue && range[0].Col.HasValue ? range[1].Col!.Value - range[0].Col!.Value + 1 : ColCount;//列数

                    return this[range[0], rowCount, colCount];
                }
                else
                {
                    var index = ExcelHelper.ConvertNameToPosition(name);

                    return this[index];
                }
            }
        }

        public object? this[ExcelCellPostion position]
        {
            get
            {
                if (position.Row.HasValue && position.Col.HasValue)
                {
                    return GetSheet(position.File, position.SheetName).Rows[position.Row.Value]?.Cells[position.Col.Value]?.Value;
                }
                else if (position.Row.HasValue)
                {
                    var row = GetSheet(position.File, position.SheetName).Rows[position.Row.Value];
                    if (row != null)
                    {
                        var allData = row.Cells;

                        return allData.Dimensional();
                    }
                }
                else if (position.Col.HasValue)
                {
                    var colData = new object?[RowCount, 1];
                    for (var rowIndex = 0; rowIndex < colData.GetLength(0); rowIndex++)
                    {
                        colData[rowIndex, 0] = GetSheet(position.File, position.SheetName).Rows[rowIndex]?.Cells[position.Col.Value]?.Value;
                    }
                }

                return null;
            }
        }

        public object?[,] this[ExcelCellPostion start, int rowCount, int colCount]
        {
            get
            {
                var datas = new object?[rowCount, colCount];
                var startRow = start.Row.HasValue ? start.Row!.Value : 0;
                var startCol = start.Col.HasValue ? start.Col!.Value : 0;

                for (var rowIndex = 0; rowIndex < rowCount; rowIndex++)
                {
                    var row = GetSheet(start.File, start.SheetName).Rows[startRow + rowIndex];
                    for (var colIndex = 0; colIndex < colCount; colIndex++)
                    {
                        datas[rowIndex, colIndex] = row?.Cells[startCol + colIndex]?.Value;
                    }
                }

                return datas;
            }
        }


        public int RowCount
        {
            get
            {
                return GetSheet().Rows.Count;
            }
        }

        public int ColCount
        {
            get
            {
                return GetSheet().ColumnNames.Count;
            }
        }

        public void Dispose()
        {
            _excel?.Dispose();
        }
    }
}
