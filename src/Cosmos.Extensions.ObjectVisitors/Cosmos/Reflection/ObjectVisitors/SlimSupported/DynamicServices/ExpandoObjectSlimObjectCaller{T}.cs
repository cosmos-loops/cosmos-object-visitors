using System.Dynamic;
using System.Reflection;
using Cosmos.Exceptions;
using Cosmos.Reflection.ObjectVisitors.Core;
using Cosmos.Reflection.ObjectVisitors.Metadata;

#if NET452 || NET462
using Cosmos.Collections;
#endif

namespace Cosmos.Reflection.ObjectVisitors.SlimSupported.DynamicServices;

/// <summary>
/// ExpandoObject Slim for Object Caller <br />
/// ExpandoObject 模拟
/// </summary>
public sealed class ExpandoObjectSlimObjectCaller : ObjectCallerBase<ExpandoObject>
{
    /// <summary>
    /// Mode <br />
    /// 模式
    /// </summary>
    public SlimSupportedFor Mode => SlimSupportedFor.ExpandoObject;

    /// <inheritdoc />
    public override void New() => Instance = new ExpandoObject();

    protected override HashSet<string> InternalMemberNames => ((IDictionary<string, object>)Instance).Keys.ToHashSet();

    /// <inheritdoc />
    public override T Get<T>(string name)
    {
        return (T)GetObject(name);
    }

    /// <inheritdoc />
    public override void Set(string name, object value)
    {
        ((IDictionary<string, object>)Instance)[name] = value;
    }

    /// <inheritdoc />
    public override object GetObject(string name)
    {
        var @try = Try.Create(() => ((IDictionary<string, object>)Instance)[name]);
        return @try.GetSafeValue(default(object));
    }

    /// <inheritdoc />
    public override ObjectMember GetMember(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name));

        if (!((IDictionary<string, object>)Instance).Keys.Contains(name))
            throw new ArgumentException($"There's no such member named {name}");

        var target = ((IDictionary<string, object>)Instance)[name];
        var runtimeType = GetRuntimeType(target);
        var isAsync = runtimeType?.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) is not null;
        return new DynamicServiceSlimObjectMember(name, runtimeType, isAsync, Mode);
    }

    private static Type GetRuntimeType(object target)
    {
        if (target is null)
            throw new InvalidOperationException("The Target value is null, and the runtime type cannot be obtained.");

        return U(target);

        Type U<T>(T t) => typeof(T);
    }
}