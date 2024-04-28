namespace Application;

public interface IFilePathValidator
{
    string Name { get; }

    bool IsValid(string filePath);
}
