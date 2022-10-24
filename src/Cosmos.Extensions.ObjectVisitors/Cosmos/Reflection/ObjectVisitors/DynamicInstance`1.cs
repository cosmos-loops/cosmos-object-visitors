using System.Dynamic;
using Cosmos.Exceptions;

namespace Cosmos.Reflection.ObjectVisitors;

/// <summary>
/// Dynamic Instance <br />
/// 动态实例
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class DynamicInstance<T> : DynamicObject
{
    private readonly IObjectVisitor<T> _visitor;
    private readonly FutureCreatingBuilder<string, object> _gettingMemberHandler;
    private readonly FutureInvokingBuilder<string, object> _settingMemberHandler;
    private readonly Lazy<IObjectGetter<T>> _lazyObjectGetter;
    private readonly Lazy<IObjectSetter<T>> _lazyObjectSetter;

    internal DynamicInstance(IObjectVisitor<T> visitor)
    {
        _visitor = visitor ?? throw new ArgumentNullException(nameof(visitor));
        _gettingMemberHandler = Try.CreateFuture<string, object>(name => _visitor.GetValue(name));
        _settingMemberHandler = Try.Future<string, object>((name, val) => _visitor.SetValue(name, val));
        _lazyObjectGetter = new Lazy<IObjectGetter<T>>(() => ObjectGetter.Type<T>(_visitor.AlgorithmKind).Instance(_visitor.Instance));
        _lazyObjectSetter = new Lazy<IObjectSetter<T>>(() => ObjectSetter.Type<T>(_visitor.AlgorithmKind).Instance(_visitor.Instance));
    }

    /// <inheritdoc />
    public override bool TryGetMember(GetMemberBinder binder, out object result)
    {
        var @try = _gettingMemberHandler.Invoke(binder.Name);
        result = @try.GetSafeValue((object)default);
        return @try.IsSuccess;
    }

    /// <inheritdoc />
    public override bool TrySetMember(SetMemberBinder binder, object value)
    {
        var @try = _settingMemberHandler.Invoke(binder.Name, value);
        return @try.IsSuccess;
    }

    /// <inheritdoc />
    public override IEnumerable<string> GetDynamicMemberNames() => _visitor.GetMemberNames();

    /// <summary>
    /// Expose Object Visitor <br />
    /// 导出对象访问器
    /// </summary>
    /// <returns></returns>
    public IObjectVisitor<T> ExposeVisitor() => _visitor;

    /// <summary>
    /// Expose Value Accessor <br />
    /// 导出值访问器
    /// </summary>
    /// <returns></returns>
    public IPropertyValueAccessor ExposeValueAccessor() => _visitor.ToValueAccessor();

    /// <summary>
    /// Expose Object Getter <br />
    /// 导出对象获取器
    /// </summary>
    /// <returns></returns>
    public IObjectGetter<T> ExposeGetter() => _lazyObjectGetter.Value;

    /// <summary>
    /// Expose Object Setter <br />
    /// 导出对象设置器
    /// </summary>
    /// <returns></returns>
    public IObjectSetter<T> ExposeSetter() => _lazyObjectSetter.Value;
}