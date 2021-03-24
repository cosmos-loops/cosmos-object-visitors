using System;
using System.Collections.Generic;
using Cosmos.Reflection.Core;
using Cosmos.Reflection.Internals.Visitors;
using Cosmos.Reflection.Metadata;
using Cosmos.Validation.Strategies;

namespace Cosmos.Reflection.Internals
{
    internal static partial class ObjectVisitorFactoryCore
    {
        public static InstanceVisitor CreateForInstance<TStrategy>(Type type, object instance, AlgorithmKind kind, bool repeatable, bool liteMode, TStrategy strategy, bool strictMode)
            where TStrategy : class, IValidationStrategy, new()
        {
            var handler = SafeObjectHandleSwitcher.Switch(kind)(type);
            var visitor = new InstanceVisitor(handler, type, instance, kind, repeatable, liteMode, strictMode);
            visitor.VerifiableEntry.SetStrategy(strategy);
            return visitor;
        }
        
        public static InstanceVisitor<T> CreateForInstance<T, TStrategy>(T instance, AlgorithmKind kind, bool repeatable, bool liteMode, bool strictMode)
            where TStrategy : class, IValidationStrategy<T>, new()
        {
            var handler = UnsafeObjectHandleSwitcher.Switch<T>(kind)().With<T>();
            var visitor = new InstanceVisitor<T>(handler, instance, kind, repeatable, liteMode, strictMode);
            visitor.VerifiableEntry.SetStrategy<TStrategy>();
            return visitor;
        }
        
        public static FutureInstanceVisitor CreateForFutureInstance<TStrategy>(Type type, AlgorithmKind kind, bool repeatable, bool liteMode, TStrategy strategy, bool strictMode, IDictionary<string, object> initialValues = null)
            where TStrategy : class, IValidationStrategy, new()
        {
            var handler = SafeObjectHandleSwitcher.Switch(kind)(type);
            var visitor = new FutureInstanceVisitor(handler, type, kind, repeatable, initialValues, liteMode, strictMode);
            visitor.VerifiableEntry.SetStrategy(strategy);
            return visitor;
        }
        
        public static FutureInstanceVisitor<T> CreateForFutureInstance<T, TStrategy>(AlgorithmKind kind, bool repeatable, bool liteMode, bool strictMode, IDictionary<string, object> initialValues = null)
            where TStrategy : class, IValidationStrategy<T>, new()
        {
            var handler = UnsafeObjectHandleSwitcher.Switch<T>(kind)().With<T>();
            var visitor = new FutureInstanceVisitor<T>(handler, kind, repeatable, initialValues, liteMode, strictMode);
            visitor.VerifiableEntry.SetStrategy<TStrategy>();
            return visitor;
        }
        
        public static StaticTypeObjectVisitor CreateForStaticType<TStrategy>(Type type, AlgorithmKind kind, bool liteMode, TStrategy strategy, bool strictMode)
            where TStrategy : class, IValidationStrategy, new()
        {
            var handler = SafeObjectHandleSwitcher.Switch(kind)(type);
            var visitor = new StaticTypeObjectVisitor(handler, type, kind, liteMode, strictMode);
            visitor.VerifiableEntry.SetStrategy(strategy);
            return visitor;
        }
        
        public static StaticTypeObjectVisitor<T> CreateForStaticType<T, TStrategy>(AlgorithmKind kind, bool liteMode, bool strictMode)
            where TStrategy : class, IValidationStrategy<T>, new()
        {
            var handler = UnsafeObjectHandleSwitcher.Switch<T>(kind)().With<T>();
            var visitor = new StaticTypeObjectVisitor<T>(handler, kind, liteMode, strictMode);
            visitor.VerifiableEntry.SetStrategy<TStrategy>();
            return visitor;
        }
    }
}