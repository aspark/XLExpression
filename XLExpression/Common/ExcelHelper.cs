using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace XLExpression.Common
{
    internal class ExcelHelper
    {
        private static List<char> colNames = new List<char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        /// <summary>
        /// 将Excel坐标转为数字
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static (int? col, int? row) ConvertNameToPosition(string name)
        {
            //var chars = new char[]
            var m = Regex.Match(name, @"(?<col>[A-Z]*)(?<row>[0-9]*)");
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

        /// <summary>
        /// 将数字转为Excel坐标
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
}
