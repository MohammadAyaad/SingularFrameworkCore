using SingularFrameworkCore.DataProcessor;
using SingularFrameworkCore.Repository;
using SingularFrameworkCore.Serialization;

namespace SingularFrameworkCore;

public class Singular<I, O> : ISingularCrudRepository<I>
{
    private ISingularCrudRepository<O> _repository;
    private IEntitySerializer<I, O> _serializer;
    public List<IDataProcessorLayer<I, O>> _processors;

    public Singular(
        ISingularCrudRepository<O> repository,
        IEntitySerializer<I, O> serializer,
        List<IDataProcessorLayer<I, O>> processors
    )
    {
        _repository = repository;
        _serializer = serializer;
        _processors = processors;
    }

    private O OutputPipeline(I input)
    {
        var c = input;
        for (int i = 0; i < _processors.Count; i++)
        {
            c = _processors[i].InputPreProcess(c);
        }
        var s = _serializer.Serialize(c);
        for (int i = 0; i < _processors.Count; i++)
        {
            s = _processors[i].InputPostProcess(s);
        }
        return s;
    }

    private I InputPipeline(O input)
    {
        var c = input;
        for (int i = 0; i < _processors.Count; i++)
        {
            c = _processors[i].OutputPreProcessor(c);
        }
        var s = _serializer.Deserialize(c);
        for (int i = 0; i < _processors.Count; i++)
        {
            s = _processors[i].OutputPostProcessor(s);
        }
        return s;
    }

    public async Task Create(I entity)
    {
        await _repository.Create(this.OutputPipeline(entity));
    }

    public async Task<I> Read()
    {
        return this.InputPipeline(await _repository.Read());
    }

    public async Task Update(I newEntity)
    {
        await _repository.Update(this.OutputPipeline(newEntity));
    }

    public async Task Delete()
    {
        await _repository.Delete();
    }
}
