# XLExpression

parse excel formula to C# expression, and compile to executable lambda
将 Excel 中的公式转为 C#的表达式树，然后使用 lambda 执行

# Usage

```C#
var exp = ExpressionBuilder.Instance.Build("IF(F2>G2,1,0)");
var result = exp.Invoke(new { F2 = 1, G2 = 0 });
```

# Develop

formula implments at `XLExpression/Functions/Impl/...`

# Supported function

has implemented:

| function/symbols | Remark                                    |
| ---------------- | ----------------------------------------- |
| +                | add/concat string                         |
| -                | minus                                     |
| \*               | minus                                     |
| /                | divide                                    |
| ^                | pow                                       |
| =                | equal                                     |
| <>               | not equal                                 |
| >                | greater than                              |
| >=               | greater than or equal                     |
| <                | less than                                 |
| <=               | less than or equal                        |
| If               |                                           |
| And              |                                           |
| Or               |                                           |
| Round            |                                           |
| Date             |                                           |
| Sum              | SUM(A1:A9), SUM(A1:D4), SUM(A1:A9, A1:D4) |
| VLookup          |                                           |
| Left             |                                           |
| Right            |                                           |
