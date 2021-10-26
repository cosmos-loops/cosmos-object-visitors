using System;
using System.Linq.Expressions;

namespace CosmosStack.Reflection.ObjectVisitors
{
    /// <summary>
    /// An interface for ObjectGetter <br />
    /// 对象获取器接口
    /// </summary>
    public interface IObjectGetter
    {
        /// <summary>
        /// Gets hosted instance <br />
        /// 获取内部维持的对象实例
        /// </summary>
        object Instance { get; }

        /// <summary>
        /// Get value <br />
        /// 获取值
        /// </summary>
        /// <param name="memberName"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        object GetValue(string memberName, AccessMode mode = AccessMode.Concise);

        /// <summary>
        /// Get value <br />
        /// 获取值
        /// </summary>
        /// <param name="memberName"></param>
        /// <param name="mode"></param>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        TValue GetValue<TValue>(string memberName, AccessMode mode = AccessMode.Concise);

        /// <summary>
        /// Get value <br />
        /// 获取值
        /// </summary>
        /// <param name="expression"></param>
        /// <typeparam name="TObj"></typeparam>
        /// <returns></returns>
        object GetValue<TObj>(Expression<Func<TObj, object>> expression);

        /// <summary>
        /// Get value <br />
        /// 获取值
        /// </summary>
        /// <param name="expression"></param>
        /// <typeparam name="TObj"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        TValue GetValue<TObj, TValue>(Expression<Func<TObj, TValue>> expression);

        /// <summary>
        /// Contains <br />
        /// 包含
        /// </summary>
        /// <param name="memberName"></param>
        /// <returns></returns>
        bool Contains(string memberName);
    }

    /// <summary>
    /// An interface for ObjectGetter <br />
    /// 对象获取器接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IObjectGetter<T>
    {
        /// <summary>
        /// Gets hosted instance <br />
        /// 获取内部维持的对象实例
        /// </summary>
        T Instance { get; }

        /// <summary>
        /// Get value <br />
        /// 获取值
        /// </summary>
        /// <param name="memberName"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        object GetValue(string memberName, AccessMode mode = AccessMode.Concise);

        /// <summary>
        /// Get value <br />
        /// 获取值
        /// </summary>
        /// <param name="memberName"></param>
        /// <param name="mode"></param>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        TValue GetValue<TValue>(string memberName, AccessMode mode = AccessMode.Concise);

        /// <summary>
        /// Get value <br />
        /// 获取值
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        object GetValue(Expression<Func<T, object>> expression);

        /// <summary>
        /// Get value <br />
        /// 获取值
        /// </summary>
        /// <param name="expression"></param>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        TValue GetValue<TValue>(Expression<Func<T, TValue>> expression);

        /// <summary>
        /// Get value <br />
        /// 获取值
        /// </summary>
        /// <param name="expression"></param>
        /// <typeparam name="TObj"></typeparam>
        /// <returns></returns>
        object GetValue<TObj>(Expression<Func<TObj, object>> expression);

        /// <summary>
        /// Get value <br />
        /// 获取值
        /// </summary>
        /// <param name="expression"></param>
        /// <typeparam name="TObj"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        TValue GetValue<TObj, TValue>(Expression<Func<TObj, TValue>> expression);

        /// <summary>
        /// Contains <br />
        /// 包含
        /// </summary>
        /// <param name="memberName"></param>
        /// <returns></returns>
        bool Contains(string memberName);
    }
}