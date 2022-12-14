using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections;
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
        private Row _xRow = null;

        //public RowModel()
        //{

        //}

        internal RowModel(SheetModel sheet, WorkbookPart workbookPart, Row xRow)
        {
            _xRow = xRow;
            Sheet = sheet;
            Cells = new CellsCollection(this, xRow);

            //<row r="3" spans="1:10" x14ac:dyDescent="0.2">
            //    <c r="A3" s="2" t = "s"><v>8</v></c>
            //    <c r="B3" s="1"><v>32</v></c>
            //    <c r="C3" s="1"><v>12345</v ></ c>
            //    <c r="D3" s="1"><v>1230</v></ c>
            //    <c r="E3" s="1"><v>1231</v></ c>
            //    <c r="F3" s="1"><v>2323</v></ c>
            //    <c r="G3" s="1"><f>SUM(E3: F3)</f><v>3554</v></c>
            //    <c r="H3" s="1"><v>5123 </v></c>
            //    <c r="I3" s="1"><v>120 </v></c>
            //    <c r="J3" s="1"><f>C3 + D3 - G3 - H3 - I3</f><v>4778</v></c>
            //</row>

            foreach (var c in xRow.Descendants<Cell>())
            {
                var position = ExcelHelper.ConvertNameToPosition(c.CellReference?.Value);
                if (position.Col.HasValue)
                {
                    this.Cells[position.Col.Value] = new CellModel(this, workbookPart, c);
                }
                else
                    throw new InvalidOperationException("invalid cell reference name");
            }
        }

        public SheetModel Sheet { get; private set; }

        public CellsCollection Cells { get; private set; }

        public int Index { get; internal set; }
    }

    public class CellsCollection : IEnumerable<CellModel>
    {
        private ConcurrentDictionary<int, CellModel> _cells = new ConcurrentDictionary<int, CellModel>();
        //private List<CellModel> _cells = new List<CellModel>();

        private RowModel _row = null;
        private Row _xRow = null;

        internal CellsCollection(RowModel row, Row xRow)
        {
            _row = row;
            _xRow = xRow;
        }

        public CellModel this[int col]
        {
            get
            {
                if (_cells.TryGetValue(col, out var value))
                    return value;

                //return null;
                throw new IndexOutOfRangeException("columns out of range");
            }
            set
            {
                value.ColIndex = col;
                _cells.AddOrUpdate(col, value, (k, o) => value);
                if (col > MaxIndex)
                    MaxIndex = col;
            }
        }

        public int Count => _cells.Count;

        public int MaxIndex { get; private set; }

        public IEnumerator<CellModel> GetEnumerator()
        {
            return _cells.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _cells.Values.GetEnumerator();
        }
    }
}
