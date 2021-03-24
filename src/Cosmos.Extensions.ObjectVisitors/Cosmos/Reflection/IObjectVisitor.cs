using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Cosmos.Reflection.Correctness;
using Cosmos.Reflection.Metadata;
using Cosmos.Validation;

namespace Cosmos.Reflection
{
    public interface IObjectVisitor
    {
        Type SourceType { get; }

        bool IsStatic { get; }

        AlgorithmKind AlgorithmKind { get; }

        object Instance { get; }

        IValidationEntry VerifiableEntry { get; }

        VerifyResult Verify();

        void VerifyAndThrow();

        void SetValue(string memberName, object value);

        void SetValue<TObj>(Expression<Func<TObj, object>> expression, object value);

        void SetValue<TObj, TValue>(Expression<Func<TObj, TValue>> expression, TValue value);

        void SetValue(IDictionary<string, object> keyValueCollection);

        object GetValue(string memberName);

        TValue GetValue<TValue>(string memberName);

        object GetValue<TObj>(Expression<Func<TObj, object>> expression);

        TValue GetValue<TObj, TValue>(Expression<Func<TObj, TValue>> expression);

        object this[string memberName] { get; set; }

        IEnumerable<string> GetMemberNames();

        ObjectMember GetMember(string memberName);

        bool Contains(string memberName);
    }
    
    public interface IObjectVisitor<T> : IObjectVisitor
    {
        new T Instance { get; }

        new IValidationEntry<T> VerifiableEntry { get; }

        void SetValue(Expression<Func<T, object>> expression, object value);

        void SetValue<TValue>(Expression<Func<T, TValue>> expression, TValue value);

        object GetValue(Expression<Func<T, object>> expression);

        TValue GetValue<TValue>(Expression<Func<T, TValue>> expression);

        ObjectMember GetMember<TValue>(Expression<Func<T, TValue>> expression);
    }
}