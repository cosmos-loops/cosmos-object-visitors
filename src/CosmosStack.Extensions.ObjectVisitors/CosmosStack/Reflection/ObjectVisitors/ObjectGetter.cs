using System;
using CosmosStack.Reflection.ObjectVisitors.Correctness;
using CosmosStack.Reflection.ObjectVisitors.Internals;
using CosmosStack.Reflection.ObjectVisitors.Metadata;
using CosmosStack.Validation;

namespace CosmosStack.Reflection.ObjectVisitors
{
    /// <summary>
    /// Object Getter <br />
    /// 对象获取器
    /// </summary>
    public static class ObjectGetter
    {
        static ObjectGetter()
        {
            CorrectnessVerifiableWallInitializer.InitializeAndPreheating();
            CorrectnessVerifiableWall.InitValidationProvider(ValidationMe.Use(CorrectnessVerifiableWall.GLOBAL_CORRECT_PROVIDER_KEY));
        }

        /// <summary>
        /// Set type <br />
        /// 设置类型
        /// </summary>
        /// <param name="declaringType"></param>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static IFluentGetter Type(Type declaringType, AlgorithmKind kind = AlgorithmKind.Precision)
            => new FluentGetterBuilder(declaringType, ObjectVisitorOptions.Default.With(x => x.AlgorithmKind = kind));

        /// <summary>
        /// Set type <br />
        /// 设置类型
        /// </summary>
        /// <param name="declaringType"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IFluentGetter Type(Type declaringType, ObjectVisitorOptions options)
            => new FluentGetterBuilder(declaringType, options ?? ObjectVisitorOptions.Default);

        /// <summary>
        /// Set type <br />
        /// 设置类型
        /// </summary>
        /// <param name="kind"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IFluentGetter<T> Type<T>(AlgorithmKind kind = AlgorithmKind.Precision)
            => new FluentGetterBuilder<T>(ObjectVisitorOptions.Default.With(x => x.AlgorithmKind = kind));

        /// <summary>
        /// Set type <br />
        /// 设置类型
        /// </summary>
        /// <param name="options"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IFluentGetter<T> Type<T>(ObjectVisitorOptions options)
            => new FluentGetterBuilder<T>(options ?? ObjectVisitorOptions.Default);

        /// <summary>
        /// To touch a ValueGetter <br />
        /// 获取值获取器
        /// </summary>
        public static IPropertyValueGetter ValueGetter => new ObjectPropertyValueGetter();
    }
}