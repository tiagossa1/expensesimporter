using Microsoft.Extensions.Logging;

namespace Application;

public class ActivoBankFilePathValidator : IFilePathValidator
{
    private readonly ILogger<ActivoBankFilePathValidator> _logger;

    public ActivoBankFilePathValidator(ILogger<ActivoBankFilePathValidator> logger)
    {
        _logger = logger;
    }

    public string Name => nameof(ActivoBankFilePathValidator);

    public bool IsValid(string filePath)
    {
        try
        {
            filePath = filePath.Trim();

            if (string.IsNullOrWhiteSpace(filePath))
            {
                return false;
            }

            var fileExists = File.Exists(filePath);
            if (!fileExists)
            {
                return false;
            }

            var fileExtension = Path.GetExtension(filePath);
            if (string.IsNullOrWhiteSpace(fileExtension) ||
                !FileExtensionConstants.Csv.Equals(fileExtension, StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            return true;
        }
        catch (Exception exception)
        {
            _logger.LogError("Cannot check if {filePath} is valid for {name}: {exceptionMessage}", filePath, Name, exception.Message);
            return false;
        }
    }
}
