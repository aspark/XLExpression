using System;
using System.Collections.Generic;
using System.Text;

namespace XLExpression.Excel.Model
{
    public class FormulaInfo
    {
        public int SheetIndex { get; set; }

        public string? SheetName { get; set; }

        public int RowIndex { get; set; }

        public int ColIndex { get; set; }

        public string Formula { get; set; } = String.Empty;
    }
}
