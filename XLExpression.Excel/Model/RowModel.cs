using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using XLExpression.Common;

namespace XLExpression.Excel.Model
{
    public class RowModel
    {
        private ConcurrentDictionary<int, CellModel> _cells = new ConcurrentDictionary<int, CellModel>();

        public RowModel()
        {

        }

        internal RowModel(SheetModel sheet, WorkbookPart workbookPart, Row r)
        {
            Sheet = sheet;

            //<row r = "3" spans = "1:10" x14ac: dyDescent = "0.2">
            //    <c r = "A3" s = "2" t = "s" ><v>8</v></c>
            //    <c r = "B3" s = "1" ><v>32</v></c>
            //    <c r = "C3" s = "1" ><v>12345</v ></ c>
            //    <c r = "D3" s = "1" ><v>1230</v></ c>
            //    <c r = "E3" s = "1" ><v>1231</v></ c>
            //    <c r = "F3" s = "1" ><v>2323</v></ c>
            //    <c r = "G3" s = "1" ><f>SUM(E3: F3)</f><v>3554</v></ c>
            //    <c r = "H3" s = "1" ><v>5123 </v></c>
            //    <c r = "I3" s = "1" ><v>120 </v></c>
            //    <c r = "J3" s = "1" ><f>C3 + D3 - G3 - H3 - I3</ f ><v>4778</v></c>
            //</row>

            foreach(var c in r.Descendants<Cell>())
            {
                var position = ExcelHelper.ConvertNameToPosition(c.CellReference?.Value);
                if (position.Col.HasValue)
                {
                    _cells.AddOrUpdate(position.Col.Value, i => new CellModel(this, workbookPart, c), (i, o) => new CellModel(this, workbookPart, c));
                }

            }
        }

        public SheetModel Sheet { get; private set; }

        public CellModel this[int col]
        {
            get
            {
                if (_cells.TryGetValue(col, out CellModel cell))
                    return cell;

                //return null;
                throw new InvalidOperationException("out of columns range");
            }
        }

        public IList<CellModel> Cells
        {
            get
            {
                //var rowData = new List<CellModel?>(_cells.Count);
                //for (var colIndex = 0; colIndex < rowData.Count; colIndex++)
                //{
                //    rowData.Add(_cells.ContainsKey(colIndex) ? _cells[colIndex] : new CellModel());
                //}

                //return rowData;

                return _cells.OrderBy(c => c.Key).Select(c => c.Value).ToList();
            }
        }
    }
}
