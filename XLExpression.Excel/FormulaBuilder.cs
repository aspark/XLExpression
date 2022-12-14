using DocumentFormat.OpenXml.Office2016.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using XLExpression.Common;
using XLExpression.Excel.Model;
[assembly: InternalsVisibleToAttribute("XLExpression.Excel.Test")]

namespace XLExpression.Excel
{
    public class FormulaBuilder
    {
        ExcelModel _excel = null;

        public FormulaBuilder(string fileName)
        {
            _excel = new ExcelModel(fileName);
        }

        public FormulaBuilder(Stream xlsx)
        {
            _excel = new ExcelModel(xlsx);
        }

        public FormulaBuilder(ExcelModel model)
        {
            _excel = model;
        }

        public List<FormulaInfo> ExtractAllFormula()
        {
            var items = new List<FormulaInfo>();

            var sheetIndex = 0;
            foreach (var sheet in _excel.Sheets)
            {
                foreach (var row in sheet.Rows)
                {
                    foreach (var cell in row.Cells)
                    {
                        if (cell.ValueType == EnumCellType.Formula && string.IsNullOrEmpty(cell.Formula) == false)
                        {
                            items.Add(new FormulaInfo() { Formula = cell.Formula, SheetIndex = sheetIndex, SheetName = sheet.Name, ColIndex = cell.ColIndex, RowIndex = cell.RowIndex});
                        }
                    }
                }

                sheetIndex++;
            }

            return items;
        }

        public List<(string Formula, string Code)> ExtractAllFormulaToCode()
        {
            return ExtractAllFormula().Select(p => (p.Formula, ExpressionBuilder.Instance.BuildToCode(p.Formula))).ToList();
        }

        public List<FormulaCalculateResult> CalculateAll(bool saveToExcel = false)
        {
            var result = new List<FormulaCalculateResult>();

            var calcChain = new Dictionary<string , FormulaRelationInfo>();

            ExtractAllFormula().ForEach(i =>
            {
                calcChain[ExcelHelper.ConvertIndexToName(i.SheetName, i.ColIndex, i.RowIndex)] = new FormulaRelationInfo(i, ExpressionBuilder.Instance.Build(i.Formula));
            });

            if (calcChain.Any())
            {
                //建依赖
                foreach(var item in calcChain)
                {
                    if (item.Value.Expression.ArgNames != null)
                    {
                        foreach (var arg in item.Value.Expression.ArgNames)
                        {
                            var key = ExcelHelper.EnsureSheetName(item.Value.SheetName, arg.Replace("$", ""));//统一去掉绝对定位

                            if (calcChain.ContainsKey(key))
                            {
                                item.Value.Relation.Add(calcChain[key]);
                            }
                        }
                    }
                }

                //按依赖的情况，调用
                var dicContext = new Dictionary<int, ExcelDataContext>();
                var hasChanged = false;
                foreach(var calc in calcChain.OrderBy(c => c.Value.RelationCount))
                {
                    //_excel.Sheets[calc.Value.SheetIndex].Rows[calc.Value]
                    var obj = calc.Value.Expression.Invoke(dicContext.GetOrAdd(calc.Value.SheetIndex, () => new ExcelDataContext(_excel, calc.Value.SheetIndex)));
                    result.Add(new FormulaCalculateResult() { ColIndex = calc.Value.ColIndex, SheetIndex = calc.Value.SheetIndex, Formula = calc.Value.Formula, Result = obj, RowIndex = calc.Value.RowIndex });
                    if (saveToExcel)
                    {
                        hasChanged = true;
                        var cell = _excel.Sheets[calc.Value.SheetIndex].Rows[calc.Value.RowIndex]?.Cells[calc.Value.ColIndex];
                        if(cell != null)
                            cell.Value = obj;
                        else
                        {
                            //todo add cell
                            throw new NotImplementedException("add cell");
                        }
                    }
                }

                if (hasChanged && saveToExcel)
                {
                    _excel.Save();
                }
            }

            return result;
        }

    }
}
