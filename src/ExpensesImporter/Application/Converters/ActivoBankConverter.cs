using System.Globalization;
using System.Reflection;
using System.Text;
using Application.Interfaces;
using CsvHelper.Configuration;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace Application.Converters;

public partial class ActivoBankConverter : IConverter<Result<ActivoBankResponse>>
{
    private readonly ILogger<ActivoBankConverter> _logger;
    private readonly ICategoryMapper _categoryMapper;
    private readonly ICsvWriter _csvWriter;
    private readonly ICsvReader _csvReader;

    public ActivoBankConverter(
        ILogger<ActivoBankConverter> logger,
        ICategoryMapper categoryMapper,
        ICsvWriter csvWriter,
        ICsvReader csvReader)
    {
        _logger = logger;
        _categoryMapper = categoryMapper;
        _csvWriter = csvWriter;
        _csvReader = csvReader;
    }

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

            var records = await _csvReader.ReadFile<ActivoBankRecord>(path, new CsvOptions(true, ";", Encoding.UTF8));

            var recordsToDto = records
            ?.Select(record => record.ToExpenseRecord(_categoryMapper.GetCategory(record.Description)))
            .ToList() ?? [];

            _csvWriter.WriteToFile(writePath, recordsToDto);

            return Result.Ok(new ActivoBankResponse(writePath));
        }
        catch (Exception exception)
        {
            _logger.LogError("{className}.{methodName}: there was an error - {exceptionMessage}", nameof(ActivoBankConverter), nameof(Convert), exception.Message);
            return Result.Fail(exception.ToString());
        }
    }
}
