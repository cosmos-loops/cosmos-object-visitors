using System;
using System.Linq.Expressions;

namespace Cosmos.Reflection.ObjectVisitors
{
    public interface IObjectGetter
    {
        object Instance { get; }

        object GetValue(string memberName, AccessMode mode = AccessMode.Concise);

        TValue GetValue<TValue>(string memberName, AccessMode mode = AccessMode.Concise);

        object GetValue<TObj>(Expression<Func<TObj, object>> expression);

        TValue GetValue<TObj, TValue>(Expression<Func<TObj, TValue>> expression);

        bool Contains(string memberName);
    }

    public interface IObjectGetter<T>
    {
        T Instance { get; }

        object GetValue(string memberName, AccessMode mode = AccessMode.Concise);

        TValue GetValue<TValue>(string memberName, AccessMode mode = AccessMode.Concise);

        object GetValue(Expression<Func<T, object>> expression);

        TValue GetValue<TValue>(Expression<Func<T, TValue>> expression);

        object GetValue<TObj>(Expression<Func<TObj, object>> expression);

        TValue GetValue<TObj, TValue>(Expression<Func<TObj, TValue>> expression);

        bool Contains(string memberName);
    }
}