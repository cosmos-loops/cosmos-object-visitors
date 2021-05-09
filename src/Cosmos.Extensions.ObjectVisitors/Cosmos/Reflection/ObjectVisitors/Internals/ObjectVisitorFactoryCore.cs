using System;
using System.Collections.Generic;
using Cosmos.Reflection.ObjectVisitors.Core;
using Cosmos.Reflection.ObjectVisitors.Internals.Visitors;
using Cosmos.Reflection.ObjectVisitors.Metadata;

namespace Cosmos.Reflection.ObjectVisitors.Internals
{
    internal static partial class ObjectVisitorFactoryCore
    {
        public static InstanceVisitor CreateForInstance(Type type, object instance, AlgorithmKind kind, bool repeatable, bool liteMode, bool strictMode)
        {
            var handler = ObjectCallerCache.Get(type, () => SafeObjectHandleSwitcher.Switch(kind)(type));
            return new InstanceVisitor(handler, type, instance, kind, repeatable, liteMode, strictMode);
        }

        public static InstanceVisitor<T> CreateForInstance<T>(T instance, AlgorithmKind kind, bool repeatable, bool liteMode, bool strictMode)
        {
            var handler = ObjectCallerCache.Get<T>(() => UnsafeObjectHandleSwitcher.Switch<T>(kind)());
            return new InstanceVisitor<T>(handler, instance, kind, repeatable, liteMode, strictMode);
        }

        public static FutureInstanceVisitor CreateForFutureInstance(Type type, AlgorithmKind kind, bool repeatable, bool liteMode, bool strictMode, IDictionary<string, object> initialValues = null)
        {
            var handler = ObjectCallerCache.Get(type, () => SafeObjectHandleSwitcher.Switch(kind)(type));
            return new FutureInstanceVisitor(handler, type, kind, repeatable, initialValues, liteMode, strictMode);
        }

        public static FutureInstanceVisitor<T> CreateForFutureInstance<T>(AlgorithmKind kind, bool repeatable, bool liteMode, bool strictMode, IDictionary<string, object> initialValues = null)
        {
            var handler = ObjectCallerCache.Get<T>(() => UnsafeObjectHandleSwitcher.Switch<T>(kind)());
            return new FutureInstanceVisitor<T>(handler, kind, repeatable, initialValues, liteMode, strictMode);
        }

        public static StaticTypeObjectVisitor CreateForStaticType(Type type, AlgorithmKind kind, bool liteMode, bool strictMode)
        {
            var handler = ObjectCallerCache.Get(type, () => SafeObjectHandleSwitcher.Switch(kind)(type));
            return new StaticTypeObjectVisitor(handler, type, kind, liteMode, strictMode);
        }

        public static StaticTypeObjectVisitor<T> CreateForStaticType<T>(AlgorithmKind kind, bool liteMode, bool strictMode)
        {
            var handler = ObjectCallerCache.Get<T>(() => UnsafeObjectHandleSwitcher.Switch<T>(kind)());
            return new StaticTypeObjectVisitor<T>(handler, kind, liteMode, strictMode);
        }
    }
}