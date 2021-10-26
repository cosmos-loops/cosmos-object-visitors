namespace CosmosStack.Reflection.ObjectVisitors
{
    /// <summary>
    /// An interface for ValueSetter for Object <br />
    /// 对象值设置器接口
    /// </summary>
    public interface IObjectValueSetter
    {
        /// <summary>
        /// Set value <br />
        /// 设置值
        /// </summary>
        /// <param name="value"></param>
        void Value(object value);

        /// <summary>
        /// Gets hosted instance <br />
        /// 获取内部维持的对象实例
        /// </summary>
        object HostedInstance { get; }
    }

    /// <summary>
    /// An interface for ValueSetter for Object <br />
    /// 对象值设置器接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IObjectValueSetter<out T>
    {
        /// <summary>
        /// Set value <br />
        /// 设置值
        /// </summary>
        /// <param name="value"></param>
        void Value(object value);

        /// <summary>
        /// Gets hosted instance <br />
        /// 获取内部维持的对象实例
        /// </summary>
        T HostedInstance { get; }
    }

    /// <summary>
    /// An interface for ValueSetter for Object <br />
    /// 对象值设置器接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TVal"></typeparam>
    public interface IObjectValueSetter<out T, in TVal>
    {
        /// <summary>
        /// Set value <br />
        /// 设置值
        /// </summary>
        /// <param name="value"></param>
        void Value(TVal value);

        /// <summary>
        /// Gets hosted instance <br />
        /// 获取内部维持的对象实例
        /// </summary>
        T HostedInstance { get; }
    }
}