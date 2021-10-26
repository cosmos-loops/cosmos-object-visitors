namespace CosmosStack.Reflection.ObjectVisitors
{
    /// <summary>
    /// An interface for Object ValueGetter <br />
    /// 值获取器接口
    /// </summary>
    public interface IObjectValueGetter
    {
        /// <summary>
        /// Gets value <br />
        /// 获取值
        /// </summary>
        object Value { get; }

        /// <summary>
        /// Gets hosted instance <br />
        /// 获取内部维持的对象实例
        /// </summary>
        object HostedInstance { get; }
    }

    /// <summary>
    /// An interface for Object ValueGetter <br />
    /// 值获取器接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IObjectValueGetter<out T>
    {
        /// <summary>
        /// Gets value <br />
        /// 获取值
        /// </summary>
        object Value { get; }

        /// <summary>
        /// Gets hosted instance <br />
        /// 获取内部维持的对象实例
        /// </summary>
        T HostedInstance { get; }
    }

    /// <summary>
    /// An interface for Object ValueGetter <br />
    /// 值获取器接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TVal"></typeparam>
    public interface IObjectValueGetter<out T, out TVal>
    {
        /// <summary>
        /// Gets value <br />
        /// 获取值
        /// </summary>
        TVal Value { get; }

        /// <summary>
        /// Gets hosted instance <br />
        /// 获取内部维持的对象实例
        /// </summary>
        T HostedInstance { get; }
    }
}