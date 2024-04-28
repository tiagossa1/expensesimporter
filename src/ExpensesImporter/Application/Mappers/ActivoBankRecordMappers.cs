using System.Text.RegularExpressions;

namespace Application;

public static partial class ActivoBankRecordMappers
{
    [GeneratedRegex(@"\s+")]
    private static partial Regex ExtraSpacesRegex();

    public static ExpenseRecord ToExpenseRecord(
        this ActivoBankRecord activoBankRecord,
        string category)
    {
        return new ExpenseRecord(
                activoBankRecord.Date.ToString("yyyy-MM-dd"),
                category,
                Math.Round(activoBankRecord.Value, 2),
                ExtraSpacesRegex().Replace(activoBankRecord.Description, " ")
            );
    }
}
