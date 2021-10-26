using System.Collections.Generic;

namespace CosmosStack.Reflection.ObjectVisitors
{
    /// <summary>
    /// An interface for ObjectRepeater <br />
    /// 对象重复器接口
    /// </summary>
    public interface IObjectRepeater
    {
        /// <summary>
        /// Play <br />
        /// 执行
        /// </summary>
        /// <param name="originalObj"></param>
        /// <returns></returns>
        object Play(object originalObj);

        /// <summary>
        /// Play <br />
        /// 执行
        /// </summary>
        /// <param name="keyValueCollections"></param>
        /// <returns></returns>
        object Play(IDictionary<string, object> keyValueCollections);

        /// <summary>
        /// Play again <br />
        /// 重新执行
        /// </summary>
        /// <returns></returns>
        object NewAndPlay();
    }

    /// <summary>
    /// An interface for ObjectRepeater <br />
    /// 对象重复器接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IObjectRepeater<T> : IObjectRepeater
    {
        /// <summary>
        /// Play <br />
        /// 执行
        /// </summary>
        /// <param name="originalObj"></param>
        /// <returns></returns>
        T Play(T originalObj);

        /// <summary>
        /// Play <br />
        /// 执行
        /// </summary>
        /// <param name="keyValueCollections"></param>
        /// <returns></returns>
        new T Play(IDictionary<string, object> keyValueCollections);

        /// <summary>
        /// Play again <br />
        /// 重新执行
        /// </summary>
        /// <returns></returns>
        new T NewAndPlay();
    }
}