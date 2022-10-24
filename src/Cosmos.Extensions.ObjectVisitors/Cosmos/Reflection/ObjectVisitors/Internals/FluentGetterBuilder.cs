using System.Linq.Expressions;
using System.Reflection;
using Cosmos.Reflection.ObjectVisitors.Internals.Members;

namespace Cosmos.Reflection.ObjectVisitors.Internals;

internal class FluentGetterBuilder : IFluentGetter, IFluentValueGetter
{
    private readonly Type _type;
    private readonly ObjectVisitorOptions _options;

    public FluentGetterBuilder(Type type, ObjectVisitorOptions options)
    {
        _type = type;
        _options = options;
    }

    #region Fluent building methods for Instance Getter

    IObjectGetter IFluentGetter.Instance(object instance)
    {
        var clone = _options
                    .Clone(x => x.LiteMode = LvMode.LITE)
                    .With(x => x.StrictMode = StMode.NORMALE);
        if (_type.IsAbstract && _type.IsSealed)
            return Cosmos.Reflection.ObjectVisitors.Internals.ObjectVisitorFactoryCore.CreateForStaticType(_type, clone);
        return Cosmos.Reflection.ObjectVisitors.Internals.ObjectVisitorFactoryCore.CreateForInstance(_type, instance, clone.With(x => x.Repeatable = RpMode.NON_REPEATABLE));
    }

    IObjectGetter IFluentGetter.InitialValues(IDictionary<string, object> initialValues)
    {
        var clone = _options
                    .Clone(x => x.LiteMode = LvMode.LITE)
                    .With(x => x.StrictMode = StMode.NORMALE);
        if (_type.IsAbstract && _type.IsSealed)
        {
            var visitor = Cosmos.Reflection.ObjectVisitors.Internals.ObjectVisitorFactoryCore.CreateForStaticType(_type, clone);
            visitor.SetValue(initialValues);
            return visitor;
        }

        return Cosmos.Reflection.ObjectVisitors.Internals.ObjectVisitorFactoryCore.CreateForFutureInstance(_type, _options.With(x => x.Repeatable = RpMode.NON_REPEATABLE), initialValues);
    }

    #endregion

    #region Fluent building methods for Value Getter

    private Func<object, ValueGetter> _func1;

    IFluentValueGetter IFluentGetter.Value(PropertyInfo propertyInfo)
    {
        if (propertyInfo is null)
            throw new ArgumentNullException(nameof(propertyInfo));

        _func1 = t =>
        {
            var clone = _options
                        .Clone(x => x.LiteMode = LvMode.LITE)
                        .With(x => x.Repeatable = RpMode.NON_REPEATABLE)
                        .With(x => x.StrictMode = StMode.NORMALE);
            var visitor = Cosmos.Reflection.ObjectVisitors.Internals.ObjectVisitorFactoryCore.CreateForInstance(_type, t, clone);
            return new ValueGetter(visitor, propertyInfo.Name);
        };

        return this;
    }

    IFluentValueGetter IFluentGetter.Value(FieldInfo fieldInfo)
    {
        if (fieldInfo is null)
            throw new ArgumentNullException(nameof(fieldInfo));

        _func1 = t =>
        {
            var clone = _options
                        .Clone(x => x.LiteMode = LvMode.LITE)
                        .With(x => x.Repeatable = RpMode.NON_REPEATABLE)
                        .With(x => x.StrictMode = StMode.NORMALE);
            var visitor = Cosmos.Reflection.ObjectVisitors.Internals.ObjectVisitorFactoryCore.CreateForInstance(_type, t, clone);
            return new ValueGetter(visitor, fieldInfo.Name);
        };

        return this;
    }

    IFluentValueGetter IFluentGetter.Value(string memberName)
    {
        if (string.IsNullOrWhiteSpace(memberName))
            throw new ArgumentNullException(nameof(memberName));

        _func1 = t =>
        {
            var clone = _options
                        .Clone(x => x.LiteMode = LvMode.LITE)
                        .With(x => x.Repeatable = RpMode.NON_REPEATABLE)
                        .With(x => x.StrictMode = StMode.NORMALE);
            var visitor = Cosmos.Reflection.ObjectVisitors.Internals.ObjectVisitorFactoryCore.CreateForInstance(_type, t, clone);
            return new ValueGetter(visitor, memberName);
        };

        return this;
    }

    IObjectValueGetter IFluentValueGetter.Instance(object instance)
    {
        return _func1.Invoke(instance);
    }

    #endregion
}

internal class FluentGetterBuilder<T> : IFluentGetter<T>, IFluentValueGetter<T>
{
    private readonly Type _type;
    private readonly ObjectVisitorOptions _options;

    public FluentGetterBuilder(ObjectVisitorOptions options)
    {
        _type = typeof(T);
        _options = options;
    }

    #region Fluent building methods for Instance Getter

    IObjectGetter<T> IFluentGetter<T>.Instance(T instance)
    {
        var clone = _options
                    .Clone(x => x.LiteMode = LvMode.LITE)
                    .With(x => x.StrictMode = StMode.NORMALE);
        if (_type.IsAbstract && _type.IsSealed)
            return Cosmos.Reflection.ObjectVisitors.Internals.ObjectVisitorFactoryCore.CreateForStaticType<T>(clone);
        return Cosmos.Reflection.ObjectVisitors.Internals.ObjectVisitorFactoryCore.CreateForInstance<T>(instance, clone.With(x=>x.Repeatable = RpMode.NON_REPEATABLE));
    }

