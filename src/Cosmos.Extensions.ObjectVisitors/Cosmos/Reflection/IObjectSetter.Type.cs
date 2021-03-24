using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Cosmos.Reflection
{
    public interface IObjectSetter
    {
        object Instance { get; }
        
        void SetValue(string memberName, object value);

        void SetValue<TObj>(Expression<Func<TObj, object>> expression, object value);

        void SetValue<TObj, TValue>(Expression<Func<TObj, TValue>> expression, TValue value);

        void SetValue(IDictionary<string, object> keyValueCollection);
        
        bool Contains(string memberName);
    }
    
    public interface IObjectSetter<T>
    {
        T Instance { get; }
        
        void SetValue(string memberName, object value);
        
        void SetValue(Expression<Func<T, object>> expression, object value);

        void SetValue<TValue>(Expression<Func<T, TValue>> expression, TValue value);

        void SetValue<TObj>(Expression<Func<TObj, object>> expression, object value);

        void SetValue<TObj, TValue>(Expression<Func<TObj, TValue>> expression, TValue value);

        void SetValue(IDictionary<string, object> keyValueCollection);
        
        bool Contains(string memberName);
    }
}