using Cosmos.Reflection.ObjectVisitors.Core;
using Cosmos.Reflection.ObjectVisitors.Internals.Visitors;
using Cosmos.Validation.Strategies;

namespace Cosmos.Reflection.ObjectVisitors.Internals;

internal static partial class ObjectVisitorFactoryCore
{
    public static InstanceVisitor CreateForInstance<TStrategy>(Type type, object instance, ObjectVisitorOptions options, TStrategy strategy)
        where TStrategy : class, IValidationStrategy, new()
    {
        options ??= ObjectVisitorOptions.Default;
        var handler = SafeObjectHandleSwitcher.Switch(options.AlgorithmKind)(type).AndSetInstance(instance);
        var visitor = new InstanceVisitor(handler, type, options);
        visitor.VerifiableEntry.SetStrategy(strategy);
        return visitor;
    }

    public static InstanceVisitor<T> CreateForInstance<T, TStrategy>(T instance, ObjectVisitorOptions options)
        where TStrategy : class, IValidationStrategy<T>, new()
    {
        options ??= ObjectVisitorOptions.Default;
        var handler = UnsafeObjectHandleSwitcher.Switch<T>(options.AlgorithmKind)().AndSetInstance(instance).With<T>();
        var visitor = new InstanceVisitor<T>(handler, options);
        visitor.VerifiableEntry.SetStrategy<TStrategy>();
        return visitor;
    }

    public static FutureInstanceVisitor CreateForFutureInstance<TStrategy>(Type type, ObjectVisitorOptions options, TStrategy strategy, IDictionary<string, object> initialValues = null)
        where TStrategy : class, IValidationStrategy, new()
    {
        options ??= ObjectVisitorOptions.Default;
        var handler = SafeObjectHandleSwitcher.Switch(options.AlgorithmKind)(type);
        var visitor = new FutureInstanceVisitor(handler, type, options, initialValues);
        visitor.VerifiableEntry.SetStrategy(strategy);
        return visitor;
    }

    public static FutureInstanceVisitor<T> CreateForFutureInstance<T, TStrategy>(ObjectVisitorOptions options, IDictionary<string, object> initialValues = null)
        where TStrategy : class, IValidationStrategy<T>, new()
    {
        options ??= ObjectVisitorOptions.Default;
        var handler = UnsafeObjectHandleSwitcher.Switch<T>(options.AlgorithmKind)().With<T>();
        var visitor = new FutureInstanceVisitor<T>(handler, options, initialValues);
        visitor.VerifiableEntry.SetStrategy<TStrategy>();
        return visitor;
    }

    public static StaticTypeObjectVisitor CreateForStaticType<TStrategy>(Type type, ObjectVisitorOptions options, TStrategy strategy)
        where TStrategy : class, IValidationStrategy, new()
    {
        options ??= ObjectVisitorOptions.Default;
        var handler = SafeObjectHandleSwitcher.Switch(options.AlgorithmKind)(type);
        var visitor = new StaticTypeObjectVisitor(handler, type, options);
        visitor.VerifiableEntry.SetStrategy(strategy);
        return visitor;
    }

    public static StaticTypeObjectVisitor<T> CreateForStaticType<T, TStrategy>(ObjectVisitorOptions options)
        where TStrategy : class, IValidationStrategy<T>, new()
    {
        options ??= ObjectVisitorOptions.Default;
        var handler = UnsafeObjectHandleSwitcher.Switch<T>(options.AlgorithmKind)().With<T>();
        var visitor = new StaticTypeObjectVisitor<T>(handler, options);
        visitor.VerifiableEntry.SetStrategy<TStrategy>();
        return visitor;
    }
}