using System;
using Cosmos.Reflection.ObjectVisitors.Core;
using Cosmos.Reflection.ObjectVisitors.Core.Builder;
using Cosmos.Reflection.ObjectVisitors.Metadata;

namespace Cosmos.Reflection.ObjectVisitors.Internals
{
    internal static class SafeObjectHandleSwitcher
    {
        public static Func<Type, ObjectCallerBase> Switch(AlgorithmKind kind)
        {
#if NETFRAMEWORK
            return CompatibleCallerBuilder.Ctor;
#else
            switch (kind)
            {
                case AlgorithmKind.Precision:
                    return PrecisionDictOperator.CreateFromType;
                case AlgorithmKind.Hash:
                    return HashDictOperator.CreateFromType;
                case AlgorithmKind.Fuzzy:
                    return FuzzyDictOperator.CreateFromType;
                default:
                    throw new InvalidOperationException("Unknown AlgorithmKind.");
            }
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
            switch (kind)
            {
                case AlgorithmKind.Precision:
                    return () => PrecisionDictOperator<T>.Create();
                case AlgorithmKind.Hash:
                    return () => HashDictOperator<T>.Create();
                case AlgorithmKind.Fuzzy:
                    return () => FuzzyDictOperator<T>.Create();
                default:
                    throw new InvalidOperationException("Unknown AlgorithmKind.");
            }
#endif
        }
    }
}