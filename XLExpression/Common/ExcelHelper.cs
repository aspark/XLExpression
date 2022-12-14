using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace XLExpression.Common
{
    public class ExcelHelper
    {
        private static List<char> colNames = new List<char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        private static Regex _regCellA1Name = new Regex(@"(?<col>[A-Z]*)(?<row>[0-9]*)");//A1
        //private static Regex _regCellR1C1Name = new Regex(@"(R\[?(?<col>\-?[0-9]+)\]?)?(C\[?(?<row>\-?[0-9]+)\]?)?");//R1C1

        /// <summary>
        /// 将Excel字符坐标转为数字坐标
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ExcelCellPostion ConvertNameToPosition(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return (null, null);

            //var chars = new char[]
            var m = _regCellA1Name.Match(name);
            int? col = null;
            int? row = null;
            if (m.Success)
            {
                var colName = m.Groups["col"].Value;
                if (!string.IsNullOrWhiteSpace(colName))
                {
                    for (int i = 0; i < colName.Length; i++)
                    {
                        col = (col ?? 0) + ((int)Math.Pow(colNames.Count, colName.Length - 1 - i) * (colNames.IndexOf(colName[i]) + 1));
                    }

                    if (col > 0)
                        col -= 1;
                }

                var rowName = m.Groups["row"].Value;
                if (!string.IsNullOrWhiteSpace(rowName))
                {
                    if (int.TryParse(rowName, out int r))
                        row = r - 1;
                }
            }
            //else
            //{
            //    m = _regCellR1C1Name.Match(name);
            //    if (m.Success)
            //    {
            //        var colName = m.Groups["col"].Value;
            //        if (!string.IsNullOrEmpty(colName))
            //        {
            //            col = colName.TryToInt();

            //            if (col > 0)
            //                col -= 1;
            //        }

            //        var rowName = m.Groups["row"].Value;
            //        if (!string.IsNullOrWhiteSpace(rowName))
            //        {
            //            row = rowName.TryToInt();

            //            if (row > 0)
            //                row -= 1;
            //        }
            //    }
            //}

            return (col, row);
        }

        /// <summary>
        /// 将数字转为Excel坐标
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public static string ConvertIndexToName(int col, int row)
        {
            return ConvertIndexToName(col) + (row + 1);
        }

        public static string ConvertIndexToName(string? sheetName, int col, int row)
        {
            return (sheetName??"Sheet") + "!" + ConvertIndexToName(col) + (row + 1);
        }

        public static string EnsureSheetName(string? sheetName, string name)
        {
            if (name.Contains('!') == false)
                return (sheetName ?? "Sheet") + "!" + name;

            return name;
        }

        /// <summary>
        /// 将数字转为Excel字符坐标
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        public static string ConvertIndexToName(int col)
        {
            var colName = "";
            var quotient = col;
            for (int i = 0; i < 1000; i++)
            {
                quotient = Math.DivRem(quotient, colNames.Count, out int rem);
                colName = colNames[i > 0 ? rem - 1 : rem] + colName;//进位后的余下的1，对应0

                if (quotient <= 0)
                    break;
            }

            return colName;
        }

    }


    public struct ExcelCellPostion
    {
        public int? Col;
        public int? Row;
        public Boolean IsRelative;

        public ExcelCellPostion(int? col, int? row)
        {
            Col = col;
            Row = row;
            IsRelative = false;
        }

        //public override bool Equals(object? obj)
        //{
        //    return obj is ExcelCellPostion other &&
        //           Col == other.Col &&
        //           Row == other.Row;
        //}

        //public override int GetHashCode()
        //{
        //    return HashCode.Combine(Col, Row);
        //}

        public void Deconstruct(out int? col, out int? row)
        {
            col = Col;
            row = Row;
        }

        public static implicit operator (int? Col, int? Row)(ExcelCellPostion value)
        {
            return (value.Col, value.Row);
        }

        public static implicit operator ExcelCellPostion((int? Col, int? Row) value)
        {
            return new ExcelCellPostion(value.Col, value.Row);
        }
    }
}
