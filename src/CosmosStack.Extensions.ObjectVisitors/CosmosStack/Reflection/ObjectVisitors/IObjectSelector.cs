using System.Collections.Generic;

namespace CosmosStack.Reflection.ObjectVisitors
{
    /// <summary>
    /// An interface for Object selector <br />
    /// 对象选择器接口
    /// </summary>
    /// <typeparam name="TVal"></typeparam>
    public interface IObjectSelector<TVal>
    {
        /// <summary>
        /// Back to ObjectVisitor <br />
        /// 返回对象访问器
        /// </summary>
        /// <returns></returns>
        IObjectVisitor BackToVisitor();

        /// <summary>
        /// Run (fire) and return <br />
        /// 执行并返回值
        /// </summary>
        /// <returns></returns>
        IEnumerable<TVal> FireAndReturn();

        /// <summary>
        /// Run (fire) and back <br />
        /// 执行并返回
        /// </summary>
        /// <param name="returnedVal"></param>
        /// <returns></returns>
        IObjectVisitor FireAndBack(out IEnumerable<TVal> returnedVal);
    }

    /// <summary>
    /// An interface for Object selector <br />
    /// 对象选择器接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TVal"></typeparam>
    public interface IObjectSelector<T, TVal>
    {
        /// <summary>
        /// Back to ObjectVisitor <br />
        /// 返回对象访问器
        /// </summary>
        /// <returns></returns>
        IObjectVisitor<T> BackToVisitor();

        /// <summary>
        /// Run (fire) and return <br />
        /// 执行并返回值
        /// </summary>
        /// <returns></returns>
        IEnumerable<TVal> FireAndReturn();

        /// <summary>
        /// Run (fire) and back <br />
        /// 执行并返回
        /// </summary>
        /// <param name="returnedVal"></param>
        /// <returns></returns>
        IObjectVisitor<T> FireAndBack(out IEnumerable<TVal> returnedVal);
    }
}