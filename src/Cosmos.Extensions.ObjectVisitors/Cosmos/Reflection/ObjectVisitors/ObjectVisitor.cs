using System;
using System.Collections.Generic;
using Cosmos.Reflection.ObjectVisitors.Correctness;
using Cosmos.Reflection.ObjectVisitors.Internals;
using Cosmos.Reflection.ObjectVisitors.Internals.Guards;
using Cosmos.Reflection.ObjectVisitors.Metadata;
using Cosmos.Validation;
using Cosmos.Validation.Strategies;

namespace Cosmos.Reflection.ObjectVisitors
{
    public static partial class ObjectVisitor
    {
        static ObjectVisitor()
        {
            CorrectnessVerifiableWallInitializer.InitializeAndPreheating();
            CorrectnessVerifiableWall.InitValidationProvider(ValidationMe.Use(CorrectnessVerifiableWall.GLOBAL_CORRECT_PROVIDER_KEY));
        }

        private static ObjectVisitorOptions FillWith(AlgorithmKind kind, bool repeatable, bool strictMode)
        {
            return ObjectVisitorOptions
                   .Default
                   .With(x => x.AlgorithmKind = kind)
                   .With(x => x.Repeatable = repeatable)
                   .With(x => x.StrictMode = strictMode);
        }

        #region Create for Type

