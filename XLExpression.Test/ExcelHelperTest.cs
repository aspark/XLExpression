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
            pos.Col.ShouldBe(0);
            pos.Row.ShouldBe(1);

            pos = ExcelHelper.ConvertNameToPosition("F2");
            pos.Col.ShouldBe(5);
            pos.Row.ShouldBe(1);

            pos = ExcelHelper.ConvertNameToPosition("F");
            pos.Col.ShouldBe(5);
            pos.Row.ShouldBe(null);

            pos = ExcelHelper.ConvertNameToPosition("2");
            pos.Col.ShouldBe(null);
            pos.Row.ShouldBe(1);

            pos = ExcelHelper.ConvertNameToPosition("Z2");
            pos.Col.ShouldBe(25);
            pos.Row.ShouldBe(1);

            pos = ExcelHelper.ConvertNameToPosition("F23");
            pos.Col.ShouldBe(5);
            pos.Row.ShouldBe(22);

            pos = ExcelHelper.ConvertNameToPosition("F223");
            pos.Col.ShouldBe(5);
            pos.Row.ShouldBe(222);

            pos = ExcelHelper.ConvertNameToPosition("AA3");
            pos.Col.ShouldBe(26);
            pos.Row.ShouldBe(2);

            pos = ExcelHelper.ConvertNameToPosition("AC3");
            pos.Col.ShouldBe(28);
            pos.Row.ShouldBe(2);

            pos = ExcelHelper.ConvertNameToPosition("CC25");
            pos.Col.ShouldBe(80);
            pos.Row.ShouldBe(24);

            pos = ExcelHelper.ConvertNameToPosition("ZA25");
            pos.Col.ShouldBe(676);
            pos.Row.ShouldBe(24);

            pos = ExcelHelper.ConvertNameToPosition("ZC25");
            pos.Col.ShouldBe(678);
            pos.Row.ShouldBe(24);

            pos = ExcelHelper.ConvertNameToPosition("ABC25");
            pos.Col.ShouldBe(730);
            pos.Row.ShouldBe(24);

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