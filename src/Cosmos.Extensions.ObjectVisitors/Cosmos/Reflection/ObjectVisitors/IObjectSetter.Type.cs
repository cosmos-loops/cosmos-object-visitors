using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Cosmos.Reflection.ObjectVisitors
{
    public interface IObjectSetter
    {
        object Instance { get; }

        void SetValue(string memberName, object value, AccessMode mode = AccessMode.Concise);

        void SetValue(string memberName, object value, string globalVerifyProviderName, AccessMode mode = AccessMode.Concise);

        void SetValue<TObj>(Expression<Func<TObj, object>> expression, object value);


        void SetValue<TObj>(Expression<Func<TObj, object>> expression, object value, string globalVerifyProviderName);

        void SetValue<TObj, TValue>(Expression<Func<TObj, TValue>> expression, TValue value);

        void SetValue<TObj, TValue>(Expression<Func<TObj, TValue>> expression, TValue value, string globalVerifyProviderName);

        void SetValue(IDictionary<string, object> keyValueCollection);

        void SetValue(IDictionary<string, object> keyValueCollection, string globalVerifyProviderName);

        bool Contains(string memberName);
    }

    public interface IObjectSetter<T>
    {
        T Instance { get; }

        void SetValue(string memberName, object value, AccessMode mode = AccessMode.Concise);

        void SetValue(string memberName, object value, string globalVerifyProviderName, AccessMode mode = AccessMode.Concise);

        void SetValue(Expression<Func<T, object>> expression, object value);

        void SetValue(Expression<Func<T, object>> expression, object value, string globalVerifyProviderName);

        void SetValue<TValue>(Expression<Func<T, TValue>> expression, TValue value);

        void SetValue<TValue>(Expression<Func<T, TValue>> expression, TValue value, string globalVerifyProviderName);

        void SetValue<TObj>(Expression<Func<TObj, object>> expression, object value);

        void SetValue<TObj>(Expression<Func<TObj, object>> expression, object value, string globalVerifyProviderName);

        void SetValue<TObj, TValue>(Expression<Func<TObj, TValue>> expression, TValue value);

        void SetValue<TObj, TValue>(Expression<Func<TObj, TValue>> expression, TValue value, string globalVerifyProviderName);

        void SetValue(IDictionary<string, object> keyValueCollection);

        void SetValue(IDictionary<string, object> keyValueCollection, string globalVerifyProviderName);

        bool Contains(string memberName);
    }
}