        public static IObjectVisitor Create(Type type, AlgorithmKind kind = AlgorithmKind.Precision, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
        {
            TypeAmGuard.RejectSimpleType(type);
            var options = FillWith(kind, repeatable, strictMode);
            if (type.IsAbstract && type.IsSealed)
                return ObjectVisitorFactoryCore.CreateForStaticType(type, options);
            return ObjectVisitorFactoryCore.CreateForFutureInstance(type, options);
        }

        public static IObjectVisitor Create(Type type, ObjectVisitorOptions options)
        {
            TypeAmGuard.RejectSimpleType(type);
            if (type.IsAbstract && type.IsSealed)
                return ObjectVisitorFactoryCore.CreateForStaticType(type, options);
            return ObjectVisitorFactoryCore.CreateForFutureInstance(type, options);
        }

        public static IObjectVisitor<T> Create<T>(AlgorithmKind kind = AlgorithmKind.Precision, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
        {
            TypeAmGuard.RejectSimpleType<T>(out var type);
            var options = FillWith(kind, repeatable, strictMode);
            if (type.IsAbstract && type.IsSealed)
                return ObjectVisitorFactoryCore.CreateForStaticType<T>(options);
            return ObjectVisitorFactoryCore.CreateForFutureInstance<T>(options);
        }

        public static IObjectVisitor<T> Create<T>(ObjectVisitorOptions options)
        {
            TypeAmGuard.RejectSimpleType<T>(out var type);
            if (type.IsAbstract && type.IsSealed)
                return ObjectVisitorFactoryCore.CreateForStaticType<T>(options);
            return ObjectVisitorFactoryCore.CreateForFutureInstance<T>(options);
        }

        #endregion

        #region Create for Instance

        public static IObjectVisitor Create(Type type, object instance, AlgorithmKind kind = AlgorithmKind.Precision, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
        {
            TypeAmGuard.RejectSimpleType(type);
            var options = FillWith(kind, repeatable, strictMode);
            return ObjectVisitorFactoryCore.CreateForInstance(type, instance, options);
        }

        public static IObjectVisitor Create(Type type, object instance, ObjectVisitorOptions options)
        {
            TypeAmGuard.RejectSimpleType(type);
            return ObjectVisitorFactoryCore.CreateForInstance(type, instance, options);
        }

        public static IObjectVisitor<T> Create<T>(T instance, AlgorithmKind kind = AlgorithmKind.Precision, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
        {
            TypeAmGuard.RejectSimpleType<T>();
            var options = FillWith(kind, repeatable, strictMode);
            return ObjectVisitorFactoryCore.CreateForInstance(instance, options);
        }

        public static IObjectVisitor<T> Create<T>(T instance, ObjectVisitorOptions options)
        {
            TypeAmGuard.RejectSimpleType<T>();
            return ObjectVisitorFactoryCore.CreateForInstance(instance, options);
        }

        #endregion

        #region Create with Initial Values

        public static IObjectVisitor Create(Type type, IDictionary<string, object> initialValues, AlgorithmKind kind = AlgorithmKind.Precision, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
        {
            TypeAmGuard.RejectSimpleType(type);
            var options = FillWith(kind, repeatable, strictMode);
            if (type.IsAbstract && type.IsSealed)
                return ObjectVisitorFactoryCore.CreateForStaticType(type, options);
            return ObjectVisitorFactoryCore.CreateForFutureInstance(type, options, initialValues);
        }

        public static IObjectVisitor Create(Type type, IDictionary<string, object> initialValues, ObjectVisitorOptions options)
        {
            TypeAmGuard.RejectSimpleType(type);
            if (type.IsAbstract && type.IsSealed)
                return ObjectVisitorFactoryCore.CreateForStaticType(type, options);
            return ObjectVisitorFactoryCore.CreateForFutureInstance(type, options, initialValues);
        }

        public static IObjectVisitor<T> Create<T>(IDictionary<string, object> initialValues, AlgorithmKind kind = AlgorithmKind.Precision, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
        {
            TypeAmGuard.RejectSimpleType<T>(out var type);
            var options = FillWith(kind, repeatable, strictMode);
            if (type.IsAbstract && type.IsSealed)
                return ObjectVisitorFactoryCore.CreateForStaticType<T>(options);
            return ObjectVisitorFactoryCore.CreateForFutureInstance<T>(options, initialValues);
        }

        public static IObjectVisitor<T> Create<T>(IDictionary<string, object> initialValues, ObjectVisitorOptions options)
        {
            TypeAmGuard.RejectSimpleType<T>(out var type);
            if (type.IsAbstract && type.IsSealed)
                return ObjectVisitorFactoryCore.CreateForStaticType<T>(options);
            return ObjectVisitorFactoryCore.CreateForFutureInstance<T>(options, initialValues);
        }

        #endregion

        #region Create for Type with Verify Strategy

        public static IObjectVisitor Create<TStrategy>(Type type, TStrategy validationStrategy, AlgorithmKind kind = AlgorithmKind.Precision, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
            where TStrategy : class, IValidationStrategy, new()
        {
            TypeAmGuard.RejectSimpleType(type);
            var options = FillWith(kind, repeatable, strictMode);
            if (type.IsAbstract && type.IsSealed)
                return ObjectVisitorFactoryCore.CreateForStaticType(type, options, validationStrategy);
            return ObjectVisitorFactoryCore.CreateForFutureInstance(type, options, validationStrategy);
        }

        public static IObjectVisitor Create<TStrategy>(Type type, TStrategy validationStrategy, ObjectVisitorOptions options)
            where TStrategy : class, IValidationStrategy, new()
        {
            TypeAmGuard.RejectSimpleType(type);
            if (type.IsAbstract && type.IsSealed)
                return ObjectVisitorFactoryCore.CreateForStaticType(type, options, validationStrategy);
            return ObjectVisitorFactoryCore.CreateForFutureInstance(type, options, validationStrategy);
        }

        public static IObjectVisitor<T> Create<T, TStrategy>(AlgorithmKind kind = AlgorithmKind.Precision, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
            where TStrategy : class, IValidationStrategy<T>, new()
        {
            TypeAmGuard.RejectSimpleType<T>(out var type);
            var options = FillWith(kind, repeatable, strictMode);
            if (type.IsAbstract && type.IsSealed)
                return ObjectVisitorFactoryCore.CreateForStaticType<T, TStrategy>(options);
            return ObjectVisitorFactoryCore.CreateForFutureInstance<T, TStrategy>(options);
        }

        public static IObjectVisitor<T> Create<T, TStrategy>(ObjectVisitorOptions options)
            where TStrategy : class, IValidationStrategy<T>, new()
        {
            TypeAmGuard.RejectSimpleType<T>(out var type);
            if (type.IsAbstract && type.IsSealed)
                return ObjectVisitorFactoryCore.CreateForStaticType<T, TStrategy>(options);
            return ObjectVisitorFactoryCore.CreateForFutureInstance<T, TStrategy>(options);
        }

        #endregion

        #region Create for Instance with Verify Strategy

        public static IObjectVisitor Create<TStrategy>(Type type, object instance, TStrategy validationStrategy, AlgorithmKind kind = AlgorithmKind.Precision, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
            where TStrategy : class, IValidationStrategy, new()
        {
            TypeAmGuard.RejectSimpleType(type);
            var options = FillWith(kind, repeatable, strictMode);
            return ObjectVisitorFactoryCore.CreateForInstance(type, instance, options, validationStrategy);
        }

        public static IObjectVisitor Create<TStrategy>(Type type, object instance, TStrategy validationStrategy, ObjectVisitorOptions options)
            where TStrategy : class, IValidationStrategy, new()
        {
            TypeAmGuard.RejectSimpleType(type);
            return ObjectVisitorFactoryCore.CreateForInstance(type, instance, options, validationStrategy);
        }

        public static IObjectVisitor<T> Create<T, TStrategy>(T instance, AlgorithmKind kind = AlgorithmKind.Precision, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
            where TStrategy : class, IValidationStrategy<T>, new()
        {
            TypeAmGuard.RejectSimpleType<T>();
            var options = FillWith(kind, repeatable, strictMode);
            return ObjectVisitorFactoryCore.CreateForInstance<T, TStrategy>(instance, options);
        }

        public static IObjectVisitor<T> Create<T, TStrategy>(T instance, ObjectVisitorOptions options)
            where TStrategy : class, IValidationStrategy<T>, new()
        {
            TypeAmGuard.RejectSimpleType<T>();
            return ObjectVisitorFactoryCore.CreateForInstance<T, TStrategy>(instance, options);
        }

        #endregion

        #region Create with Initial Values and Verify Strategy

        public static IObjectVisitor Create<TStrategy>(Type type, IDictionary<string, object> initialValues, TStrategy validationStrategy, AlgorithmKind kind = AlgorithmKind.Precision, bool repeatable = RpMode.REPEATABLE,
            bool strictMode = StMode.NORMALE) where TStrategy : class, IValidationStrategy, new()
        {
            TypeAmGuard.RejectSimpleType(type);
            var options = FillWith(kind, repeatable, strictMode);
            if (type.IsAbstract && type.IsSealed)
                return ObjectVisitorFactoryCore.CreateForStaticType(type, options, validationStrategy);
            return ObjectVisitorFactoryCore.CreateForFutureInstance(type, options, validationStrategy, initialValues);
        }

        public static IObjectVisitor Create<TStrategy>(Type type, IDictionary<string, object> initialValues, TStrategy validationStrategy, ObjectVisitorOptions options)
            where TStrategy : class, IValidationStrategy, new()
        {
            TypeAmGuard.RejectSimpleType(type);
            if (type.IsAbstract && type.IsSealed)
                return ObjectVisitorFactoryCore.CreateForStaticType(type, options, validationStrategy);
            return ObjectVisitorFactoryCore.CreateForFutureInstance(type, options, validationStrategy, initialValues);
        }

        public static IObjectVisitor<T> Create<T, TStrategy>(IDictionary<string, object> initialValues, AlgorithmKind kind = AlgorithmKind.Precision, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
            where TStrategy : class, IValidationStrategy<T>, new()
        {
            TypeAmGuard.RejectSimpleType<T>(out var type);
            var options = FillWith(kind, repeatable, strictMode);
            if (type.IsAbstract && type.IsSealed)
                return ObjectVisitorFactoryCore.CreateForStaticType<T, TStrategy>(options);
            return ObjectVisitorFactoryCore.CreateForFutureInstance<T, TStrategy>(options, initialValues);
        }

        public static IObjectVisitor<T> Create<T, TStrategy>(IDictionary<string, object> initialValues, ObjectVisitorOptions options)
            where TStrategy : class, IValidationStrategy<T>, new()
        {
            TypeAmGuard.RejectSimpleType<T>(out var type);
            if (type.IsAbstract && type.IsSealed)
                return ObjectVisitorFactoryCore.CreateForStaticType<T, TStrategy>(options);
            return ObjectVisitorFactoryCore.CreateForFutureInstance<T, TStrategy>(options, initialValues);
        }

        #endregion
    }
}