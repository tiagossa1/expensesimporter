using System.Globalization;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using ThirdPartyCsvReader = CsvHelper.CsvReader;

namespace Application;

public partial class CsvReader : ICsvReader
{
    private readonly ILogger<CsvReader> _logger;

    public CsvReader(ILogger<CsvReader> logger)
    {
        _logger = logger;
    }

    public async Task<List<T>> ReadFile<T>(
        string filePath,
        CsvOptions csvOptions)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = csvOptions.HasHeaderRecord,
            Delimiter = csvOptions.Delimiter,
            Encoding = csvOptions.Encoding,
            HeaderValidated = null,
            MissingFieldFound = null
        };

        using var reader = new StreamReader(filePath);
        using var csv = new ThirdPartyCsvReader(reader, config);

        var records = new List<T>();

        await foreach (var record in csv.GetRecordsAsync<T>())
        {
            _logger.LogInformation("{className}.{methodName}: processing {record}", nameof(CsvReader), nameof(ReadFile), record);

            records.Add(record);
        }

        return records;
    }
}
