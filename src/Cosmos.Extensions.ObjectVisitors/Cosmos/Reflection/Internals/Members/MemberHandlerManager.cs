using System;
using System.Collections.Generic;
using Cosmos.Reflection.Metadata;

namespace Cosmos.Reflection.Internals.Members
{
    internal static class MemberHandlerResolver
    {
        private static readonly Dictionary<int, MemberHandler> _memberHandlerCache = new();
        private static readonly object _lockObj = new();

        public static MemberHandler Resolve(Type type, AlgorithmKind kind = AlgorithmKind.Precision)
        {
            if (type is null)
                return default;

            var hash = type.GetHashCode();

            if (_memberHandlerCache.ContainsKey(hash))
                return _memberHandlerCache[hash];

            lock (_lockObj)
            {
                if (_memberHandlerCache.ContainsKey(hash))
                    return _memberHandlerCache[hash];

                var memberHandler = MemberHandler.For(type, kind);

                _memberHandlerCache[hash] = memberHandler;

                return memberHandler;
            }
        }

        public static MemberHandler Resolve<T>(AlgorithmKind kind = AlgorithmKind.Precision)
        {
            var type = typeof(T);
            var hash = type.GetHashCode();

            if (_memberHandlerCache.ContainsKey(hash))
                return _memberHandlerCache[hash];

            lock (_lockObj)
            {
                if (_memberHandlerCache.ContainsKey(hash))
                    return _memberHandlerCache[hash];

                var memberHandler = MemberHandler.For<T>(kind);

                _memberHandlerCache[hash] = memberHandler;

                return memberHandler;
            }
        }
    }
}