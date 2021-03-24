using System;
using System.Collections.Generic;
using Cosmos.Reflection.Correctness;
using Cosmos.Reflection.Internals;
using Cosmos.Reflection.Metadata;
using Cosmos.Validation;
using Cosmos.Validation.Strategies;

namespace Cosmos.Reflection
{
    public static class ObjectVisitorFactory
    {
        static ObjectVisitorFactory()
        {
            CorrectnessVerifiableWallInitializer.InitializeAndPreheating();
            CorrectnessVerifiableWall.InitValidationProvider(ValidationMe.Use(CorrectnessVerifiableWall.GLOBAL_CORRECT_PROVIDER_KEY));
        }

        public static IObjectVisitor Create(Type type, object instance, AlgorithmKind kind = AlgorithmKind.Precision, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
        {
            return ObjectVisitorFactoryCore.CreateForInstance(type, instance, kind, repeatable, LvMode.FULL, strictMode);
        }

        public static IObjectVisitor Create(Type type, AlgorithmKind kind = AlgorithmKind.Precision, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
        {
            if (type.IsAbstract && type.IsSealed)
                return ObjectVisitorFactoryCore.CreateForStaticType(type, kind, LvMode.FULL, strictMode);
            return ObjectVisitorFactoryCore.CreateForFutureInstance(type, kind, repeatable, LvMode.FULL, strictMode);
        }

        public static IObjectVisitor Create(Type type, IDictionary<string, object> initialValues, AlgorithmKind kind = AlgorithmKind.Precision, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
        {
            if (type.IsAbstract && type.IsSealed)
                return ObjectVisitorFactoryCore.CreateForStaticType(type, kind, LvMode.FULL, strictMode);
            return ObjectVisitorFactoryCore.CreateForFutureInstance(type, kind, repeatable, LvMode.FULL, strictMode, initialValues);
        }

        public static IObjectVisitor<T> Create<T>(T instance, AlgorithmKind kind = AlgorithmKind.Precision, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
        {
            return ObjectVisitorFactoryCore.CreateForInstance<T>(instance, kind, repeatable, LvMode.FULL, strictMode);
        }

        public static IObjectVisitor<T> Create<T>(AlgorithmKind kind = AlgorithmKind.Precision, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
        {
            var type = typeof(T);
            if (type.IsAbstract && type.IsSealed)
                return ObjectVisitorFactoryCore.CreateForStaticType<T>(kind, LvMode.FULL, strictMode);
            return ObjectVisitorFactoryCore.CreateForFutureInstance<T>(kind, repeatable, LvMode.FULL, strictMode);
        }

        public static IObjectVisitor<T> Create<T>(IDictionary<string, object> initialValues, AlgorithmKind kind = AlgorithmKind.Precision, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
        {
            var type = typeof(T);
            if (type.IsAbstract && type.IsSealed)
                return ObjectVisitorFactoryCore.CreateForStaticType<T>(kind, LvMode.FULL, strictMode);
            return ObjectVisitorFactoryCore.CreateForFutureInstance<T>(kind, repeatable, LvMode.FULL, strictMode, initialValues);
        }

        public static IObjectVisitor Create<TStrategy>(Type type, object instance, TStrategy validationStrategy, AlgorithmKind kind = AlgorithmKind.Precision, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
            where TStrategy : class, IValidationStrategy, new()
        {
            return ObjectVisitorFactoryCore.CreateForInstance(type, instance, kind, repeatable, LvMode.FULL, validationStrategy, strictMode);
        }

        public static IObjectVisitor Create<TStrategy>(Type type, TStrategy validationStrategy, AlgorithmKind kind = AlgorithmKind.Precision, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
            where TStrategy : class, IValidationStrategy, new()
        {
            if (type.IsAbstract && type.IsSealed)
                return ObjectVisitorFactoryCore.CreateForStaticType(type, kind, LvMode.FULL, validationStrategy, strictMode);
            return ObjectVisitorFactoryCore.CreateForFutureInstance(type, kind, repeatable, LvMode.FULL, validationStrategy, strictMode);
        }

        public static IObjectVisitor Create<TStrategy>(Type type, IDictionary<string, object> initialValues, TStrategy validationStrategy, AlgorithmKind kind = AlgorithmKind.Precision, bool repeatable = RpMode.REPEATABLE,
            bool strictMode = StMode.NORMALE) where TStrategy : class, IValidationStrategy, new()
        {
            if (type.IsAbstract && type.IsSealed)
                return ObjectVisitorFactoryCore.CreateForStaticType(type, kind, LvMode.FULL, validationStrategy, strictMode);
            return ObjectVisitorFactoryCore.CreateForFutureInstance(type, kind, repeatable, LvMode.FULL, validationStrategy, strictMode, initialValues);
        }

        public static IObjectVisitor<T> Create<T, TStrategy>(T instance, AlgorithmKind kind = AlgorithmKind.Precision, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE) where TStrategy : class, IValidationStrategy<T>, new()
        {
            return ObjectVisitorFactoryCore.CreateForInstance<T, TStrategy>(instance, kind, repeatable, LvMode.FULL, strictMode);
        }

        public static IObjectVisitor<T> Create<T, TStrategy>(AlgorithmKind kind = AlgorithmKind.Precision, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE) where TStrategy : class, IValidationStrategy<T>, new()
        {
            var type = typeof(T);
            if (type.IsAbstract && type.IsSealed)
                return ObjectVisitorFactoryCore.CreateForStaticType<T, TStrategy>(kind, LvMode.FULL, strictMode);
            return ObjectVisitorFactoryCore.CreateForFutureInstance<T, TStrategy>(kind, repeatable, LvMode.FULL, strictMode);
        }

        public static IObjectVisitor<T> Create<T, TStrategy>(IDictionary<string, object> initialValues, AlgorithmKind kind = AlgorithmKind.Precision, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
            where TStrategy : class, IValidationStrategy<T>, new()
        {
            var type = typeof(T);
            if (type.IsAbstract && type.IsSealed)
                return ObjectVisitorFactoryCore.CreateForStaticType<T, TStrategy>(kind, LvMode.FULL, strictMode);
            return ObjectVisitorFactoryCore.CreateForFutureInstance<T, TStrategy>(kind, repeatable, LvMode.FULL, strictMode, initialValues);
        }
    }
}