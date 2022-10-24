namespace Cosmos.Reflection.ObjectVisitors.Internals.Repeat;

internal class ObjectRepeater : IObjectRepeater
{
    private HistoricalContext NormalHistoricalContext { get; set; }

    public ObjectRepeater(HistoricalContext context)
    {
        NormalHistoricalContext = context ?? throw new ArgumentNullException(nameof(context));
    }

    public object Play(object originalObj)
    {
        return NormalHistoricalContext.Repeat(originalObj);
    }

    public object Play(IDictionary<string, object> keyValueCollections)
    {
        return NormalHistoricalContext.Repeat(keyValueCollections);
    }

    public object NewAndPlay()
    {
        return NormalHistoricalContext.Repeat();
    }
}

internal class ObjectRepeater<T> : IObjectRepeater<T>
{
    private HistoricalContext<T> GenericHistoricalContext { get; set; }

    public ObjectRepeater(HistoricalContext<T> context)
    {
        GenericHistoricalContext = context ?? throw new ArgumentNullException(nameof(context));
    }

    public T Play(T originalObj)
    {
        return GenericHistoricalContext.Repeat(originalObj);
    }

    public T Play(IDictionary<string, object> keyValueCollections)
    {
        return GenericHistoricalContext.Repeat(keyValueCollections);
    }

    object IObjectRepeater.Play(object originalObj)
    {
        return GenericHistoricalContext.Repeat(originalObj);
    }

    object IObjectRepeater.Play(IDictionary<string, object> keyValueCollections)
    {
        return GenericHistoricalContext.Repeat(keyValueCollections);
    }

    public T NewAndPlay()
    {
        return GenericHistoricalContext.Repeat();
    }

    object IObjectRepeater.NewAndPlay()
    {
        return ((HistoricalContext) GenericHistoricalContext).Repeat();
    }
}

internal class EmptyRepeater : IObjectRepeater
{
    private readonly Type _sourceType;

    public EmptyRepeater(Type sourceType)
    {
        _sourceType = sourceType;
    }

    public object Play(object originalObj) => originalObj;

    public object Play(IDictionary<string, object> keyValueCollections) => default;

    public object NewAndPlay() => Activator.CreateInstance(_sourceType);
}

internal class EmptyRepeater<T> : IObjectRepeater<T>
{
    public T Play(T originalObj) => originalObj;

    public T Play(IDictionary<string, object> keyValueCollections) => default;

    object IObjectRepeater.Play(object originalObj) => originalObj;

    object IObjectRepeater.Play(IDictionary<string, object> keyValueCollections) => default;

    public T NewAndPlay() => Activator.CreateInstance<T>();

    object IObjectRepeater.NewAndPlay() => Activator.CreateInstance(typeof(T));
}

internal class StaticEmptyRepeater : IObjectRepeater
{
    public object Play(object originalObj) => originalObj;

    public object Play(IDictionary<string, object> keyValueCollections) => default;

    public object NewAndPlay() => null;

    public static StaticEmptyRepeater Instance { get; } = new();
}

internal class StaticEmptyRepeater<T> : IObjectRepeater<T>
{
    public T Play(T originalObj) => originalObj;

    object IObjectRepeater.Play(object originalObj) => originalObj;

    public T Play(IDictionary<string, object> keyValueCollections) => default;

    object IObjectRepeater.Play(IDictionary<string, object> keyValueCollections) => default;

    public T NewAndPlay() => default;

    object IObjectRepeater.NewAndPlay() => null;

    public static StaticEmptyRepeater<T> Instance { get; } = new();
}