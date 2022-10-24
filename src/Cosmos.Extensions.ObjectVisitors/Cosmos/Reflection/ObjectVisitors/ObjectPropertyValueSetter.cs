using System.Reflection;

namespace Cosmos.Reflection.ObjectVisitors;

/// <summary>
/// Object property value setter <br />
/// 值设置器
/// </summary>
public class ObjectPropertyValueSetter : IPropertyValueSetter
{
    internal ObjectPropertyValueSetter() { }
        
    /// <summary>
    /// Invoke <br />
    /// 调用
    /// </summary>
    /// <param name="type"></param>
    /// <param name="that"></param>
    /// <param name="propertyName"></param>
    /// <param name="value"></param>
    public void Invoke(Type type, object that, string propertyName, object value)
    {
        ObjectSetter.Type(type).Instance(that).SetValue(propertyName, value);
    }

    /// <summary>
    /// Invoke <br />
    /// 调用
    /// </summary>
    /// <param name="type"></param>
    /// <param name="that"></param>
    /// <param name="propertyName"></param>
    /// <param name="bindingAttr"></param>
    /// <param name="value"></param>
    public void Invoke(Type type, object that, string propertyName, BindingFlags bindingAttr, object value)
    {
        ObjectSetter.Type(type).Instance(that).SetValue(propertyName, value);
    }

    /// <summary>
    /// Get an instance of <see cref="ObjectPropertyValueSetter"/> <br />
    /// 获取一个 ObjectPropertyValueSetter 实例
    /// </summary>
    public static IPropertyValueSetter Instance { get; } = new ObjectPropertyValueSetter();
}