using System;
using System.Collections.Generic;
using Cosmos.Reflection.Metadata;

namespace Cosmos.Reflection
{
    public static class ObjectInstanceCreator
    {
        public static object Create(Type type, IDictionary<string, object> keyValueCollections, AlgorithmKind kind = AlgorithmKind.Precision)
        {
            return Create(type, keyValueCollections, out _, kind);
        }

        public static object Create(Type type, IDictionary<string, object> keyValueCollections, out IObjectVisitor visitor, AlgorithmKind kind = AlgorithmKind.Precision)
        {
            visitor = ObjectVisitorFactory.Create(type, keyValueCollections, kind);
            return visitor.Instance;
        }

        public static T Create<T>(IDictionary<string, object> keyValueCollections, AlgorithmKind kind = AlgorithmKind.Precision)
        {
            return Create<T>(keyValueCollections, out _, kind);
        }

        public static T Create<T>(IDictionary<string, object> keyValueCollections, out IObjectVisitor<T> visitor, AlgorithmKind kind = AlgorithmKind.Precision)
        {
            visitor = ObjectVisitorFactory.Create<T>(keyValueCollections, kind);
            return visitor.Instance;
        }
    }
}