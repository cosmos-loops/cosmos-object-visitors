using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CosmosStack.Reflection.ObjectVisitors.Correctness;
using CosmosStack.Reflection.ObjectVisitors.Metadata;
using CosmosStack.Validation;

namespace CosmosStack.Reflection.ObjectVisitors
{
    /// <summary>
    /// An interface for ObjectVisitor <br />
    /// 对象访问器接口
    /// </summary>
    public interface IObjectVisitor
    {
        /// <summary>
        /// Source type <br />
        /// 源类型
        /// </summary>
        Type SourceType { get; }

        /// <summary>
        /// Is static class <br />
        /// 是否为静态类型
        /// </summary>
        bool IsStatic { get; }

        /// <summary>
        /// Algorithm kind <br />
        /// 算法种类
        /// </summary>
        AlgorithmKind AlgorithmKind { get; }

        /// <summary>
        /// Gets instance <br />
        /// 获取实例
        /// </summary>
        object Instance { get; }

        /// <summary>
        /// Gets Verifiable entry <br />
        /// 获取验证入口
        /// </summary>
        IValidationEntry VerifiableEntry { get; }

        /// <summary>
        /// Verify <br />
        /// 执行验证
        /// </summary>
        /// <returns></returns>
        VerifyResult Verify();

        /// <summary>
        /// Verify <br />
        /// 执行验证
        /// </summary>
        /// <param name="globalVerifyProviderName"></param>
        /// <returns></returns>
        VerifyResult Verify(string globalVerifyProviderName);

        /// <summary>
        /// Verify and throw if need <br />
        /// 执行验证，如果有异常则抛出
        /// </summary>
        void VerifyAndThrow();

        /// <summary>
        /// Verify and throw if need <br />
        /// 执行验证，如果有异常则抛出
        /// </summary>
        /// <param name="globalVerifyProviderName"></param>
        void VerifyAndThrow(string globalVerifyProviderName);

        /// <summary>
        /// Set value <br />
        /// 设置值
        /// </summary>
        /// <param name="memberName"></param>
        /// <param name="value"></param>
        /// <param name="mode"></param>
        void SetValue(string memberName, object value, AccessMode mode = AccessMode.Concise);

        /// <summary>
        /// Set value <br />
        /// 设置值
        /// </summary>
        /// <param name="memberName"></param>
        /// <param name="value"></param>
        /// <param name="globalVerifyProviderName"></param>
        /// <param name="mode"></param>
        void SetValue(string memberName, object value, string globalVerifyProviderName, AccessMode mode = AccessMode.Concise);

        /// <summary>
        /// Set value <br />
        /// 设置值
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="value"></param>
        /// <typeparam name="TObj"></typeparam>
        void SetValue<TObj>(Expression<Func<TObj, object>> expression, object value);

        /// <summary>
        /// Set value <br />
        /// 设置值
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="value"></param>
        /// <param name="globalVerifyProviderName"></param>
        /// <typeparam name="TObj"></typeparam>
        void SetValue<TObj>(Expression<Func<TObj, object>> expression, object value, string globalVerifyProviderName);

        /// <summary>
        /// Set value <br />
        /// 设置值
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="value"></param>
        /// <typeparam name="TObj"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        void SetValue<TObj, TValue>(Expression<Func<TObj, TValue>> expression, TValue value);

        /// <summary>
        /// Set value <br />
        /// 设置值
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="value"></param>
        /// <param name="globalVerifyProviderName"></param>
        /// <typeparam name="TObj"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        void SetValue<TObj, TValue>(Expression<Func<TObj, TValue>> expression, TValue value, string globalVerifyProviderName);

        /// <summary>
        /// Set value <br />
        /// 设置值
        /// </summary>
        /// <param name="keyValueCollection"></param>
        void SetValue(IDictionary<string, object> keyValueCollection);

        /// <summary>
        /// Set value <br />
        /// 设置值
        /// </summary>
        /// <param name="keyValueCollection"></param>
        /// <param name="globalVerifyProviderName"></param>
        void SetValue(IDictionary<string, object> keyValueCollection, string globalVerifyProviderName);

