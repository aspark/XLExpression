using XLExpression.Common;
using XLExpression.Excel.Model;

namespace XLExpression.Excel.Test
{
    public class ExcelDataContextTest
    {
        [Fact]
        public void Excel()
        {
            var xls = new ExcelModel("Attachments/XLExpression.xlsx");
            var context = new ExcelDataContext(xls, 0);

            context.ShouldNotBeNull();

            context["A1"].ShouldBe("姓名");
            context["F2"].ShouldBe("养老");
            context["G3"].ShouldBe(3554);
            context["Sheet2!G3"].ShouldBe(3554);
            context["Sheet3!A1"].ShouldBe(null);

            context[ExcelCellPostion.Create(1, 1)].ShouldBe(null);
            context[ExcelCellPostion.Create(5, 1)].ShouldBe("养老");
            context[ExcelCellPostion.Create(6, 2)].ShouldBe(3554);

            var result = context["F2:G3"];
            (result is object[,]).ShouldBe(true);
            ((object[,])result!).GetLength(0).ShouldBe(2);
            ((object[,])result).GetLength(1).ShouldBe(2);
            ((object[,])result)[0, 1].ShouldBe("小计");
            ((object[,])result)[1, 0].ShouldBe(2323);
            ((object[,])result)[1, 1].ShouldBe(3554);

            result = context[ExcelCellPostion.Create(5, 1), 2, 2];
            (result is object[,]).ShouldBe(true);
            ((object[,])result!).GetLength(0).ShouldBe(2);
            ((object[,])result).GetLength(1).ShouldBe(2);
            ((object[,])result)[0, 1].ShouldBe("小计");
            ((object[,])result)[1, 0].ShouldBe(2323);
            ((object[,])result)[1, 1].ShouldBe(3554);

            result = context["F:F"];
            (result is object[,]).ShouldBe(true);
            ((object[,])result!).GetLength(0).ShouldBe(46);
            ((object[,])result).GetLength(1).ShouldBe(1);
            ((object[,])result)[0, 0].ShouldBe(null);
            ((object[,])result)[1, 0].ShouldBe("养老");
            ((object[,])result)[2, 0].ShouldBe(2323);
            ((object[,])result)[13, 0].ShouldBe(null);
            ((object[,])result)[14, 0].ShouldBe(6);

            result = context["F:G"];
            (result is object[,]).ShouldBe(true);
            ((object[,])result!).GetLength(0).ShouldBe(46);
            ((object[,])result).GetLength(1).ShouldBe(2);
            ((object[,])result)[0, 1].ShouldBe(null);
            ((object[,])result)[1, 0].ShouldBe("养老");
            ((object[,])result)[1, 1].ShouldBe("小计");
            ((object[,])result)[2, 1].ShouldBe(3554);
            ((object[,])result)[13, 1].ShouldBe(null);
            ((object[,])result)[14, 1].ShouldBe(7);

            result = context["2:2"];
            (result is object[,]).ShouldBe(true);
            ((object[,])result!).GetLength(0).ShouldBe(1);
            ((object[,])result).GetLength(1).ShouldBe(12);
            ((object[,])result)[0, 1].ShouldBe(null);
            ((object[,])result)[0, 6].ShouldBe("小计");

            result = context["2:3"];
            (result is object[,]).ShouldBe(true);
            ((object[,])result!).GetLength(0).ShouldBe(2);
            ((object[,])result).GetLength(1).ShouldBe(12);
            ((object[,])result)[0, 1].ShouldBe(null);
            ((object[,])result)[1, 1].ShouldBe(32);
            ((object[,])result)[0, 6].ShouldBe("小计");
            ((object[,])result)[1, 6].ShouldBe(3554);
            ((object[,])result)[1, 7].ShouldBe(5123);

        }
    }
}