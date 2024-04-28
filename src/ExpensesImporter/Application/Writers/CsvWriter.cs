using System.Globalization;
using Microsoft.Extensions.Logging;
using ThirdPartyCsvWriter = CsvHelper.CsvWriter;

namespace Application;

public class CsvWriter : ICsvWriter
{
    private readonly ILogger<CsvWriter> _logger;

    public CsvWriter(ILogger<CsvWriter> logger)
    {
        _logger = logger;
    }

    public void WriteToFile<T>(string filePath, List<T> records)
    {
        using var writer = new StreamWriter(filePath);
        using var writerCsv = new ThirdPartyCsvWriter(writer, CultureInfo.InvariantCulture);

        writerCsv.WriteHeader<ExpenseRecord>();
        writerCsv.NextRecord();

        foreach (var record in records)
        {
            _logger.LogInformation("{className}.{methodName}: writing {record}", nameof(WriteToFile), nameof(Convert), record);

            writerCsv.WriteRecord(record);
            writerCsv.NextRecord();
        }
    }
}
