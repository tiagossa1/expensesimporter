using System.Collections.ObjectModel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application;

public class CategoryMapper : ICategoryMapper
{
    private const string NoCategory = "No Category";
    private readonly ILogger<CategoryMapper> _logger;

    private readonly ReadOnlyDictionary<string, string> _activoBankMapper;

    public CategoryMapper(
        IOptions<Dictionary<string, string>> servicesAccessor,
        ILogger<CategoryMapper> logger)
    {
        _logger = logger;

        if (servicesAccessor.Value is null ||
        servicesAccessor.Value.Count == 0)
        {
            _logger.LogWarning("{className}: no mapping has been defined - {noCategory} will be used", nameof(CategoryMapper), NoCategory);
        }

        _activoBankMapper = servicesAccessor.Value?.AsReadOnly() ?? new Dictionary<string, string>(0).AsReadOnly();
    }

    public string GetCategory(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            return NoCategory;
        }

        var key = _activoBankMapper.Keys
            .FirstOrDefault(key => description.Contains(key, StringComparison.InvariantCultureIgnoreCase));

        if (string.IsNullOrWhiteSpace(key))
        {
            return NoCategory;
        }

        return _activoBankMapper[key];
    }
}
