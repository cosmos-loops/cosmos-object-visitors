using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Cosmos.Reflection.ObjectVisitors
{
    public interface IObjectSetter
    {
        object Instance { get; }
        
        void SetValue(string memberName, object value, bool validationWithGlobalRules = false);

        void SetValue<TObj>(Expression<Func<TObj, object>> expression, object value, bool validationWithGlobalRules = false);

        void SetValue<TObj, TValue>(Expression<Func<TObj, TValue>> expression, TValue value, bool validationWithGlobalRules = false);

        void SetValue(IDictionary<string, object> keyValueCollection, bool validationWithGlobalRules = false);
        
        bool Contains(string memberName);
    }
    
    public interface IObjectSetter<T>
    {
        T Instance { get; }
        
        void SetValue(string memberName, object value, bool validationWithGlobalRules = false);
        
        void SetValue(Expression<Func<T, object>> expression, object value, bool validationWithGlobalRules = false);

        void SetValue<TValue>(Expression<Func<T, TValue>> expression, TValue value, bool validationWithGlobalRules = false);

        void SetValue<TObj>(Expression<Func<TObj, object>> expression, object value, bool validationWithGlobalRules = false);

        void SetValue<TObj, TValue>(Expression<Func<TObj, TValue>> expression, TValue value, bool validationWithGlobalRules = false);

        void SetValue(IDictionary<string, object> keyValueCollection, bool validationWithGlobalRules = false);
        
        bool Contains(string memberName);
    }
}