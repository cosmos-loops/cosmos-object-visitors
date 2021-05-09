using System;
using Cosmos.Reflection.ObjectVisitors.Internals.Members;
using Cosmos.Reflection.ObjectVisitors.Internals.Repeat;
using Cosmos.Reflection.ObjectVisitors.Metadata;

namespace Cosmos.Reflection.ObjectVisitors.Internals
{
    internal interface ICoreVisitor
    {
        Type SourceType { get; }

        bool IsStatic { get; }

        AlgorithmKind AlgorithmKind { get; }

        object Instance { get; }

        HistoricalContext ExposeHistoricalContext();

        Lazy<MemberHandler> ExposeLazyMemberHandler();

        IObjectVisitor Owner { get; }

        bool LiteMode { get; }
    }

    internal interface ICoreVisitor<T>
    {
        Type SourceType { get; }

        bool IsStatic { get; }

        AlgorithmKind AlgorithmKind { get; }

        T Instance { get; }

        HistoricalContext<T> ExposeHistoricalContext();

        Lazy<MemberHandler> ExposeLazyMemberHandler();

        IObjectVisitor<T> Owner { get; }

        bool LiteMode { get; }
    }
}