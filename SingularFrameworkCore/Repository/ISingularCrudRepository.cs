namespace SingularFrameworkCore.Repository;

public interface ISingularCrudRepository<T>
{
    Task Create(T entity);
    Task<T> Read();
    Task Update(T newEntity);
    Task Delete();
}
