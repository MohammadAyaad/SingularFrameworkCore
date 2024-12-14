using System.IO;
using SingularFrameworkCore.Repository;

namespace SingularFrameworkCore.Integration.Repository.Files;

public class TextFileRepository : ISingularCrudRepository<string>
{
    public string Path { get; }

    public TextFileRepository(string path)
    {
        this.Path = path;
    }

    public async Task Create(string entity)
    {
        if (!File.Exists(this.Path))
        {
            File.Create(this.Path);
            await File.WriteAllTextAsync(this.Path, entity);
        }
        else
            throw new TextFileRepositoryFileAlreadyExistsException("File already exists");
    }

    public Task Delete()
    {
        if (File.Exists(this.Path))
            File.Delete(this.Path);
        return Task.CompletedTask;
    }

    public Task<string> Read()
    {
        return File.ReadAllTextAsync(this.Path);
    }

    public Task Update(string newEntity)
    {
        return File.WriteAllTextAsync(this.Path, newEntity);
    }
}

[System.Serializable]
public class TextFileRepositoryFileAlreadyExistsException : System.Exception
{
    public TextFileRepositoryFileAlreadyExistsException() { }

    public TextFileRepositoryFileAlreadyExistsException(string message)
        : base(message) { }

    public TextFileRepositoryFileAlreadyExistsException(string message, System.Exception inner)
        : base(message, inner) { }

    protected TextFileRepositoryFileAlreadyExistsException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context
    ) { }
}
