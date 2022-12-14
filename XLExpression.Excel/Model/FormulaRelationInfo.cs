using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XLExpression.Excel.Model
{
    internal class FormulaRelationInfo: FormulaInfo
    {
        public FormulaRelationInfo(FormulaInfo info, ExpressionResult expression)
        {
            this.Formula = info.Formula;
            this.ColIndex = info.ColIndex;
            this.RowIndex = info.RowIndex;
            this.SheetIndex = info.SheetIndex;
            this.SheetName = info.SheetName;

            Expression = expression;
        }

        public ExpressionResult Expression { get; set; }

        public List<FormulaRelationInfo> Relation { get; private set; } = new List<FormulaRelationInfo>();

        public int RelationCount
        {
            get
            {
                var i = Relation.Count;

                if (Relation.Any())
                {
                    foreach (var rel in Relation)
                    {
                        i += rel.RelationCount;
                    }
                }

                return i;
            }
        }
    }
}
