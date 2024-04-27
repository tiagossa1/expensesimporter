namespace Application.Interfaces;

public interface IConverter<T>
{
    string Name { get; }

    Task<T> Convert(string path);
}
