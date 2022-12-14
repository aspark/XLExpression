using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace XLExpression.Excel.Model
{
    //https://www.todaysoftmag.com/article/848/getting-started-with-openxml

    public class ExcelModel: IDisposable
    {
        SpreadsheetDocument _xDoc;

        public ExcelModel(string fileName)
        {
            _xDoc = SpreadsheetDocument.Open(fileName, false);
            Init();
        }

        public ExcelModel(Stream xlsx)
        {
            _xDoc = SpreadsheetDocument.Open(xlsx, false);
            Init();
        }

        private void Init()
        {
            var wbPart = _xDoc.WorkbookPart;

            ParseSharedStringTable(wbPart);

            if (wbPart?.Workbook.Sheets != null)
                Sheets = wbPart.Workbook.Sheets.Descendants<Sheet>().Select(s => new SheetModel(this, wbPart, s)).ToList();
        }

        public IList<SheetModel> Sheets { get; private set; } = new List<SheetModel>();

        private Dictionary<string, string> _dicSharedString = new Dictionary<string, string>();
        private void ParseSharedStringTable(WorkbookPart? xWorkbookPart)
        {
            if (xWorkbookPart == null)
                return;

            //<sst count="15" uniqueCount="15">
            //<si><t>姓名</t><phoneticPr fontId="1" type="noConversion"/></si>
            //<si><t>年龄</t><phoneticPr fontId="1" type="noConversion"/></si>
            //</sst>
            var stringTable = xWorkbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();

            var index = 0;
            foreach (var item in stringTable.SharedStringTable.Elements<SharedStringItem>())
            {
                _dicSharedString[index.ToString()] = item.InnerText;
                index++;
            }
        }

        internal string GetSharedString(string index)
        {
            if (_dicSharedString.ContainsKey(index))
            {
                return _dicSharedString[index];
            }

            return String.Empty;
        }

        private void ParseStyleFormat(WorkbookPart? xWorkbookPart)
        {
            if (xWorkbookPart == null)
                return;

            xWorkbookPart.GetPartsOfType<StylesPart>();
        }


        //public void ParseCalcChain()
        //{
        //    var calcpart = _xDoc.WorkbookPart.GetPartsOfType<CalculationChainPart>().FirstOrDefault();

        //}

        public void Save()
        {
            _xDoc.Save();
        }


        public void Dispose()
        {
            _xDoc?.Dispose();
        }
    }
}
