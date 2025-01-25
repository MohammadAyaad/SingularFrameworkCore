namespace SingularFrameworkCore.Repository;

public interface ISingularCrudRepository<T>
{
    void Create(T entity);
    T Read();
    void Update(T newEntity);
    void Delete();
}
