using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Application.Interfaces;
using CsvHelper;
using CsvHelper.Configuration;
using FluentResults;

namespace Application.Converters;

public partial class ActivoBankConverter : IConverter<Result<ActivoBankResponse>>
{
    public string Name => nameof(ActivoBankConverter);

    public async Task<Result<ActivoBankResponse>> Convert(string path)
    {
        try
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(path);

            var writePath = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/activobank_expenses.csv";

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ";",
                Encoding = Encoding.UTF8,
                HeaderValidated = null,
                MissingFieldFound = null
            };

            using var reader = new StreamReader(path);
            using var csv = new CsvReader(reader, config);

            var expensesRecord = new List<ExpenseRecord>();

            await foreach (var record in csv.GetRecordsAsync<ActivoBankRecord>())
            {
                System.Console.WriteLine(JsonSerializer.Serialize(record));

                if (record is null ||
                record.Date == default ||
                string.IsNullOrWhiteSpace(record.Description))
                {
                    continue;
                }

                expensesRecord.Add(new ExpenseRecord(
                                    record.Date.ToString("yyyy-MM-dd"),
                                    "No Category", // TODO: Add a category mapper
                                    Math.Round(record.Value, 2),
                                    ExtraSpaceRegex().Replace(record.Description, " ")));
            }

            using (var writer = new StreamWriter(writePath))
            using (var writerCsv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                writerCsv.WriteHeader<ExpenseRecord>();
                writerCsv.NextRecord();

                foreach (var expenseRecord in expensesRecord)
                {
                    writerCsv.WriteRecord(expenseRecord);
                    writerCsv.NextRecord();
                }
            }

            return Result.Ok(new ActivoBankResponse(writePath));
        }
        catch (Exception exception)
        {
            return Result.Fail(exception.ToString());
        }
    }

    [GeneratedRegex(@"\s+")]
    private static partial Regex ExtraSpaceRegex();
}
