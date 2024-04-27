using CsvHelper.Configuration;

namespace Application;

public class ExpenseRecordMapper : ClassMap<ExpenseRecord>
{
    public ExpenseRecordMapper()
    {
        Map(m => m.Date).Index(0).Name("Date");
        Map(m => m.Category).Index(1).Name("Category");
        Map(m => m.Price).Index(2).Name("Price");
        Map(m => m.Notes).Index(3).Name("Notes");
    }
}
