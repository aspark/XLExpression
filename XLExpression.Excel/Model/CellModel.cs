using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace XLExpression.Excel.Model
{
    public class CellModel
    {
        public CellModel()
        {

        }

        internal CellModel(RowModel row, WorkbookPart workbookPart, Cell xCell)
        {
            this.Row = row;

            //<c r = "A3" s = "2" t = "s" ><v>8</ v></c> //字符
            //<c r = "F3" s = "1" ><v>2323</v></ c> //数值
            //<c r = "G3" s = "1" ><f>SUM(E3: F3)</f><v>3554</v></c> //公式
            //<x:c r=\"G5\" s=\"1\"><x:f t=\"shared\" si=\"0\" /><x:v>4734</x:v></x:c>
            //CellValues
            StringValue = xCell.CellValue?.InnerText;

            if (!string.IsNullOrEmpty(StringValue))
            {
                if (xCell.DataType != null)
                {
                    switch (xCell.DataType.Value)
                    {
                        case CellValues.Boolean:
                            Value = StringValue.TryToBool();
                            ValueType = EnumCellType.Boolean;
                            break;
                        case CellValues.Number:
                            Value = StringValue.TryToDecimal();
                            ValueType = EnumCellType.Number;
                            break;
                        case CellValues.Error:
                            Value = null;
                            ValueType = EnumCellType.Err;
                            break;
                        case CellValues.SharedString:
                            StringValue = this.Row.Sheet.Excel.GetSharedString(StringValue);
                            Value = StringValue;
                            ValueType = EnumCellType.String;
                            break;
                        case CellValues.String:
                            Value = StringValue;
                            ValueType = EnumCellType.String;
                            break;
                        case CellValues.InlineString:
                            Value = StringValue;
                            ValueType = EnumCellType.String;
                            break;
                        case CellValues.Date:
                            Value = StringValue;
                            ValueType = EnumCellType.Date;
                            break;
                        default:


                            break;
                    }
                }
                else
                {
                    Value = StringValue.TryToDecimal();
                    ValueType = EnumCellType.Number;
                }
            }

            if (xCell.CellFormula != null)
            {
                Formula = xCell.CellFormula?.InnerText;

                if (xCell.CellFormula.SharedIndex != null)
                {
                    if (!string.IsNullOrEmpty(Formula))
                    {
                        Row.Sheet.RegisterSharedFormula(xCell.CellFormula.SharedIndex.Value, Formula, xCell.CellReference.Value);
                    }
                    else
                    {
                        Formula = Row.Sheet.ResolveSharedFormula(xCell.CellFormula.SharedIndex.Value, xCell.CellReference.Value);
                    }
                }

                if (!string.IsNullOrEmpty(Formula))
                {
                    this.ValueType = EnumCellType.Formula;
                }
            }
        }

        public RowModel Row { get; private set; }

        public object? Value { get; private set; }

        public EnumCellType ValueType { get; }

        public string? StringValue { get; private set; }

        public string? Formula { get; private set; }

        public bool IsMerged => MergedParent != null;

        public CellModel? MergedParent { get; private set; }
    }

    public enum EnumCellType
    {
        String,

        Number,

        Date,

        Boolean,

        Formula,

        Err,
    }
}
