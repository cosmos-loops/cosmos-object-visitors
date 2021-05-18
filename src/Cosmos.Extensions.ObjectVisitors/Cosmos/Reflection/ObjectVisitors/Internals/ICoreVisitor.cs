using System;
using Cosmos.Reflection.ObjectVisitors.Core;
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
        
        int Signature { get; }
        
        //void SyncInstance(object instance);

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