namespace Cosmos.Reflection.ObjectVisitors.Core;

/// <summary>
/// Core caller base <br />
/// 核心调用基类
/// </summary>
public abstract class CoreCallerBase
{
    /// <summary>
    /// If it is not a static class, you can create a new instance <br />
    /// 如果不是静态类，可以新建一个实例
    /// </summary>
    public virtual void New() { }

    /// <summary>
    /// Reflect strongly typed fields or attribute values by specifying generics and member names <br />
    /// 通过指定泛型以及成员名反射出强类型的字段或者属性值
    /// </summary>
    /// <typeparam name="T">字段/属性的类型</typeparam>
    /// <param name="name">字段/属性名</param>
    /// <returns></returns>
    public abstract unsafe T Get<T>(string name);

    /// <summary>
    /// Set field or attribute values by specifying generics and member names <br />
    /// 通过指定泛型以及成员名设置字段或者属性值
    /// </summary>
    /// <param name="name">字段/属性名</param>
    /// <param name="value">字段/属性新值</param>
    public abstract unsafe void Set(string name, object value);
}