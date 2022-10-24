using System.Reflection;

namespace Cosmos.Reflection.ObjectVisitors;

/// <summary>
/// Object property value getter <br />
/// 值读取器
/// </summary>
public class ObjectPropertyValueGetter : IPropertyValueGetter
{
    internal ObjectPropertyValueGetter() { }

    /// <summary>
    /// Invoke <br />
    /// 调用
    /// </summary>
    /// <param name="type"></param>
    /// <param name="that"></param>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    public object Invoke(Type type, object that, string propertyName)
    {
        return ObjectGetter.Type(type).Instance(that).GetValue(propertyName);
    }

    /// <summary>
    /// Invoke <br />
    /// 调用
    /// </summary>
    /// <param name="type"></param>
    /// <param name="that"></param>
    /// <param name="propertyName"></param>
    /// <param name="bindingAttr"></param>
    /// <returns></returns>
    public object Invoke(Type type, object that, string propertyName, BindingFlags bindingAttr)
    {
        return ObjectGetter.Type(type).Instance(that).GetValue(propertyName);
    }

    /// <summary>
    /// Get an instance of <see cref="ObjectPropertyValueGetter"/> <br />
    /// 获取一个 ObjectPropertyValueGetter 实例
    /// </summary>
    public static IPropertyValueGetter Instance { get; } = new ObjectPropertyValueGetter();
}