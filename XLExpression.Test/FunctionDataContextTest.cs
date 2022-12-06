using XLExpression.Common;
using XLExpression.Functions;
using XLExpression.Nodes;
using XLParser;

namespace XLExpression.Test
{
    public class FunctionDataContextTest
    {
        [Fact]
        public void ExcelNameTest()
        {
            var context = new DefaultDataContext(new Dictionary<string, object>
            {
                //F:5 G:6 H:7
                // null, null, null
                {"F2", 11 }, {"G2", 21 }, {"H2", 31 },
                {"F3", 12 }, {"G3", 22 }, {"H3", 32 },
            });


            context["A1"].ShouldBe(null);
            context["F2"].ShouldBe(11);
            context["G3"].ShouldBe(22);

            context[1, 1].ShouldBe(null);
            context[1, 5].ShouldBe(11);
            context[2, 6].ShouldBe(22);

            var result = context["F:F"];
            (result is object[,]).ShouldBe(true);
            ((object[,])result).GetLength(0).ShouldBe(3);
            ((object[,])result).GetLength(1).ShouldBe(1);
            ((object[,])result)[0, 0].ShouldBe(null);
            ((object[,])result)[1, 0].ShouldBe(11);
            ((object[,])result)[2, 0].ShouldBe(12);

            result = context["F:G"];
            (result is object[,]).ShouldBe(true);
            ((object[,])result).GetLength(0).ShouldBe(3);
            ((object[,])result).GetLength(1).ShouldBe(2);
            ((object[,])result)[0, 1].ShouldBe(null);
            ((object[,])result)[1, 0].ShouldBe(11);
            ((object[,])result)[1, 1].ShouldBe(21);
            ((object[,])result)[2, 1].ShouldBe(22);

            result = context["2:2"];
            (result is object[,]).ShouldBe(true);
            ((object[,])result).GetLength(0).ShouldBe(1);
            ((object[,])result).GetLength(1).ShouldBe(8);
            ((object[,])result)[0, 1].ShouldBe(null);
            ((object[,])result)[0, 6].ShouldBe(21);

            result = context["2:3"];
            (result is object[,]).ShouldBe(true);
            ((object[,])result).GetLength(0).ShouldBe(2);
            ((object[,])result).GetLength(1).ShouldBe(8);
            ((object[,])result)[1, 1].ShouldBe(null);
            ((object[,])result)[0, 6].ShouldBe(21);
            ((object[,])result)[1, 6].ShouldBe(22);
            ((object[,])result)[1, 7].ShouldBe(32);


            context["F:F"] = 1;
            result = context["F:F"];
            (result is object[,]).ShouldBe(true);
            ((object[,])result).GetLength(0).ShouldBe(3);
            ((object[,])result).GetLength(1).ShouldBe(1);
            ((object[,])result)[0, 0].ShouldBe(1);
            ((object[,])result)[1, 0].ShouldBe(1);
            ((object[,])result)[2, 0].ShouldBe(1);

            context["2:3"] = 2;
            result = context["2:3"];
            (result is object[,]).ShouldBe(true);
            ((object[,])result).GetLength(0).ShouldBe(2);
            ((object[,])result).GetLength(1).ShouldBe(8);
            ((object[,])result)[1, 1].ShouldBe(2);
            ((object[,])result)[0, 6].ShouldBe(2);
            ((object[,])result)[1, 6].ShouldBe(2);
            ((object[,])result)[1, 7].ShouldBe(2);
        }

    }
}