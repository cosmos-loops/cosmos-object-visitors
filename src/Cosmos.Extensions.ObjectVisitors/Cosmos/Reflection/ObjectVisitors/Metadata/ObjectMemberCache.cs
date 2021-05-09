#if NETFRAMEWORK
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Cosmos.Reflection.ObjectVisitors.Metadata
{
    internal static class ObjectMemberCache
    {
        private static readonly ConcurrentDictionary<Type, Dictionary<string, ObjectMember>> _objectMemberCache;
        private static readonly object _cacheLockObj = new();

        static ObjectMemberCache()
        {
            _objectMemberCache = new ConcurrentDictionary<Type, Dictionary<string, ObjectMember>>();
        }

        public static void Cache(Type type, Dictionary<string, ObjectMember> dictionary)
        {
            // ReSharper disable once InconsistentlySynchronizedField
            if (_objectMemberCache.ContainsKey(type))
                return;

            lock (_cacheLockObj)
            {
                if (_objectMemberCache.ContainsKey(type))
                    return;

                _objectMemberCache[type] = dictionary;
            }
        }

        public static Dictionary<string, ObjectMember> Touch(Type type)
        {
            if (_objectMemberCache.TryGetValue(type, out var result))
                return result;
            throw new NullReferenceException($"There's no dictionary cached for type '{type.FullName}'");
        }

        public static bool TryTouch(Type type, out Dictionary<string, ObjectMember> dictionary)
        {
            return _objectMemberCache.TryGetValue(type, out dictionary);
        }
    }
}

#endif