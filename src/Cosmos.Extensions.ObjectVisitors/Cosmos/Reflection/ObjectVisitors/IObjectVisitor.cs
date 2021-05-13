using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Cosmos.Reflection.ObjectVisitors.Correctness;
using Cosmos.Reflection.ObjectVisitors.Metadata;
using Cosmos.Validation;

namespace Cosmos.Reflection.ObjectVisitors
{
    public interface IObjectVisitor
    {
        Type SourceType { get; }

        bool IsStatic { get; }

        AlgorithmKind AlgorithmKind { get; }

        object Instance { get; }

        IValidationEntry VerifiableEntry { get; }

        VerifyResult Verify(bool withGlobalRules = false);

        void VerifyAndThrow(bool withGlobalRules = false);

        void SetValue(string memberName, object value, bool validationWithGlobalRules = false);

        void SetValue<TObj>(Expression<Func<TObj, object>> expression, object value, bool validationWithGlobalRules = false);

        void SetValue<TObj, TValue>(Expression<Func<TObj, TValue>> expression, TValue value, bool validationWithGlobalRules = false);

        void SetValue(IDictionary<string, object> keyValueCollection, bool validationWithGlobalRules = false);

        object GetValue(string memberName);

        TValue GetValue<TValue>(string memberName);

        object GetValue<TObj>(Expression<Func<TObj, object>> expression);

        TValue GetValue<TObj, TValue>(Expression<Func<TObj, TValue>> expression);

        object this[string memberName] { get; set; }

        IEnumerable<string> GetMemberNames();

        ObjectMember GetMember(string memberName);

        bool Contains(string memberName);

        IPropertyValueAccessor ToValueAccessor();
    }
    
    public interface IObjectVisitor<T> : IObjectVisitor
    {
        new T Instance { get; }

        new IValidationEntry<T> VerifiableEntry { get; }

        void SetValue(Expression<Func<T, object>> expression, object value, bool validationWithGlobalRules = false);

        void SetValue<TValue>(Expression<Func<T, TValue>> expression, TValue value, bool validationWithGlobalRules = false);

        object GetValue(Expression<Func<T, object>> expression);

        TValue GetValue<TValue>(Expression<Func<T, TValue>> expression);

        ObjectMember GetMember<TValue>(Expression<Func<T, TValue>> expression);
    }
}