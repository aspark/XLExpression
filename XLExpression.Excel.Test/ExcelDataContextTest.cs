using XLExpression.Excel.Model;

namespace XLExpression.Excel.Test
{
    public class ExcelDataContextTest
    {
        [Fact]
        public void Excel()
        {
            var xls = new ExcelModel("Attachments/XLExpression.xlsx");
            var ctx = new ExcelDataContext(xls, 0);

            ctx.ShouldNotBeNull();

            //XLException

        }
    }
}