    IObjectGetter<T> IFluentGetter<T>.InitialValues(IDictionary<string, object> initialValues)
    {
        var clone = _options
                    .Clone(x => x.LiteMode = LvMode.LITE)
                    .With(x => x.StrictMode = StMode.NORMALE);
        if (_type.IsAbstract && _type.IsSealed)
        {
            var visitor = Cosmos.Reflection.ObjectVisitors.Internals.ObjectVisitorFactoryCore.CreateForStaticType<T>(clone);
            visitor.SetValue(initialValues);
            return visitor;
        }

        return Cosmos.Reflection.ObjectVisitors.Internals.ObjectVisitorFactoryCore.CreateForFutureInstance<T>(clone.With(x => x.Repeatable = RpMode.NON_REPEATABLE), initialValues);
    }

    #endregion

    #region Fluent building methods for Value Getter

    private Func<T, ValueGetter<T>> _func1;

    IFluentValueGetter<T> IFluentGetter<T>.Value(PropertyInfo propertyInfo)
    {
        if (propertyInfo is null)
            throw new ArgumentNullException(nameof(propertyInfo));

        _func1 = t =>
        {
            var clone = _options
                        .Clone(x => x.LiteMode = LvMode.LITE)
                        .With(x => x.Repeatable = RpMode.NON_REPEATABLE)
                        .With(x => x.StrictMode = StMode.NORMALE);
            var visitor = Cosmos.Reflection.ObjectVisitors.Internals.ObjectVisitorFactoryCore.CreateForInstance<T>(t, clone);
            return new ValueGetter<T>(visitor, propertyInfo.Name);
        };

        return this;
    }

    IFluentValueGetter<T> IFluentGetter<T>.Value(FieldInfo fieldInfo)
    {
        if (fieldInfo is null)
            throw new ArgumentNullException(nameof(fieldInfo));

        _func1 = t =>
        {
            var clone = _options
                        .Clone(x => x.LiteMode = LvMode.LITE)
                        .With(x => x.Repeatable = RpMode.NON_REPEATABLE)
                        .With(x => x.StrictMode = StMode.NORMALE);
            var visitor = Cosmos.Reflection.ObjectVisitors.Internals.ObjectVisitorFactoryCore.CreateForInstance<T>(t, clone);
            return new ValueGetter<T>(visitor, fieldInfo.Name);
        };

        return this;
    }

    IFluentValueGetter<T> IFluentGetter<T>.Value(string memberName)
    {
        if (string.IsNullOrWhiteSpace(memberName))
            throw new ArgumentNullException(nameof(memberName));

        _func1 = t =>
        {
            var clone = _options
                        .Clone(x => x.LiteMode = LvMode.LITE)
                        .With(x => x.Repeatable = RpMode.NON_REPEATABLE)
                        .With(x => x.StrictMode = StMode.NORMALE);
            var visitor = Cosmos.Reflection.ObjectVisitors.Internals.ObjectVisitorFactoryCore.CreateForInstance<T>(t, clone);
            return new ValueGetter<T>(visitor, memberName);
        };

        return this;
    }

    IFluentValueGetter<T> IFluentGetter<T>.Value(Expression<Func<T, object>> expression)
    {
        if (expression is null)
            throw new ArgumentNullException(nameof(expression));

        _func1 = t =>
        {
            var clone = _options
                        .Clone(x => x.LiteMode = LvMode.LITE)
                        .With(x => x.Repeatable = RpMode.NON_REPEATABLE)
                        .With(x => x.StrictMode = StMode.NORMALE);
            var visitor = Cosmos.Reflection.ObjectVisitors.Internals.ObjectVisitorFactoryCore.CreateForInstance<T>(t, clone);
            return new ValueGetter<T>(visitor, expression);
        };

        return this;
    }

    IFluentValueGetter<T, TVal> IFluentGetter<T>.Value<TVal>(Expression<Func<T, TVal>> expression)
    {
        if (expression is null)
            throw new ArgumentNullException(nameof(expression));

        Func<T, ValueGetter<T, TVal>> func = t =>
        {
            var clone = _options
                        .Clone(x => x.LiteMode = LvMode.LITE)
                        .With(x => x.Repeatable = RpMode.NON_REPEATABLE)
                        .With(x => x.StrictMode = StMode.NORMALE);
            var visitor = Cosmos.Reflection.ObjectVisitors.Internals.ObjectVisitorFactoryCore.CreateForInstance<T>(t, clone);
            return new ValueGetter<T, TVal>(visitor, expression);
        };

        return new FluentGetterBuilder<T, TVal>(func);
    }

    IObjectValueGetter<T> IFluentValueGetter<T>.Instance(T instance)
    {
        return _func1.Invoke(instance);
    }

    #endregion
}

internal class FluentGetterBuilder<T, TVal> : IFluentValueGetter<T, TVal>
{
    public FluentGetterBuilder(Func<T, ValueGetter<T, TVal>> func)
    {
        _func1 = func;
    }

    #region Fluent building methods for Value Getter

    private Func<T, ValueGetter<T, TVal>> _func1;

    IObjectValueGetter<T, TVal> IFluentValueGetter<T, TVal>.Instance(T instance)
    {
        return _func1.Invoke(instance);
    }

    #endregion
}