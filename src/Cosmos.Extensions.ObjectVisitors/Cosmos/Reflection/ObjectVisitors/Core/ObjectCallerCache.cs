using System;
using System.Collections.Concurrent;

namespace Cosmos.Reflection.ObjectVisitors.Core
{
    internal static class ObjectCallerCache
    {
        private static readonly ConcurrentDictionary<Type, ObjectCallerBase> Cached;

        static ObjectCallerCache()
        {
            Cached = new();
        }

        public static ObjectCallerBase Get(Type type, Func<ObjectCallerBase> factory)
        {
            return Cached.GetOrAdd(type, t => factory());
        }

        public static ObjectCallerBase<T> Get<T>(Func<ObjectCallerBase> factory)
        {
            return Cached.GetOrAdd(typeof(T), t => factory()).With<T>();
        }
    }
}