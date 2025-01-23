using SingularFrameworkCore.DataProcessor;
using SingularFrameworkCore.Repository;
using SingularFrameworkCore.Serialization;

namespace SingularFrameworkCore;

public class Singular<I, O> : ISingularCrudRepository<I>
{
    private ISingularCrudRepository<O> _repository;
    private IEntitySerializer<I, O> _serializer;
    public List<IDataProcessorLayer<I>> _preProcessors;
    public List<IDataProcessorLayer<O>> _postProcessors;

    public Singular(
        ISingularCrudRepository<O> repository,
        IEntitySerializer<I, O> serializer,
        List<IDataProcessorLayer<I>> preProcessors,
        List<IDataProcessorLayer<O>> postProcessors
    )
    {
        _repository = repository;
        _serializer = serializer;
        _preProcessors = preProcessors;
        _postProcessors = postProcessors;
    }

    private O OutputPipeline(I input)
    {
        var c = input;
        for (int i = 0; i < _preProcessors.Count; i++)
        {
            c = _preProcessors[i].Process(c);
        }
        var s = _serializer.Serialize(c);
        for (int i = 0; i < _postProcessors.Count; i++)
        {
            s = _postProcessors[i].Process(s);
        }
        return s;
    }

    private I InputPipeline(O input)
    {
        var c = input;
        for (int i = _postProcessors.Count - 1; i >= 0; i--)
        {
            c = _postProcessors[i].Reverse(c);
        }
        var s = _serializer.Deserialize(c);
        for (int i = _preProcessors.Count - 1; i >= 0; i--)
        {
            s = _preProcessors[i].Reverse(s);
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
