using System.Text;

namespace Application;

public record CsvOptions(
    bool HasHeaderRecord,
    string Delimiter,
    Encoding Encoding
);
