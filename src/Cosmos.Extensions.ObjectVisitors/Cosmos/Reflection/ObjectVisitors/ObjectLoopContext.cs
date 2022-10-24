using Cosmos.Reflection.ObjectVisitors.Metadata;

namespace Cosmos.Reflection.ObjectVisitors;

/// <summary>
/// Object lopping context <br />
/// 对象循环上下文
/// </summary>
public class ObjectLoopContext
{
    /// <summary>
    /// Object name <br />
    /// 对象名
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Value <br />
    /// 对象值
    /// </summary>
    public object Value { get; }

    /// <summary>
    /// Metadata <br />
    /// 对象的 ObjectMember 信息
    /// </summary>
    public ObjectMember Metadata { get; }

    /// <summary>
    /// Index <br />
    /// 索引
    /// </summary>
    public int Index { get; }

    public ObjectLoopContext(string name, object value, ObjectMember member, int index)
    {
        Name = name;
        Value = value;
        Metadata = member;
        Index = index;
    }
}