        /// <summary>
        /// Get value <br />
        /// 获得值
        /// </summary>
        /// <param name="memberName"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        object GetValue(string memberName, AccessMode mode = AccessMode.Concise);

        /// <summary>
        /// Get value <br />
        /// 获得值
        /// </summary>
        /// <param name="memberName"></param>
        /// <param name="mode"></param>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        TValue GetValue<TValue>(string memberName, AccessMode mode = AccessMode.Concise);

        /// <summary>
        /// Get value <br />
        /// 获得值
        /// </summary>
        /// <param name="expression"></param>
        /// <typeparam name="TObj"></typeparam>
        /// <returns></returns>
        object GetValue<TObj>(Expression<Func<TObj, object>> expression);

        /// <summary>
        /// Get value <br />
        /// 获得值
        /// </summary>
        /// <param name="expression"></param>
        /// <typeparam name="TObj"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        TValue GetValue<TObj, TValue>(Expression<Func<TObj, TValue>> expression);

        /// <summary>
        /// Gets or sets value by the given member name <br />
        /// 根据给定的成员名读写值
        /// </summary>
        /// <param name="memberName"></param>
        object this[string memberName] { get; set; }

        /// <summary>
        /// Gets or sets value by the given member name <br />
        /// 根据给定的成员名读写值
        /// </summary>
        /// <param name="memberName"></param>
        /// <param name="mode"></param>
        object this[string memberName, AccessMode mode] { get; set; }

        /// <summary>
        /// Gets all member names <br />
        /// 获取所有成员的名称
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetMemberNames();

        /// <summary>
        /// Get ObjectMember info by the given member name <br />
        /// 根据给定的成员名，获取元信息
        /// </summary>
        /// <param name="memberName"></param>
        /// <returns></returns>
        ObjectMember GetMember(string memberName);

        /// <summary>
        /// Contains <br />
        /// 包含
        /// </summary>
        /// <param name="memberName"></param>
        /// <returns></returns>
        bool Contains(string memberName);

        /// <summary>
        /// Convert the Object Visitor into ValueAccessor <br />
        /// 将对象访问器转换为值访问器
        /// </summary>
        /// <returns></returns>
        IPropertyValueAccessor ToValueAccessor();
    }

    /// <summary>
    /// An interface for ObjectVisitor <br />
    /// 对象访问器接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IObjectVisitor<T> : IObjectVisitor
    {
        /// <summary>
        /// Gets instance <br />
        /// 获取实例
        /// </summary>
        new T Instance { get; }

        /// <summary>
        /// Gets Verifiable entry <br />
        /// 获取验证入口
        /// </summary>
        new IValidationEntry<T> VerifiableEntry { get; }

        /// <summary>
        /// Set value <br />
        /// 设置值
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="value"></param>
        void SetValue(Expression<Func<T, object>> expression, object value);

        /// <summary>
        /// Set value <br />
        /// 设置值
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="value"></param>
        /// <param name="validationWithGlobalRules"></param>
        void SetValue(Expression<Func<T, object>> expression, object value, string validationWithGlobalRules);

        /// <summary>
        /// Set value <br />
        /// 设置值
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="value"></param>
        /// <typeparam name="TValue"></typeparam>
        void SetValue<TValue>(Expression<Func<T, TValue>> expression, TValue value);

        /// <summary>
        /// Set value <br />
        /// 设置值
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="value"></param>
        /// <param name="globalVerifyProviderName"></param>
        /// <typeparam name="TValue"></typeparam>
        void SetValue<TValue>(Expression<Func<T, TValue>> expression, TValue value, string globalVerifyProviderName);

        /// <summary>
        /// Get value <br />
        /// 获得值
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        object GetValue(Expression<Func<T, object>> expression);

        /// <summary>
        /// Get value <br />
        /// 获得值
        /// </summary>
        /// <param name="expression"></param>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        TValue GetValue<TValue>(Expression<Func<T, TValue>> expression);

        /// <summary>
        /// Get ObjectMember info by the given member name <br />
        /// 根据给定的成员名，获取元信息
        /// </summary>
        /// <param name="expression"></param>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        ObjectMember GetMember<TValue>(Expression<Func<T, TValue>> expression);
    }
}