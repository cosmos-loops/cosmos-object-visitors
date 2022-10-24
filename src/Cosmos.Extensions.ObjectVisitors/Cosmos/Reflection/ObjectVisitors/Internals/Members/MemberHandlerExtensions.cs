using Cosmos.Reflection.ObjectVisitors.Metadata;
using Cosmos.Validation.Objects;

namespace Cosmos.Reflection.ObjectVisitors.Internals.Members;

internal static class MemberHandlerExtensions
{
    public static VerifiableMemberContract ConvertToVerifiableMemberContract(this ObjectMember member, Type declaringType)
    {
        if (member is null)
            throw new ArgumentNullException(nameof(member));
        if (declaringType is null)
            throw new ArgumentNullException(nameof(declaringType));
        var impl = new MemberValueContractImpl(member, declaringType);
        return new VerifiableMemberContract(impl);
    }

    public static VerifiableObjectContract ConvertToVerifiableObjectContract(this MemberHandler handler)
    {
        if (handler is null)
            throw new ArgumentNullException(nameof(handler));
        var impl = new MemberObjectContractImpl(handler);
        return new VerifiableObjectContract(impl);
    }
}