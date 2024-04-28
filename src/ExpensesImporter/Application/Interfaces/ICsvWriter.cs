namespace Application;

public interface ICsvWriter
{
    void WriteToFile<T>(string filePath, List<T> records);
}
