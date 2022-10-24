using Cosmos.Reflection.ObjectVisitors.Correctness;
using Cosmos.Reflection.ObjectVisitors.Internals;
using Cosmos.Reflection.ObjectVisitors.Metadata;
using Cosmos.Validation;

namespace Cosmos.Reflection.ObjectVisitors;

/// <summary>
/// Object Setter <br />
/// 对象设置器，用于对对象成员的值进行设置
/// </summary>
public static class ObjectSetter
{
    static ObjectSetter()
    {
        CorrectnessVerifiableWallInitializer.InitializeAndPreheating();
        CorrectnessVerifiableWall.InitValidationProvider(ValidationMe.Use(CorrectnessVerifiableWall.GLOBAL_CORRECT_PROVIDER_KEY));
    }

    /// <summary>
    /// Set type <br />
    /// 设置类型
    /// </summary>
    /// <param name="type"></param>
    /// <param name="kind"></param>
    /// <returns></returns>
    public static IFluentSetter Type(Type type, AlgorithmKind kind = AlgorithmKind.Precision)
        => new FluentSetterBuilder(type, ObjectVisitorOptions.Default.With(x => x.AlgorithmKind = kind));

    /// <summary>
    /// Set type <br />
    /// 设置类型
    /// </summary>
    /// <param name="type"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static IFluentSetter Type(Type type, ObjectVisitorOptions options)
        => new FluentSetterBuilder(type, options?? ObjectVisitorOptions.Default);

    /// <summary>
    /// Set type <br />
    /// 设置类型
    /// </summary>
    /// <param name="kind"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IFluentSetter<T> Type<T>(AlgorithmKind kind = AlgorithmKind.Precision)
        => new FluentSetterBuilder<T>(ObjectVisitorOptions.Default.With(x => x.AlgorithmKind = kind));

    /// <summary>
    /// Set type <br />
    /// 设置类型
    /// </summary>
    /// <param name="options"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IFluentSetter<T> Type<T>(ObjectVisitorOptions options)
        => new FluentSetterBuilder<T>(options?? ObjectVisitorOptions.Default);

    /// <summary>
    /// To touch a ValueSetter <br />
    /// 获取值设置器
    /// </summary>
    public static IPropertyValueSetter ValueSetter => new ObjectPropertyValueSetter();
}