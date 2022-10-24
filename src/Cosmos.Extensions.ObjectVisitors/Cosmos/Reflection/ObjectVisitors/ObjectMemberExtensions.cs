using Cosmos.Reflection.ObjectVisitors.Metadata;

namespace Cosmos.Reflection.ObjectVisitors;

public static class ObjectMemberExtensions
{
    public static void AllowToVisitPrivateMembers(this Type type)
    {
        ObjectMemberAccessibilityMan.AllowPrivateMembersFor(type);
    }
}