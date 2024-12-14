using System.IO;
using SingularFrameworkCore.Repository;

namespace SingularFrameworkCore.Integration.Repository.Files;

public class BinaryFileRepository : ISingularCrudRepository<byte[]>
{
    public string Path { get; }

    public BinaryFileRepository(string path)
    {
        this.Path = path;
    }

    public async Task Create(byte[] entity)
    {
        if (!File.Exists(this.Path))
        {
            File.Create(this.Path);
            await File.WriteAllBytesAsync(this.Path, entity);
        }
        else
            throw new BinaryFileRepositoryFileAlreadyExistsException("File already exists");
    }

    public Task Delete()
    {
        if (File.Exists(this.Path))
            File.Delete(this.Path);
        return Task.CompletedTask;
    }

    public Task<byte[]> Read()
    {
        return File.ReadAllBytesAsync(this.Path);
    }

    public Task Update(byte[] newEntity)
    {
        return File.WriteAllBytesAsync(this.Path, newEntity);
    }
}

[System.Serializable]
public class BinaryFileRepositoryFileAlreadyExistsException : System.Exception
{
    public BinaryFileRepositoryFileAlreadyExistsException() { }

    public BinaryFileRepositoryFileAlreadyExistsException(string message)
        : base(message) { }

    public BinaryFileRepositoryFileAlreadyExistsException(string message, System.Exception inner)
        : base(message, inner) { }

    protected BinaryFileRepositoryFileAlreadyExistsException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context
    ) { }
}
