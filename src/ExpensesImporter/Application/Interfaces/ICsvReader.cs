namespace Application;

public interface ICsvReader
{
    Task<List<T>> ReadFile<T>(
        string filePath,
        CsvOptions csvOptions);
}
