using XLParser;

namespace XLExpression.Test
{
    public class XLParserTest
    {
        [Fact]
        public void TestSimple1()
        {
            var node = ExcelFormulaParser.Parse("1+2");

            node.ShouldNotBeNull();
            node.Print().ShouldBe("1+2");
            node.ChildNodes[0].IsFunction().ShouldBeTrue();
            node.ChildNodes[0].IsOperation().ShouldBeTrue();
            node.ChildNodes[0].GetFunction().ShouldBe("+");
        }

        [Fact]
        public void TestSimple2()
        {
            var node = ExcelFormulaParser.Parse("IF(F2>G2,1,0)");

            node.ShouldNotBeNull();
            node.Print().ShouldBe("IF(F2>G2,1,0)");
            node.ChildNodes[0].ChildNodes[0].IsFunction().ShouldBeTrue();
            node.ChildNodes[0].ChildNodes[0].IsOperation().ShouldBeFalse();
        }
    }
}