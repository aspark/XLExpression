using XLExpression.Excel.Model;

namespace XLExpression.Excel.Test
{
    public class FormulaBuilderTest
    {
        [Fact]
        public void Excel()
        {
            var builder = new FormulaBuilder("Attachments/XLExpression.xlsx");

            builder.ShouldNotBeNull();

            var dicCode = builder.ExtractAllFormulaToCode();

            dicCode.Count.ShouldBe(12);

        }
    }
}