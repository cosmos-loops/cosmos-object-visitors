using System;
using CosmosStack.Reflection.ObjectVisitors.Core;
using CosmosStack.Reflection.ObjectVisitors.Internals.Members;
using CosmosStack.Reflection.ObjectVisitors.Internals.Repeat;
using CosmosStack.Reflection.ObjectVisitors.Metadata;

namespace CosmosStack.Reflection.ObjectVisitors.Internals
{
    internal interface ICoreVisitor
    {
        Type SourceType { get; }

        bool IsStatic { get; }

        AlgorithmKind AlgorithmKind { get; }

        object Instance { get; }
        
        int Signature { get; }

        HistoricalContext ExposeHistoricalContext();

        Lazy<MemberHandler> ExposeLazyMemberHandler();

        ObjectCallerBase ExposeObjectCaller();

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

        ObjectCallerBase<T> ExposeObjectCaller();

        IObjectVisitor<T> Owner { get; }

        bool LiteMode { get; }
    }
}