namespace SingularFrameworkCore.Serialization;

public interface IEntitySerializer<I, O>
{
    O Serialize(I entity);
    I Deserialize(O entity);
}
