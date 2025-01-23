namespace SingularFrameworkCore.DataProcessor;

public interface IDataProcessorLayer<T>
{
    T Process(T input);
    T Reverse(T input);
}
