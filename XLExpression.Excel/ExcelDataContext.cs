using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Linq;
using XLExpression.Common;
using XLExpression.Excel.Model;
using XLExpression.Functions;

namespace XLExpression.Excel
{
    internal class ExcelDataContext: IDataContext
    {
        private SheetModel _sheet = null;

        public ExcelDataContext(string fileName)
        {
            var xls = new ExcelModel(fileName);

            _sheet = xls.Sheets.FirstOrDefault(s=>s.IsSelected);
            if(_sheet == null)
            {
                _sheet = xls.Sheets.FirstOrDefault();
            }

            if (_sheet == null)
                throw new ArgumentNullException("cannot find sheet");
        }

        public ExcelDataContext(SheetModel sheet)
        {
            _sheet = sheet;
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
                    if (index.Row.HasValue && index.Col.HasValue)
                    {
                        return this[index.Row.Value, index.Col.Value];
                    }
                    else if (index.Row.HasValue)
                    {
                        var row = _sheet.Rows[index.Row.Value];
                        if (row != null)
                        {
                            var allData = row.Cells;

                            return allData.Dimensional();
                        }

                    }
                    else if (index.Col.HasValue)
                    {
                        var colData = new object?[RowCount, 1];
                        for (var rowIndex = 0; rowIndex < colData.GetLength(0); rowIndex++)
                        {
                            colData[rowIndex, 0] = _sheet.Rows[rowIndex]?.Cells[index.Col.Value]?.Value;
                        }
                    }

                    return null;
                }
            }
        }

        public object? this[int row, int col]
        {
            get
            {
                return _sheet.Rows[row].Cells[col]?.Value;
            }
        }


        public object?[,] this[int rowStart, int rowCount, int colStart, int colCount]
        {
            get
            {
                var datas = new object?[rowCount, colCount];
                for (var rowIndex = 0; rowIndex < rowCount; rowIndex++)
                {
                    var row = _sheet.Rows[rowStart + rowIndex];
                    for (var colIndex = 0; colIndex < colCount; colIndex++)
                    {
                        datas[rowIndex, colIndex] = row?.Cells[colStart + colIndex]?.Value;
                    }
                }

                return datas;
            }
        }


        public int RowCount
        {
            get
            {
                return _sheet.Rows.Count;
            }
        }

        public int ColCount
        {
            get
            {
                return _sheet.ColumnNames.Count;
            }
        }

    }
}
