using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using XLExpression.Excel.Model;
[assembly:InternalsVisibleToAttribute("XLExpression.Excel.Test")]

namespace XLExpression.Excel
{
    public class FormulaBuilder
    {
        ExcelModel excel = null;

        public FormulaBuilder(string fileName)
        {
            excel = new ExcelModel(fileName);
        }

        public FormulaBuilder(Stream xlsx)
        {
            excel = new ExcelModel(xlsx);
        }

        public Dictionary<string, string> ExtractAllFormulaToCode()
        {
            var dic = new Dictionary<string, string>();

            foreach(var sheet in excel.Sheets)
            {
                foreach(var row in sheet.Rows)
                {
                    foreach(var cell in row.Cells)
                    {
                        if(cell.ValueType == EnumCellType.Formula)
                        {
                            dic[cell.Formula] = ExpressionBuilder.Instance.BuildToCode(cell.Formula);
                        }
                    }
                }
            }

            return dic;
        }

        public void CalculateAll()
        {
            foreach (var sheet in excel.Sheets)
            {
                foreach (var row in sheet.Rows)
                {
                    foreach (var cell in row.Cells)
                    {
                        if (cell.ValueType == EnumCellType.Formula)
                        {
                            
                        }
                    }
                }
            }

        }

        public void Calculate()
        {

        }
    }
}
