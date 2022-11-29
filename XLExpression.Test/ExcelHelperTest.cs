using XLExpression.Common;
using XLExpression.Nodes;
using XLParser;

namespace XLExpression.Test
{
    public class ExcelHelperTest
    {
        [Fact]
        public void ExcelNameTest()
        {
            var pos = ExcelHelper.ConvertNameToPosition("A2");
            pos.col.ShouldBe(0);
            pos.row.ShouldBe(1);

            pos = ExcelHelper.ConvertNameToPosition("F2");
            pos.col.ShouldBe(5);
            pos.row.ShouldBe(1);

            pos = ExcelHelper.ConvertNameToPosition("Z2");
            pos.col.ShouldBe(25);
            pos.row.ShouldBe(1);

            pos = ExcelHelper.ConvertNameToPosition("F23");
            pos.col.ShouldBe(5);
            pos.row.ShouldBe(22);

            pos = ExcelHelper.ConvertNameToPosition("F223");
            pos.col.ShouldBe(5);
            pos.row.ShouldBe(222);

            pos = ExcelHelper.ConvertNameToPosition("AA3");
            pos.col.ShouldBe(26);
            pos.row.ShouldBe(2);

            pos = ExcelHelper.ConvertNameToPosition("AC3");
            pos.col.ShouldBe(28);
            pos.row.ShouldBe(2);

            pos = ExcelHelper.ConvertNameToPosition("CC25");
            pos.col.ShouldBe(80);
            pos.row.ShouldBe(24);

            pos = ExcelHelper.ConvertNameToPosition("ZA25");
            pos.col.ShouldBe(676);
            pos.row.ShouldBe(24);

            pos = ExcelHelper.ConvertNameToPosition("ZC25");
            pos.col.ShouldBe(678);
            pos.row.ShouldBe(24);

            pos = ExcelHelper.ConvertNameToPosition("ABC25");
            pos.col.ShouldBe(730);
            pos.row.ShouldBe(24);

            var name = ExcelHelper.ConvertIndexToName(0);
            name.ShouldBe("A");

            name = ExcelHelper.ConvertIndexToName(25);
            name.ShouldBe("Z");

            name = ExcelHelper.ConvertIndexToName(26);
            name.ShouldBe("AA");

            name = ExcelHelper.ConvertIndexToName(0, 0);
            name.ShouldBe("A1");

            name = ExcelHelper.ConvertIndexToName(4, 1);
            name.ShouldBe("E2");

            name = ExcelHelper.ConvertIndexToName(730, 24);
            name.ShouldBe("ABC25");

        }

    }
}