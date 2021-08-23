#if !NETFRAMEWORK
using System;
using Cosmos.Reflection.ObjectVisitors.Core;
using Cosmos.Reflection.ObjectVisitors.Core.TypeHelpers;

namespace Cosmos.Reflection.ObjectVisitors.SlimSupported.AnonymousServices
{
    internal static class AnonymousServiceTypeHelper
    {
        public static bool IsSupportedAnonymousType<T>()
        {
            return IsSupportedAnonymousType(typeof(T));
        }

        public static bool IsSupportedAnonymousType(Type type)
        {
            if (type is null)
                return false;

            if (TypeHelper.IsAnonymousType(type))
                return true;

            return false;
        }

        public static ObjectCallerBase<T> Create<T>()
        {
            return Create(typeof(T)).With<T>();
        }

        public static ObjectCallerBase Create(Type type)
        {
            if (type is null)
                return default;

            if (type == typeof(NullObjectClass))
                return default;

            if (!TypeHelper.IsAnonymousType(type))
                return default;

            var z = typeof(AnonymousServiceSlimObjectCaller<>).MakeGenericType(type);
            var caller = TypeVisit.CreateInstance<ObjectCallerBase>(z);

            return caller;
        }
    }
}
#endif