using System;
using CosmosStack.Reflection.ObjectVisitors.Core;
using CosmosStack.Reflection.ObjectVisitors.Core.Builder;
using CosmosStack.Reflection.ObjectVisitors.Metadata;
using CosmosStack.Reflection.ObjectVisitors.SlimSupported;

namespace CosmosStack.Reflection.ObjectVisitors.Internals
{
    internal static class SafeObjectHandleSwitcher
    {
        public static Func<Type, ObjectCallerBase> Switch(AlgorithmKind kind)
        {
#if NETFRAMEWORK
            return CompatibleCallerBuilder.Ctor;
#else
            return kind switch
            {
                AlgorithmKind.Precision => PrecisionDictOperator.CreateFromType,
                AlgorithmKind.Hash => HashDictOperator.CreateFromType,
                AlgorithmKind.Fuzzy => FuzzyDictOperator.CreateFromType,
                _ => throw new InvalidOperationException("Unknown AlgorithmKind.")
            };
#endif
        }
    }

    internal static unsafe class UnsafeObjectHandleSwitcher
    {
        public static Func<ObjectCallerBase> Switch<T>(AlgorithmKind kind)
        {
#if NETFRAMEWORK
            return CompatibleCallerBuilder<T>.Ctor;
#else
            if (SlimDeterminer.Check<T>(out var createFromSlimService))
                return createFromSlimService;

            return kind switch
            {
                AlgorithmKind.Precision => () => PrecisionDictOperator<T>.Create(),
                AlgorithmKind.Hash => () => HashDictOperator<T>.Create(),
                AlgorithmKind.Fuzzy => () => FuzzyDictOperator<T>.Create(),
                _ => throw new InvalidOperationException("Unknown AlgorithmKind.")
            };
#endif
        }
    }
}