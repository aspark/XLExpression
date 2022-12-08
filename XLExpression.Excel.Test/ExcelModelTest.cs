using XLExpression.Excel.Model;

namespace XLExpression.Excel.Test
{
    public class ExcelModelTest
    {
        [Fact]
        public void Excel()
        {
            var excel = new ExcelModel("Attachments/XLExpression.xlsx");

            excel.ShouldNotBeNull();

            excel.Sheets.Count.ShouldBe(3);

            excel.Sheets[0].Rows.Count.ShouldBe(7);
            excel.Sheets[0].Colmuns.Count.ShouldBe(12);

            excel.Sheets[0].Rows[2][0].ValueType.ShouldBe(EnumCellType.String);
            excel.Sheets[0].Rows[2][0].Value.ShouldBe("ÕÅÈý");

            excel.Sheets[0].Rows[2][1].Value.ShouldBe(32);
            excel.Sheets[0].Rows[2][1].ValueType.ShouldBe(EnumCellType.Number);

            excel.Sheets[0].Rows[2][6].ValueType.ShouldBe(EnumCellType.Formula);
            excel.Sheets[0].Rows[2][6].Formula.ShouldBe("SUM(E3:F3)");
            excel.Sheets[0].Rows[2][6].Value.ShouldBe(3554);

            //excel.Sheets[0].Rows[2][10].ValueType.ShouldBe(EnumCellType.Date);
            //excel.Sheets[0].Rows[2][10].Value.ShouldBe(DateTime.Parse("2022/12/8"));
            //excel.Sheets[0].Rows[2][10].StringValue.ShouldBe("2022/12/8");

            excel.Sheets[0].Rows[2][11].ValueType.ShouldBe(EnumCellType.Boolean);
            excel.Sheets[0].Rows[2][11].Value.ShouldBe(false);
            excel.Sheets[0].Rows[2][11].StringValue.ShouldBe("0", StringCompareShould.IgnoreCase);

            excel.Sheets[0].Rows[3][11].ValueType.ShouldBe(EnumCellType.Boolean);
            excel.Sheets[0].Rows[3][11].Value.ShouldBe(true);
            excel.Sheets[0].Rows[3][11].StringValue.ShouldBe("1", StringCompareShould.IgnoreCase);

            excel.Sheets[0].Rows[5][6].ValueType.ShouldBe(EnumCellType.Formula);
            excel.Sheets[0].Rows[5][6].Formula.ShouldBe("SUM(E6:F6)");
            excel.Sheets[0].Rows[5][6].Value.ShouldBe(3324);

            excel.Sheets[0].Rows[5][9].ValueType.ShouldBe(EnumCellType.Formula);
            excel.Sheets[0].Rows[5][9].Formula.ShouldBe("C6+D6-G6-H6-I6");
            excel.Sheets[0].Rows[5][9].Value.ShouldBe(20027);

            excel.Sheets[0].Rows[6][9].ValueType.ShouldBe(EnumCellType.Formula);
            excel.Sheets[0].Rows[6][9].Formula.ShouldBe("SUM(J3:J6)");
            excel.Sheets[0].Rows[6][9].Value.ShouldBe(92490);
        }
    }
}