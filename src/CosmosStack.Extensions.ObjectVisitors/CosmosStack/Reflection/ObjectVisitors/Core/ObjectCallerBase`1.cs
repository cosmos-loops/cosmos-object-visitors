#if NET5_0_OR_GREATER
using System.Runtime.CompilerServices;
#endif

namespace CosmosStack.Reflection.ObjectVisitors.Core
{
    /// <summary>
    /// Object caller base <br />
    /// 对象调用基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
#if NET5_0_OR_GREATER
    [SkipLocalsInit]
#endif
    public abstract class ObjectCallerBase<T> : ObjectCallerBase
    {
        /// <summary>
        /// Gets instance
        /// </summary>
        public T Instance;

        /// <summary>
        /// Sets instance
        /// </summary>
        /// <param name="value"></param>
        public void SetInstance(T value) => Instance = value;

        /// <summary>
        /// Set instance via object
        /// </summary>
        /// <param name="obj"></param>
        public override void SetObjInstance(object obj)
        {
            Instance = (T)obj;
        }

        /// <summary>
        /// Gets instance
        /// </summary>
        /// <returns></returns>
        public T GetInstance() => Instance;

        /// <summary>
        /// Get instance by object
        /// </summary>
        /// <returns></returns>
        public override object GetObjInstance() => Instance;
    }
}