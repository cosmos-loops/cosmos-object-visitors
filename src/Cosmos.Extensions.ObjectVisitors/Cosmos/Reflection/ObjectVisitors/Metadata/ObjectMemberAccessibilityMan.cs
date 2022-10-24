using System.Collections.Concurrent;

namespace Cosmos.Reflection.ObjectVisitors.Metadata;

internal static class ObjectMemberAccessibilityMan
{
    internal static readonly ConcurrentDictionary<Type, ObjectMemberAccessibilityLevel> _accessCache;

    static ObjectMemberAccessibilityMan()
    {
        _accessCache = new ConcurrentDictionary<Type, ObjectMemberAccessibilityLevel>();
    }

    public static void AllowPrivateMembersFor(Type type)
    {
        _accessCache[type] = ObjectMemberAccessibilityLevel.AllowNoPublic;
    }
}