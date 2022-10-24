using System.Reflection;

namespace Cosmos.Reflection.ObjectVisitors;

/// <summary>
/// Object property value accessor <br />
/// 对象值访问器
/// </summary>
public class ObjectPropertyValueAccessor : IPropertyValueAccessor
{
    internal ObjectPropertyValueAccessor(object instance, Type type)
    {
        _instance = instance;
        _type = type;
    }

    private readonly Type _type;
    private readonly object _instance;

    /// <summary>
    /// Get value <br />
    /// 获取值
    /// </summary>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    public object GetValue(string propertyName)
    {
        return ObjectGetter.Type(_type).Instance(_instance).GetValue(propertyName);
    }

    /// <summary>
    /// Get value <br />
    /// 获取值
    /// </summary>
    /// <param name="propertyName"></param>
    /// <param name="bindingAttr"></param>
    /// <returns></returns>
    public object GetValue(string propertyName, BindingFlags bindingAttr)
    {
        return ObjectGetter.Type(_type).Instance(_instance).GetValue(propertyName);
    }

    /// <summary>
    /// Set value <br />
    /// 设置值
    /// </summary>
    /// <param name="propertyName"></param>
    /// <param name="value"></param>
    public void SetValue(string propertyName, object value)
    {
        ObjectSetter.Type(_type).Instance(_instance).SetValue(propertyName, value);
    }

    /// <summary>
    /// Set value <br />
    /// 设置值
    /// </summary>
    /// <param name="propertyName"></param>
    /// <param name="bindingAttr"></param>
    /// <param name="value"></param>
    public void SetValue(string propertyName, BindingFlags bindingAttr, object value)
    {
        ObjectSetter.Type(_type).Instance(_instance).SetValue(propertyName, value);
    }

    /// <summary>
    /// Of <br />
    /// 根据给定的实例，创建一个值访问器
    /// </summary>
    /// <param name="instance"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IPropertyValueAccessor Of<T>(T instance) => new ObjectPropertyValueAccessor<T>(instance);

    /// <summary>
    /// Of <br />
    /// 根据给定的实例，创建一个值访问器
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static IPropertyValueAccessor Of(object instance, Type type) => new ObjectPropertyValueAccessor(instance, type);

    /// <summary>
    /// Of <br />
    /// 根据给定的静态类型，创建一个静态值访问器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IPropertyValueAccessor OfStatic<T>() => new StaticPropertyValueAccessor<T>();

    /// <summary>
    /// Of <br />
    /// 根据给定的静态类型，创建一个静态值访问器
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static IPropertyValueAccessor OfStatic(Type type) => new StaticPropertyValueAccessor(type);
}

/// <summary>
/// Object property value accessor <br />
/// 对象值访问器
/// </summary>
/// <typeparam name="T"></typeparam>
public class ObjectPropertyValueAccessor<T> : IPropertyValueAccessor
{
    internal ObjectPropertyValueAccessor(object instance)
    {
        _instance = instance;
    }

    private readonly Type _type = typeof(T);
    private readonly object _instance;

    /// <summary>
    /// Get value <br />
    /// 获取值
    /// </summary>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    public object GetValue(string propertyName)
    {
        return ObjectGetter.Type(_type).Instance(_instance).GetValue(propertyName);
    }

    /// <summary>
    /// Get value <br />
    /// 获取值
    /// </summary>
    /// <param name="propertyName"></param>
    /// <param name="bindingAttr"></param>
    /// <returns></returns>
    public object GetValue(string propertyName, BindingFlags bindingAttr)
    {
        return ObjectGetter.Type(_type).Instance(_instance).GetValue(propertyName);
    }

    /// <summary>
    /// Set value <br />
    /// 设置值
    /// </summary>
    /// <param name="propertyName"></param>
    /// <param name="value"></param>
    public void SetValue(string propertyName, object value)
    {
        ObjectSetter.Type(_type).Instance(_instance).SetValue(propertyName, value);
    }

    /// <summary>
    /// Set value <br />
    /// 设置值
    /// </summary>
    /// <param name="propertyName"></param>
    /// <param name="bindingAttr"></param>
    /// <param name="value"></param>
    public void SetValue(string propertyName, BindingFlags bindingAttr, object value)
    {
        ObjectSetter.Type(_type).Instance(_instance).SetValue(propertyName, value);
    }
}