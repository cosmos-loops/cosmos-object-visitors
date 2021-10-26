using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CosmosStack.Reflection.ObjectVisitors
{
    /// <summary>
    /// An interface for ValueSetter for Type <br />
    /// 类型值设置器接口
    /// </summary>
    public interface IObjectSetter
    {
        /// <summary>
        /// Gets hosted instance <br />
        /// 获取内部维持的对象实例
        /// </summary>
        object Instance { get; }

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
        /// Contains <br />
        /// 包含
        /// </summary>
        /// <param name="memberName"></param>
        /// <returns></returns>
        bool Contains(string memberName);
    }

    /// <summary>
    /// An interface for ValueSetter for Type <br />
    /// 类型值设置器接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IObjectSetter<T>
    {
        /// <summary>
        /// Gets hosted instance <br />
        /// 获取内部维持的对象实例
        /// </summary>
        T Instance { get; }

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
        void SetValue(Expression<Func<T, object>> expression, object value);

        /// <summary>
        /// Set value <br />
        /// 设置值
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="value"></param>
        /// <param name="globalVerifyProviderName"></param>
        void SetValue(Expression<Func<T, object>> expression, object value, string globalVerifyProviderName);

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
        /// Contains <br />
        /// 包含
        /// </summary>
        /// <param name="memberName"></param>
        /// <returns></returns>
        bool Contains(string memberName);
    }
}