using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using XLExpression.Common;

namespace XLExpression.Excel.Model
{
    public class SheetModel
    {
        internal SheetModel(ExcelModel excel, WorkbookPart xWorkbookPart, Sheet xSheet)
        {
            Excel = excel;
            Id = xSheet.Id?.Value;
            Name = xSheet.Name?.Value;


            //<worksheet>
            //    <dimension ref="A1"/><sheetViews><sheetView workbookViewId="0"/>
            //    </sheetViews><sheetFormatPr defaultRowHeight="14.25" x14ac:dyDescent="0.2"/>
            //    <sheetData/>
            //    <phoneticPr fontId="1" type="noConversion"/>
            //    <pageMargins left="0.7" right="0.7" top="0.75" bottom="0.75" header="0.3" footer="0.3"/>
            //</worksheet>
            var xWorksheet = ((WorksheetPart)xWorkbookPart.GetPartById(Id)).Worksheet;

            ParseDimension(xWorksheet);

            var xSheetView = xWorksheet.GetFirstChild<SheetViews>().Elements<SheetView>().FirstOrDefault();
            IsSelected = xSheetView?.TabSelected?.Value ?? false;

            var xSheetData = xWorksheet.GetFirstChild<SheetData>();
            Rows = xSheetData?.Elements<Row>().Select(r => new RowModel(this, xWorkbookPart, r)).ToList() ?? new List<RowModel>();
        }

        public ExcelModel Excel { get; private set; }

        public string? Id { get; set; }

        public string? Name { get; set; }

        public bool IsSelected { get; set; }

        //private ConcurrentDictionary<int, RowModel> _rows = new ConcurrentDictionary<int, RowModel>();

        private HashSet<string> _columns = new HashSet<string>();
        public ICollection<string> Colmuns => _columns;

        internal void RegisterColumn(string col)
        {
            _columns.Add(col);
        }

        private void ParseDimension(Worksheet xWorksheet)
        {
            //有数据的范围 <dimension ref="A1:J6" />
            var xDimension = xWorksheet.GetFirstChild<SheetDimension>();
            var xRange = xDimension?.GetAttribute("ref", string.Empty).Value;
            if (!string.IsNullOrWhiteSpace(xRange))
            {
                var max = ExcelHelper.ConvertNameToPosition(xRange.Split(':').Last());
                if (max.Col.HasValue)
                {
                    for (var i = 0; i <= max.Col.Value; i++)
                    {
                        RegisterColumn(ExcelHelper.ConvertIndexToName(i));
                    }
                }
            }
        }

        public IList<RowModel> Rows { get; private set; }

        private Dictionary<uint, (string formula, ExcelCellPostion position)> _dicSharedFormula = new Dictionary<uint, (string formula, ExcelCellPostion position)>();

        internal void RegisterSharedFormula(uint sharedIndex, string formula, string position)
        {
            var pos = ExcelHelper.ConvertNameToPosition(position);

            _dicSharedFormula[sharedIndex] = (formula, pos);
        }

        internal string ResolveSharedFormula(uint sharedIndex, string newPosition)
        {
            if (_dicSharedFormula.ContainsKey(sharedIndex))
            {
                var sharedItem = _dicSharedFormula[sharedIndex];
                var formula = sharedItem.formula;
                var newPos = ExcelHelper.ConvertNameToPosition(newPosition);
                if (newPos.Row.HasValue && newPos.Row != sharedItem.position.Row)
                {
                    formula = Regex.Replace(formula, @"(?<=\$?[a-zA-Z]+)(\d+)", (newPos.Row + 1).ToString(), RegexOptions.Compiled | RegexOptions.Singleline);
                }
                else if(newPos.Col.HasValue && newPos.Col != sharedItem.position.Col)
                {
                    formula = Regex.Replace(formula, @"([a-zA-Z]+)(?=\$?\d+)", ExcelHelper.ConvertIndexToName(newPos.Col.Value), RegexOptions.Compiled | RegexOptions.Singleline);
                }

                return formula;
            }

            return string.Empty;
        }
    }
}
