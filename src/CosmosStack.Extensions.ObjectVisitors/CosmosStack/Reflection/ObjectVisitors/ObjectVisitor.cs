using System;
using System.Collections.Generic;
using CosmosStack.Reflection.ObjectVisitors.Correctness;
using CosmosStack.Reflection.ObjectVisitors.Internals;
using CosmosStack.Reflection.ObjectVisitors.Internals.Guards;
using CosmosStack.Reflection.ObjectVisitors.Metadata;
using CosmosStack.Validation;
using CosmosStack.Validation.Strategies;

namespace CosmosStack.Reflection.ObjectVisitors
{
    /// <summary>
    /// Object Visitor <br />
    /// 对象访问器
    /// </summary>
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

        /// <summary>
        /// Create an Object Visitor instance for the given type <br />
        /// 为给定的类型创建一个 Object Visitor 实例
        /// </summary>
        /// <param name="type"></param>
        /// <param name="kind"></param>
        /// <param name="repeatable"></param>
        /// <param name="strictMode"></param>
        /// <returns></returns>
        public static IObjectVisitor Create(Type type, AlgorithmKind kind = AlgorithmKind.Precision, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
        {
            TypeAmGuard.RejectSimpleType(type);
            var options = FillWith(kind, repeatable, strictMode);
            if (type.IsAbstract && type.IsSealed)
                return ObjectVisitorFactoryCore.CreateForStaticType(type, options);
            return ObjectVisitorFactoryCore.CreateForFutureInstance(type, options);
        }

        /// <summary>
        /// Create an Object Visitor instance for the given type <br />
        /// 为给定的类型创建一个 Object Visitor 实例
        /// </summary>
        /// <param name="type"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IObjectVisitor Create(Type type, ObjectVisitorOptions options)
        {
            TypeAmGuard.RejectSimpleType(type);
            if (type.IsAbstract && type.IsSealed)
                return ObjectVisitorFactoryCore.CreateForStaticType(type, options);
            return ObjectVisitorFactoryCore.CreateForFutureInstance(type, options);
        }

        /// <summary>
        /// Create an Object Visitor instance for the given type <br />
        /// 为给定的类型创建一个 Object Visitor 实例
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="repeatable"></param>
        /// <param name="strictMode"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IObjectVisitor<T> Create<T>(AlgorithmKind kind = AlgorithmKind.Precision, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
        {
            TypeAmGuard.RejectSimpleType<T>(out var type);
            var options = FillWith(kind, repeatable, strictMode);
            if (type.IsAbstract && type.IsSealed)
                return ObjectVisitorFactoryCore.CreateForStaticType<T>(options);
            return ObjectVisitorFactoryCore.CreateForFutureInstance<T>(options);
        }

        /// <summary>
        /// Create an Object Visitor instance for the given type <br />
        /// 为给定的类型创建一个 Object Visitor 实例
        /// </summary>
        /// <param name="options"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IObjectVisitor<T> Create<T>(ObjectVisitorOptions options)
        {
            TypeAmGuard.RejectSimpleType<T>(out var type);
            if (type.IsAbstract && type.IsSealed)
                return ObjectVisitorFactoryCore.CreateForStaticType<T>(options);
            return ObjectVisitorFactoryCore.CreateForFutureInstance<T>(options);
        }

        #endregion

        #region Create for Instance

        /// <summary>
        /// Create an Object Visitor instance for the given object instance <br />
        /// 为给定的对象实例创建一个 Object Visitor 实例
        /// </summary>
        /// <param name="type"></param>
        /// <param name="instance"></param>
        /// <param name="kind"></param>
        /// <param name="repeatable"></param>
        /// <param name="strictMode"></param>
        /// <returns></returns>
        public static IObjectVisitor Create(Type type, object instance, AlgorithmKind kind = AlgorithmKind.Precision, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
        {
            TypeAmGuard.RejectSimpleType(type);
            var options = FillWith(kind, repeatable, strictMode);
            return ObjectVisitorFactoryCore.CreateForInstance(type, instance, options);
        }

        /// <summary>
        /// Create an Object Visitor instance for the given object instance <br />
        /// 为给定的对象实例创建一个 Object Visitor 实例
        /// </summary>
        /// <param name="type"></param>
        /// <param name="instance"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IObjectVisitor Create(Type type, object instance, ObjectVisitorOptions options)
        {
            TypeAmGuard.RejectSimpleType(type);
            return ObjectVisitorFactoryCore.CreateForInstance(type, instance, options);
        }

        /// <summary>
        /// Create an Object Visitor instance for the given object instance <br />
        /// 为给定的对象实例创建一个 Object Visitor 实例
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="kind"></param>
        /// <param name="repeatable"></param>
        /// <param name="strictMode"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IObjectVisitor<T> Create<T>(T instance, AlgorithmKind kind = AlgorithmKind.Precision, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
        {
            TypeAmGuard.RejectSimpleType<T>();
            var options = FillWith(kind, repeatable, strictMode);
            return ObjectVisitorFactoryCore.CreateForInstance(instance, options);
        }

        /// <summary>
        /// Create an Object Visitor instance for the given object instance <br />
        /// 为给定的对象实例创建一个 Object Visitor 实例
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="options"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IObjectVisitor<T> Create<T>(T instance, ObjectVisitorOptions options)
        {
            TypeAmGuard.RejectSimpleType<T>();
            return ObjectVisitorFactoryCore.CreateForInstance(instance, options);
        }

        #endregion

        #region Create with Initial Values

        /// <summary>
        /// Create an Object Visitor instance for the given object instance and initialize it with the given dictionary <br />
        /// 为给定的对象实例创建一个 Object Visitor 实例，并通过给定的字典对其进行初始化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="initialValues"></param>
        /// <param name="kind"></param>
        /// <param name="repeatable"></param>
        /// <param name="strictMode"></param>
        /// <returns></returns>
        public static IObjectVisitor Create(Type type, IDictionary<string, object> initialValues, AlgorithmKind kind = AlgorithmKind.Precision, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
        {
            TypeAmGuard.RejectSimpleType(type);
            var options = FillWith(kind, repeatable, strictMode);
            if (type.IsAbstract && type.IsSealed)
                return ObjectVisitorFactoryCore.CreateForStaticType(type, options);
            return ObjectVisitorFactoryCore.CreateForFutureInstance(type, options, initialValues);
        }

        /// <summary>
        /// Create an Object Visitor instance for the given object instance and initialize it with the given dictionary <br />
        /// 为给定的对象实例创建一个 Object Visitor 实例，并通过给定的字典对其进行初始化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="initialValues"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IObjectVisitor Create(Type type, IDictionary<string, object> initialValues, ObjectVisitorOptions options)
        {
            TypeAmGuard.RejectSimpleType(type);
            if (type.IsAbstract && type.IsSealed)
                return ObjectVisitorFactoryCore.CreateForStaticType(type, options);
            return ObjectVisitorFactoryCore.CreateForFutureInstance(type, options, initialValues);
        }

        /// <summary>
        /// Create an Object Visitor instance for the given object instance and initialize it with the given dictionary <br />
        /// 为给定的对象实例创建一个 Object Visitor 实例，并通过给定的字典对其进行初始化
        /// </summary>
        /// <param name="initialValues"></param>
        /// <param name="kind"></param>
        /// <param name="repeatable"></param>
        /// <param name="strictMode"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IObjectVisitor<T> Create<T>(IDictionary<string, object> initialValues, AlgorithmKind kind = AlgorithmKind.Precision, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
        {
            TypeAmGuard.RejectSimpleType<T>(out var type);
            var options = FillWith(kind, repeatable, strictMode);
            if (type.IsAbstract && type.IsSealed)
                return ObjectVisitorFactoryCore.CreateForStaticType<T>(options);
            return ObjectVisitorFactoryCore.CreateForFutureInstance<T>(options, initialValues);
        }

        /// <summary>
        /// Create an Object Visitor instance for the given object instance and initialize it with the given dictionary <br />
        /// 为给定的对象实例创建一个 Object Visitor 实例，并通过给定的字典对其进行初始化
        /// </summary>
        /// <param name="initialValues"></param>
        /// <param name="options"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IObjectVisitor<T> Create<T>(IDictionary<string, object> initialValues, ObjectVisitorOptions options)
        {
            TypeAmGuard.RejectSimpleType<T>(out var type);
            if (type.IsAbstract && type.IsSealed)
                return ObjectVisitorFactoryCore.CreateForStaticType<T>(options);
            return ObjectVisitorFactoryCore.CreateForFutureInstance<T>(options, initialValues);
        }

        #endregion

        #region Create for Type with Verify Strategy

        /// <summary>
        /// Create an Object Visitor instance for the given type and use the specified verification strategy <br />
        /// 为给定的类型创建一个 Object Visitor 实例，并使用指定的验证策略
        /// </summary>
        /// <param name="type"></param>
        /// <param name="validationStrategy"></param>
        /// <param name="kind"></param>
        /// <param name="repeatable"></param>
        /// <param name="strictMode"></param>
        /// <typeparam name="TStrategy"></typeparam>
        /// <returns></returns>
        public static IObjectVisitor Create<TStrategy>(Type type, TStrategy validationStrategy, AlgorithmKind kind = AlgorithmKind.Precision, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
            where TStrategy : class, IValidationStrategy, new()
        {
            TypeAmGuard.RejectSimpleType(type);
            var options = FillWith(kind, repeatable, strictMode);
            if (type.IsAbstract && type.IsSealed)
                return ObjectVisitorFactoryCore.CreateForStaticType(type, options, validationStrategy);
            return ObjectVisitorFactoryCore.CreateForFutureInstance(type, options, validationStrategy);
        }

        /// <summary>
        /// Create an Object Visitor instance for the given type and use the specified verification strategy <br />
        /// 为给定的类型创建一个 Object Visitor 实例，并使用指定的验证策略
        /// </summary>
        /// <param name="type"></param>
        /// <param name="validationStrategy"></param>
        /// <param name="options"></param>
        /// <typeparam name="TStrategy"></typeparam>
        /// <returns></returns>
        public static IObjectVisitor Create<TStrategy>(Type type, TStrategy validationStrategy, ObjectVisitorOptions options)
            where TStrategy : class, IValidationStrategy, new()
        {
            TypeAmGuard.RejectSimpleType(type);
            if (type.IsAbstract && type.IsSealed)
                return ObjectVisitorFactoryCore.CreateForStaticType(type, options, validationStrategy);
            return ObjectVisitorFactoryCore.CreateForFutureInstance(type, options, validationStrategy);
        }

        /// <summary>
        /// Create an Object Visitor instance for the given type and use the specified verification strategy <br />
        /// 为给定的类型创建一个 Object Visitor 实例，并使用指定的验证策略
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="repeatable"></param>
        /// <param name="strictMode"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TStrategy"></typeparam>
        /// <returns></returns>
        public static IObjectVisitor<T> Create<T, TStrategy>(AlgorithmKind kind = AlgorithmKind.Precision, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
            where TStrategy : class, IValidationStrategy<T>, new()
        {
            TypeAmGuard.RejectSimpleType<T>(out var type);
            var options = FillWith(kind, repeatable, strictMode);
            if (type.IsAbstract && type.IsSealed)
                return ObjectVisitorFactoryCore.CreateForStaticType<T, TStrategy>(options);
            return ObjectVisitorFactoryCore.CreateForFutureInstance<T, TStrategy>(options);
        }

        /// <summary>
        /// Create an Object Visitor instance for the given type and use the specified verification strategy <br />
        /// 为给定的类型创建一个 Object Visitor 实例，并使用指定的验证策略
        /// </summary>
        /// <param name="options"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TStrategy"></typeparam>
        /// <returns></returns>
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

        /// <summary>
        /// Create an Object Visitor instance for a given object instance and use the specified verification strategy <br />
        /// 为给定的对象实例创建一个 Object Visitor 实例，并使用指定的验证策略
        /// </summary>
        /// <param name="type"></param>
        /// <param name="instance"></param>
        /// <param name="validationStrategy"></param>
        /// <param name="kind"></param>
        /// <param name="repeatable"></param>
        /// <param name="strictMode"></param>
        /// <typeparam name="TStrategy"></typeparam>
        /// <returns></returns>
        public static IObjectVisitor Create<TStrategy>(Type type, object instance, TStrategy validationStrategy, AlgorithmKind kind = AlgorithmKind.Precision, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
            where TStrategy : class, IValidationStrategy, new()
        {
            TypeAmGuard.RejectSimpleType(type);
            var options = FillWith(kind, repeatable, strictMode);
            return ObjectVisitorFactoryCore.CreateForInstance(type, instance, options, validationStrategy);
        }

        /// <summary>
        /// Create an Object Visitor instance for a given object instance and use the specified verification strategy <br />
        /// 为给定的对象实例创建一个 Object Visitor 实例，并使用指定的验证策略
        /// </summary>
        /// <param name="type"></param>
        /// <param name="instance"></param>
        /// <param name="validationStrategy"></param>
        /// <param name="options"></param>
        /// <typeparam name="TStrategy"></typeparam>
        /// <returns></returns>
        public static IObjectVisitor Create<TStrategy>(Type type, object instance, TStrategy validationStrategy, ObjectVisitorOptions options)
            where TStrategy : class, IValidationStrategy, new()
        {
            TypeAmGuard.RejectSimpleType(type);
            return ObjectVisitorFactoryCore.CreateForInstance(type, instance, options, validationStrategy);
        }

        /// <summary>
        /// Create an Object Visitor instance for a given object instance and use the specified verification strategy <br />
        /// 为给定的对象实例创建一个 Object Visitor 实例，并使用指定的验证策略
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="kind"></param>
        /// <param name="repeatable"></param>
        /// <param name="strictMode"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TStrategy"></typeparam>
        /// <returns></returns>
        public static IObjectVisitor<T> Create<T, TStrategy>(T instance, AlgorithmKind kind = AlgorithmKind.Precision, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
            where TStrategy : class, IValidationStrategy<T>, new()
        {
            TypeAmGuard.RejectSimpleType<T>();
            var options = FillWith(kind, repeatable, strictMode);
            return ObjectVisitorFactoryCore.CreateForInstance<T, TStrategy>(instance, options);
        }

        /// <summary>
        /// Create an Object Visitor instance for a given object instance and use the specified verification strategy <br />
        /// 为给定的对象实例创建一个 Object Visitor 实例，并使用指定的验证策略
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="options"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TStrategy"></typeparam>
        /// <returns></returns>
        public static IObjectVisitor<T> Create<T, TStrategy>(T instance, ObjectVisitorOptions options)
            where TStrategy : class, IValidationStrategy<T>, new()
        {
            TypeAmGuard.RejectSimpleType<T>();
            return ObjectVisitorFactoryCore.CreateForInstance<T, TStrategy>(instance, options);
        }

        #endregion

        #region Create with Initial Values and Verify Strategy

        /// <summary>
        /// Create an Object Visitor instance for the given object instance,
        /// use the specified verification strategy,
        /// and initialize it with the given dictionary <br />
        /// 为给定的对象实例创建一个 Object Visitor 实例，使用指定的验证策略，并通过给定的字典对其进行初始化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="initialValues"></param>
        /// <param name="validationStrategy"></param>
        /// <param name="kind"></param>
        /// <param name="repeatable"></param>
        /// <param name="strictMode"></param>
        /// <typeparam name="TStrategy"></typeparam>
        /// <returns></returns>
        public static IObjectVisitor Create<TStrategy>(Type type, IDictionary<string, object> initialValues, TStrategy validationStrategy, AlgorithmKind kind = AlgorithmKind.Precision, bool repeatable = RpMode.REPEATABLE,
            bool strictMode = StMode.NORMALE) where TStrategy : class, IValidationStrategy, new()
        {
            TypeAmGuard.RejectSimpleType(type);
            var options = FillWith(kind, repeatable, strictMode);
            if (type.IsAbstract && type.IsSealed)
                return ObjectVisitorFactoryCore.CreateForStaticType(type, options, validationStrategy);
            return ObjectVisitorFactoryCore.CreateForFutureInstance(type, options, validationStrategy, initialValues);
        }

        /// <summary>
        /// Create an Object Visitor instance for the given object instance,
        /// use the specified verification strategy,
        /// and initialize it with the given dictionary <br />
        /// 为给定的对象实例创建一个 Object Visitor 实例，使用指定的验证策略，并通过给定的字典对其进行初始化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="initialValues"></param>
        /// <param name="validationStrategy"></param>
        /// <param name="options"></param>
        /// <typeparam name="TStrategy"></typeparam>
        /// <returns></returns>
        public static IObjectVisitor Create<TStrategy>(Type type, IDictionary<string, object> initialValues, TStrategy validationStrategy, ObjectVisitorOptions options)
            where TStrategy : class, IValidationStrategy, new()
        {
            TypeAmGuard.RejectSimpleType(type);
            if (type.IsAbstract && type.IsSealed)
                return ObjectVisitorFactoryCore.CreateForStaticType(type, options, validationStrategy);
            return ObjectVisitorFactoryCore.CreateForFutureInstance(type, options, validationStrategy, initialValues);
        }

        /// <summary>
        /// Create an Object Visitor instance for the given object instance,
        /// use the specified verification strategy,
        /// and initialize it with the given dictionary <br />
        /// 为给定的对象实例创建一个 Object Visitor 实例，使用指定的验证策略，并通过给定的字典对其进行初始化
        /// </summary>
        /// <param name="initialValues"></param>
        /// <param name="kind"></param>
        /// <param name="repeatable"></param>
        /// <param name="strictMode"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TStrategy"></typeparam>
        /// <returns></returns>
        public static IObjectVisitor<T> Create<T, TStrategy>(IDictionary<string, object> initialValues, AlgorithmKind kind = AlgorithmKind.Precision, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
            where TStrategy : class, IValidationStrategy<T>, new()
        {
            TypeAmGuard.RejectSimpleType<T>(out var type);
            var options = FillWith(kind, repeatable, strictMode);
            if (type.IsAbstract && type.IsSealed)
                return ObjectVisitorFactoryCore.CreateForStaticType<T, TStrategy>(options);
            return ObjectVisitorFactoryCore.CreateForFutureInstance<T, TStrategy>(options, initialValues);
        }

        /// <summary>
        /// Create an Object Visitor instance for the given object instance,
        /// use the specified verification strategy,
        /// and initialize it with the given dictionary <br />
        /// 为给定的对象实例创建一个 Object Visitor 实例，使用指定的验证策略，并通过给定的字典对其进行初始化
        /// </summary>
        /// <param name="initialValues"></param>
        /// <param name="options"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TStrategy"></typeparam>
        /// <returns></returns>
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