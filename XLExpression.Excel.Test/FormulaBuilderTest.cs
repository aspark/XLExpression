using XLExpression.Excel.Model;

namespace XLExpression.Excel.Test
{
    public class FormulaBuilderTest
    {
        [Fact]
        public void Excel1()
        {
            var builder = new FormulaBuilder("Attachments/XLExpression.xlsx");

            builder.ShouldNotBeNull();

            var dicCode = builder.ExtractAllFormulaToCode();

            dicCode.Count.ShouldBe(24);

        }

        [Fact]
        public void Excel2()
        {
            var excel = new ExcelModel("Attachments/XLExpression.xlsx");
            var builder = new FormulaBuilder(excel);

            builder.ShouldNotBeNull();

            var dicCode = builder.ExtractAllFormulaToCode();

            dicCode.Count.ShouldBe(24);

            var result = builder.CalculateAll();

            result.Count.ShouldBe(24);

            foreach(var info in result)
            {
                info.Result.ShouldBe(excel.Sheets[info.SheetIndex].Rows[info.RowIndex].Cells[info.ColIndex].Value);//验证计算结果与文件中的一致
            }
        }
    }
}