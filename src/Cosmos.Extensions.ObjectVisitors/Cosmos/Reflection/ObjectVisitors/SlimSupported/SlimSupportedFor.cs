namespace Cosmos.Reflection.ObjectVisitors.SlimSupported;

/// <summary>
/// Slim support for ...
/// </summary>
public enum SlimSupportedFor
{
#if !NETFRAMEWORK
    /// <summary>
    /// Anonymous object <br />
    /// 匿名对象
    /// </summary>
    AnonymousObject,
#endif
    /// <summary>
    /// Dynamic Object <br />
    /// 动态对象
    /// </summary>
    DynamicObject,
        
    /// <summary>
    /// ExpandoObject <br />
    /// 动态对象
    /// </summary>
    ExpandoObject,
        
    /// <summary>
    /// Tuple <br />
    /// 元组
    /// </summary>
    Tuple
}