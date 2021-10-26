using System;
using CosmosStack.Reflection.ObjectVisitors.Metadata;

namespace CosmosStack.Reflection.ObjectVisitors.Internals.Members
{
    internal static class MemberHandlerResolver
    {
        public static MemberHandler Resolve(Type type, AlgorithmKind kind = AlgorithmKind.Precision) => MemberHandler.For(type, kind);

        public static MemberHandler Resolve<T>(AlgorithmKind kind = AlgorithmKind.Precision) => MemberHandler.For<T>(kind);
    }
}