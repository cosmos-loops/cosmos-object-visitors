using Cosmos.Reflection.ObjectVisitors.Metadata;

namespace Cosmos.Reflection.ObjectVisitors;

/// <summary>
/// Object instance creator <br />
/// 对象实例创建器
/// </summary>
public static class ObjectInstanceCreator
{
    /// <summary>
    /// According to the given type, create an object of the corresponding type,
    /// and fill the object with the data provided in the dictionary <br />
    /// 根据给定的类型，创建对应类型的对象，并使用字典中提供的数据填充该对象
    /// </summary>
    /// <param name="type"></param>
    /// <param name="keyValueCollections"></param>
    /// <param name="kind"></param>
    /// <returns></returns>
    public static object Create(Type type, IDictionary<string, object> keyValueCollections, AlgorithmKind kind = AlgorithmKind.Precision)
    {
        return Create(type, keyValueCollections, out _, kind);
    }

    /// <summary>
    /// According to the given type, create an object of the corresponding type,
    /// and fill the object with the data provided in the dictionary <br />
    /// 根据给定的类型，创建对应类型的对象，并使用字典中提供的数据填充该对象
    /// </summary>
    /// <param name="type"></param>
    /// <param name="keyValueCollections"></param>
    /// <param name="visitor"></param>
    /// <param name="kind"></param>
    /// <returns></returns>
    public static object Create(Type type, IDictionary<string, object> keyValueCollections, out IObjectVisitor visitor, AlgorithmKind kind = AlgorithmKind.Precision)
    {
        visitor = ObjectVisitor.Create(type, keyValueCollections, kind);
        return visitor.Instance;
    }

    /// <summary>
    /// According to the given type, create an object of the corresponding type,
    /// and fill the object with the data provided in the dictionary <br />
    /// 根据给定的类型，创建对应类型的对象，并使用字典中提供的数据填充该对象
    /// </summary>
    /// <param name="keyValueCollections"></param>
    /// <param name="kind"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T Create<T>(IDictionary<string, object> keyValueCollections, AlgorithmKind kind = AlgorithmKind.Precision)
    {
        return Create<T>(keyValueCollections, out _, kind);
    }

    /// <summary>
    /// According to the given type, create an object of the corresponding type,
    /// and fill the object with the data provided in the dictionary <br />
    /// 根据给定的类型，创建对应类型的对象，并使用字典中提供的数据填充该对象
    /// </summary>
    /// <param name="keyValueCollections"></param>
    /// <param name="visitor"></param>
    /// <param name="kind"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T Create<T>(IDictionary<string, object> keyValueCollections, out IObjectVisitor<T> visitor, AlgorithmKind kind = AlgorithmKind.Precision)
    {
        visitor = Cosmos.Reflection.ObjectVisitors.ObjectVisitor.Create<T>(keyValueCollections, kind);
        return visitor.Instance;
    }
}