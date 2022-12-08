using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace XLExpression.Excel.Model
{
    //https://www.todaysoftmag.com/article/848/getting-started-with-openxml

    public class ExcelModel
    {
        public ExcelModel(string fileName)
        {
            using (var doc = SpreadsheetDocument.Open(fileName, false))
            {
                Init(doc);
            }
        }

        public ExcelModel(Stream xlsx)
        {
            using (var doc = SpreadsheetDocument.Open(xlsx, false))
            {
                Init(doc);
            }
        }

        private void Init(SpreadsheetDocument doc)
        {
            ParseSharedStringTable(doc.WorkbookPart);

            if (doc.WorkbookPart?.Workbook.Sheets != null)
                Sheets = doc.WorkbookPart.Workbook.Sheets.Descendants<Sheet>().Select(s => new SheetModel(this, doc.WorkbookPart, s)).ToList();
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
    }
}
