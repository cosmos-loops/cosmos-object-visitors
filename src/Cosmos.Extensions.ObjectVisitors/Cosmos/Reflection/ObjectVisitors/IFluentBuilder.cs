using System.Linq.Expressions;
using System.Reflection;
using Cosmos.Reflection.ObjectVisitors.Internals;

namespace Cosmos.Reflection.ObjectVisitors;

/// <summary>
/// An interface for FluentGetter
/// </summary>
public interface IFluentGetter
{
    /// <summary>
    /// Instance
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    IObjectGetter Instance(object instance);

    /// <summary>
    /// Initial values
    /// </summary>
    /// <param name="initialValues"></param>
    /// <returns></returns>
    IObjectGetter InitialValues(IDictionary<string, object> initialValues);

    /// <summary>
    /// Value
    /// </summary>
    /// <param name="propertyInfo"></param>
    /// <returns></returns>
    IFluentValueGetter Value(PropertyInfo propertyInfo);

    /// <summary>
    /// Value
    /// </summary>
    /// <param name="fieldInfo"></param>
    /// <returns></returns>
    IFluentValueGetter Value(FieldInfo fieldInfo);

    /// <summary>
    /// Value
    /// </summary>
    /// <param name="memberName"></param>
    /// <returns></returns>
    IFluentValueGetter Value(string memberName);
}

/// <summary>
/// An interface for FluentGetter
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IFluentGetter<T>
{
    /// <summary>
    /// Instance
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    IObjectGetter<T> Instance(T instance);

    /// <summary>
    /// Initial values
    /// </summary>
    /// <param name="initialValues"></param>
    /// <returns></returns>
    IObjectGetter<T> InitialValues(IDictionary<string, object> initialValues);

    /// <summary>
    /// Value
    /// </summary>
    /// <param name="propertyInfo"></param>
    /// <returns></returns>
    IFluentValueGetter<T> Value(PropertyInfo propertyInfo);

    /// <summary>
    /// Value
    /// </summary>
    /// <param name="fieldInfo"></param>
    /// <returns></returns>
    IFluentValueGetter<T> Value(FieldInfo fieldInfo);

    /// <summary>
    /// Value
    /// </summary>
    /// <param name="memberName"></param>
    /// <returns></returns>
    IFluentValueGetter<T> Value(string memberName);

    /// <summary>
    /// Value
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    IFluentValueGetter<T> Value(Expression<Func<T, object>> expression);

    /// <summary>
    /// Value
    /// </summary>
    /// <param name="expression"></param>
    /// <typeparam name="TVal"></typeparam>
    /// <returns></returns>
    IFluentValueGetter<T, TVal> Value<TVal>(Expression<Func<T, TVal>> expression);
}

/// <summary>
/// An interface for FluentValueGetter
/// </summary>
public interface IFluentValueGetter
{
    /// <summary>
    /// Instance
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    IObjectValueGetter Instance(object instance);
}

/// <summary>
/// An interface for FluentValueGetter
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IFluentValueGetter<T>
{
    /// <summary>
    /// Instance
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    IObjectValueGetter<T> Instance(T instance);
}

public interface IFluentValueGetter<T, out TVal>
{
    /// <summary>
    /// Instance
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    IObjectValueGetter<T, TVal> Instance(T instance);
}

/// <summary>
/// An interface for FluentSetter
/// </summary>
public interface IFluentSetter
{
    /// <summary>
    /// New Instance
    /// </summary>
    /// <param name="strictMode"></param>
    /// <returns></returns>
    IObjectSetter NewInstance(bool strictMode = StMode.NORMALE);

    /// <summary>
    /// Instance
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="strictMode"></param>
    /// <returns></returns>
    IObjectSetter Instance(object instance, bool strictMode = StMode.NORMALE);

    /// <summary>
    /// Initial values
    /// </summary>
    /// <param name="initialValues"></param>
    /// <param name="strictMode"></param>
    /// <returns></returns>
    IObjectSetter InitialValues(IDictionary<string, object> initialValues, bool strictMode = StMode.NORMALE);

    /// <summary>
    /// Value
    /// </summary>
    /// <param name="propertyInfo"></param>
    /// <returns></returns>
    IFluentValueSetter Value(PropertyInfo propertyInfo);

    /// <summary>
    /// Value
    /// </summary>
    /// <param name="fieldInfo"></param>
    /// <returns></returns>
    IFluentValueSetter Value(FieldInfo fieldInfo);

    /// <summary>
    /// Value
    /// </summary>
    /// <param name="memberName"></param>
    /// <returns></returns>
    IFluentValueSetter Value(string memberName);
}

/// <summary>
/// An interface for FluentSetter
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IFluentSetter<T>
{
    /// <summary>
    /// New Instance
    /// </summary>
    /// <param name="strictMode"></param>
    /// <returns></returns>
    IObjectSetter<T> NewInstance(bool strictMode = StMode.NORMALE);

    /// <summary>
    /// Instance
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="strictMode"></param>
    /// <returns></returns>
    IObjectSetter<T> Instance(T instance, bool strictMode = StMode.NORMALE);

    /// <summary>
    /// Initial values
    /// </summary>
    /// <param name="initialValues"></param>
    /// <param name="strictMode"></param>
    /// <returns></returns>
    IObjectSetter<T> InitialValues(IDictionary<string, object> initialValues, bool strictMode = StMode.NORMALE);

    /// <summary>
    /// Value
    /// </summary>
    /// <param name="propertyInfo"></param>
    /// <returns></returns>
    IFluentValueSetter<T> Value(PropertyInfo propertyInfo);

    /// <summary>
    /// Value
    /// </summary>
    /// <param name="fieldInfo"></param>
    /// <returns></returns>
    IFluentValueSetter<T> Value(FieldInfo fieldInfo);

    /// <summary>
    /// Value
    /// </summary>
    /// <param name="memberName"></param>
    /// <returns></returns>
    IFluentValueSetter<T> Value(string memberName);

    /// <summary>
    /// Value
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    IFluentValueSetter<T> Value(Expression<Func<T, object>> expression);

    /// <summary>
    /// Value
    /// </summary>
    /// <param name="expression"></param>
    /// <typeparam name="TVal"></typeparam>
    /// <returns></returns>
    IFluentValueSetter<T, TVal> Value<TVal>(Expression<Func<T, TVal>> expression);
}

/// <summary>
/// An interface for FluentValueSetter
/// </summary>
public interface IFluentValueSetter
{
    /// <summary>
    /// Instance
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="strictMode"></param>
    /// <returns></returns>
    IObjectValueSetter Instance(object instance, bool strictMode = StMode.NORMALE);
}

/// <summary>
/// An interface for FluentValueSetter
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IFluentValueSetter<T>
{
    /// <summary>
    /// Instance
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="strictMode"></param>
    /// <returns></returns>
    IObjectValueSetter<T> Instance(T instance, bool strictMode = StMode.NORMALE);
}

/// <summary>
/// An interface for FluentValueSetter
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TVal"></typeparam>
public interface IFluentValueSetter<T, in TVal>
{
    /// <summary>
    /// Instance
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="strictMode"></param>
    /// <returns></returns>
    IObjectValueSetter<T, TVal> Instance(T instance, bool strictMode = StMode.NORMALE);
}