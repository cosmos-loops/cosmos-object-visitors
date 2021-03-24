#if NETFRAMEWORK
using System;
using Cosmos.Reflection.Core.TypeHelpers;
using Cosmos.Reflection.Metadata;

namespace Cosmos.Reflection.Core.Builder
{
    public static class CompatibleCallerBuilder
    {
        public static ObjectCallerBase Ctor(Type type)
        {
            var callerType = typeof(CompatibleObjectCaller<>).MakeGenericType(type);

            var accessor = type.CreateTypeAccessor(true);

            if (!ObjectMemberCache.TryTouch(type, out var members))
            {
                members = type.GetObjectMembers();
                
                ObjectMemberCache.Cache(type, members);
            }

            var @params = new object[] {accessor, members};

            return TypeVisit.CreateInstance<ObjectCallerBase>(callerType, @params);
        }
    }

    public static class CompatibleCallerBuilder<T>
    {
        public static Func<ObjectCallerBase> Ctor => () => CompatibleCallerBuilder.Ctor(typeof(T));
    }
}

#endif