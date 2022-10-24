using Cosmos.Reflection.ObjectVisitors.Metadata;
using Cosmos.Validation.Objects;

namespace Cosmos.Reflection.ObjectVisitors.SlimSupported.DynamicServices;

/// <summary>
/// Dynamic Service slim for Object Member <br />
/// 动态服务模拟
/// </summary>
public sealed class DynamicServiceSlimObjectMember : ObjectMember
{
    public DynamicServiceSlimObjectMember(
        string name,
        Type type,
        bool isAsync,
        SlimSupportedFor dynamicType
    ) : base(true, true, false, name, type, false, isAsync, false, false, false, false, false, VerifiableMemberKind.Field)
    {
        SlimFor = dynamicType;
    }

    /// <summary>
    /// Slim for ...
    /// </summary>
    public SlimSupportedFor SlimFor;
}