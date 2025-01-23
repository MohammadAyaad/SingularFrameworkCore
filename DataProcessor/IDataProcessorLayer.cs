namespace SingularFrameworkCore.DataProcessor;

public interface IDataProcessorLayer<I, O>
{
    I InputPreProcess(I input);
    O InputPostProcess(O input);
    I OutputPostProcessor(I input);
    O OutputPreProcessor(O input);
}
