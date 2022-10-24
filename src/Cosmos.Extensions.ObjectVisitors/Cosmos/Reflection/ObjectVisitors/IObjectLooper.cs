namespace Cosmos.Reflection.ObjectVisitors;

/// <summary>
/// An interface for ObjectLopper <br />
/// 对象循环器接口
/// </summary>
public interface IObjectLooper
{
    /// <summary>
    /// Back to ObjectVisitor <br />
    /// 返回对象访问器
    /// </summary>
    /// <returns></returns>
    IObjectVisitor BackToVisitor();

    /// <summary>
    /// Run (fire) <br />
    /// 执行
    /// </summary>
    void Fire();

    /// <summary>
    /// Run (fire) and back <br />
    /// 执行并返回
    /// </summary>
    /// <returns></returns>
    IObjectVisitor FireAndBack();
}

/// <summary>
/// An interface for ObjectLopper <br />
/// 对象循环器接口
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IObjectLooper<T>
{
    /// <summary>
    /// Back to ObjectVisitor <br />
    /// 返回对象访问器
    /// </summary>
    /// <returns></returns>
    IObjectVisitor<T> BackToVisitor();

    /// <summary>
    /// Run (fire) <br />
    /// 执行
    /// </summary>
    void Fire();

    /// <summary>
    /// Run (fire) and back <br />
    /// 执行并返回
    /// </summary>
    /// <returns></returns>
    IObjectVisitor<T> FireAndBack();
}