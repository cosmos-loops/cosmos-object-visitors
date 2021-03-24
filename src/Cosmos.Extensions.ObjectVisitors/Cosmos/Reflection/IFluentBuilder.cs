using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Cosmos.Reflection.Internals;

namespace Cosmos.Reflection
{
    public interface IFluentGetter
    {
        IObjectGetter Instance(object instance);

        IObjectGetter InitialValues(IDictionary<string, object> initialValues);

        IFluentValueGetter Value(PropertyInfo propertyInfo);

        IFluentValueGetter Value(FieldInfo fieldInfo);

        IFluentValueGetter Value(string memberName);
    }

    public interface IFluentGetter<T>
    {
        IObjectGetter<T> Instance(T instance);

        IObjectGetter<T> InitialValues(IDictionary<string, object> initialValues);

        IFluentValueGetter<T> Value(PropertyInfo propertyInfo);

        IFluentValueGetter<T> Value(FieldInfo fieldInfo);

        IFluentValueGetter<T> Value(string memberName);

        IFluentValueGetter<T> Value(Expression<Func<T, object>> expression);

        IFluentValueGetter<T, TVal> Value<TVal>(Expression<Func<T, TVal>> expression);
    }

    public interface IFluentValueGetter
    {
        IObjectValueGetter Instance(object instance);
    }

    public interface IFluentValueGetter<T>
    {
        IObjectValueGetter<T> Instance(T instance);
    }

    public interface IFluentValueGetter<T, out TVal>
    {
        IObjectValueGetter<T, TVal> Instance(T instance);
    }

    public interface IFluentSetter
    {
        IObjectSetter NewInstance(bool strictMode = StMode.NORMALE);

        IObjectSetter Instance(object instance, bool strictMode = StMode.NORMALE);

        IObjectSetter InitialValues(IDictionary<string, object> initialValues, bool strictMode = StMode.NORMALE);

        IFluentValueSetter Value(PropertyInfo propertyInfo);

        IFluentValueSetter Value(FieldInfo fieldInfo);

        IFluentValueSetter Value(string memberName);
    }

    public interface IFluentSetter<T>
    {
        IObjectSetter<T> NewInstance(bool strictMode = StMode.NORMALE);

        IObjectSetter<T> Instance(T instance, bool strictMode = StMode.NORMALE);

        IObjectSetter<T> InitialValues(IDictionary<string, object> initialValues, bool strictMode = StMode.NORMALE);

        IFluentValueSetter<T> Value(PropertyInfo propertyInfo);

        IFluentValueSetter<T> Value(FieldInfo fieldInfo);

        IFluentValueSetter<T> Value(string memberName);

        IFluentValueSetter<T> Value(Expression<Func<T, object>> expression);

        IFluentValueSetter<T, TVal> Value<TVal>(Expression<Func<T, TVal>> expression);
    }

    public interface IFluentValueSetter
    {
        IObjectValueSetter Instance(object instance, bool strictMode = StMode.NORMALE);
    }

    public interface IFluentValueSetter<T>
    {
        IObjectValueSetter<T> Instance(T instance, bool strictMode = StMode.NORMALE);
    }

    public interface IFluentValueSetter<T, in TVal>
    {
        IObjectValueSetter<T, TVal> Instance(T instance, bool strictMode = StMode.NORMALE);
    }
}