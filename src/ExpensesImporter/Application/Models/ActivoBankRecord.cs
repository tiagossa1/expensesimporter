using CsvHelper.Configuration.Attributes;

namespace Application;

public record ActivoBankRecord(
    [Name("Data Valor")] DateTime Date,
    [Name("Descrição")] string Description,
    [Name("Valor")] double Value);
