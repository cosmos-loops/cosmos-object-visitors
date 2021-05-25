using System;
using System.Collections.Generic;
using Cosmos.Reflection.ObjectVisitors.Core;
using Cosmos.Reflection.ObjectVisitors.Internals.Visitors;

namespace Cosmos.Reflection.ObjectVisitors.Internals
{
    internal static partial class ObjectVisitorFactoryCore
    {
        public static InstanceVisitor CreateForInstance(Type type, object instance, ObjectVisitorOptions options)
        {
            options ??= ObjectVisitorOptions.Default;
            var handler = SafeObjectHandleSwitcher.Switch(options.AlgorithmKind)(type).AndSetInstance(instance);
            return new InstanceVisitor(handler, type, options);
        }

        public static InstanceVisitor<T> CreateForInstance<T>(T instance, ObjectVisitorOptions options)
        {
            options ??= ObjectVisitorOptions.Default;
            var handler = UnsafeObjectHandleSwitcher.Switch<T>(options.AlgorithmKind)().AndSetInstance(instance).With<T>();
            return new InstanceVisitor<T>(handler, options);
        }

        public static FutureInstanceVisitor CreateForFutureInstance(Type type, ObjectVisitorOptions options, IDictionary<string, object> initialValues = null)
        {
            options ??= ObjectVisitorOptions.Default;
            var handler = SafeObjectHandleSwitcher.Switch(options.AlgorithmKind)(type);
            return new FutureInstanceVisitor(handler, type, options, initialValues);
        }

        public static FutureInstanceVisitor<T> CreateForFutureInstance<T>(ObjectVisitorOptions options, IDictionary<string, object> initialValues = null)
        {
            options ??= ObjectVisitorOptions.Default;
            var handler = UnsafeObjectHandleSwitcher.Switch<T>(options.AlgorithmKind)().With<T>();
            return new FutureInstanceVisitor<T>(handler, options, initialValues);
        }

        public static StaticTypeObjectVisitor CreateForStaticType(Type type, ObjectVisitorOptions options)
        {
            options ??= ObjectVisitorOptions.Default;
            var handler = SafeObjectHandleSwitcher.Switch(options.AlgorithmKind)(type);
            return new StaticTypeObjectVisitor(handler, type, options);
        }

        public static StaticTypeObjectVisitor<T> CreateForStaticType<T>(ObjectVisitorOptions options)
        {
            options ??= ObjectVisitorOptions.Default;
            var handler = UnsafeObjectHandleSwitcher.Switch<T>(options.AlgorithmKind)().With<T>();
            return new StaticTypeObjectVisitor<T>(handler, options);
        }
    }
}