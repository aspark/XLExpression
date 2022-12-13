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

            excel.Sheets[0].Rows.Count.ShouldBe(10);
            excel.Sheets[0].ColumnNames.Count.ShouldBe(12);

            excel.Sheets[0].Rows[2].Cells[0].ValueType.ShouldBe(EnumCellType.String);
            excel.Sheets[0].Rows[2].Cells[0].Value.ShouldBe("ÕÅÈý");

            excel.Sheets[0].Rows[2].Cells[1].Value.ShouldBe(32);
            excel.Sheets[0].Rows[2].Cells[1].ValueType.ShouldBe(EnumCellType.Number);

            excel.Sheets[0].Rows[2].Cells[6].ValueType.ShouldBe(EnumCellType.Formula);
            excel.Sheets[0].Rows[2].Cells[6].Formula.ShouldBe("SUM(E3:F3)");
            excel.Sheets[0].Rows[2].Cells[6].Value.ShouldBe(3554);

            //excel.Sheets[0].Rows[2][10].ValueType.ShouldBe(EnumCellType.Date);
            //excel.Sheets[0].Rows[2][10].Value.ShouldBe(DateTime.Parse("2022/12/8"));
            //excel.Sheets[0].Rows[2][10].StringValue.ShouldBe("2022/12/8");

            excel.Sheets[0].Rows[2].Cells[11].ValueType.ShouldBe(EnumCellType.Boolean);
            excel.Sheets[0].Rows[2].Cells[11].Value.ShouldBe(false);
            excel.Sheets[0].Rows[2].Cells[11].StringValue.ShouldBe("0", StringCompareShould.IgnoreCase);

            excel.Sheets[0].Rows[3].Cells[11].ValueType.ShouldBe(EnumCellType.Boolean);
            excel.Sheets[0].Rows[3].Cells[11].Value.ShouldBe(true);
            excel.Sheets[0].Rows[3].Cells[11].StringValue.ShouldBe("1", StringCompareShould.IgnoreCase);

            excel.Sheets[0].Rows[5].Cells[6].ValueType.ShouldBe(EnumCellType.Formula);
            excel.Sheets[0].Rows[5].Cells[6].Formula.ShouldBe("SUM(E6:F6)");
            excel.Sheets[0].Rows[5].Cells[6].Value.ShouldBe(3324);

            excel.Sheets[0].Rows[5].Cells[9].ValueType.ShouldBe(EnumCellType.Formula);
            excel.Sheets[0].Rows[5].Cells[9].Formula.ShouldBe("C6+D6-G6-H6-I6");
            excel.Sheets[0].Rows[5].Cells[9].Value.ShouldBe(20027);

            excel.Sheets[0].Rows[6].Cells[9].ValueType.ShouldBe(EnumCellType.Formula);
            excel.Sheets[0].Rows[6].Cells[9].Formula.ShouldBe("SUM(J3:J6)");
            excel.Sheets[0].Rows[6].Cells[9].Value.ShouldBe(92490);

            excel.Sheets[0].Rows[14].Cells[0].Value.ShouldBe(1);
            excel.Sheets[0].Rows[25].Cells[0].Value.ShouldBe(11);
            excel.Sheets[0].Rows[45].Cells[0].Value.ShouldBe(21);

        }
    }
}