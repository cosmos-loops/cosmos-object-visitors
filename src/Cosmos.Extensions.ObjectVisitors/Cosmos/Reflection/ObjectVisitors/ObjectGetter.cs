using System;
using Cosmos.Reflection.ObjectVisitors.Correctness;
using Cosmos.Reflection.ObjectVisitors.Internals;
using Cosmos.Reflection.ObjectVisitors.Metadata;
using Cosmos.Validation;

namespace Cosmos.Reflection.ObjectVisitors
{
    public static class ObjectGetter
    {
        static ObjectGetter()
        {
            CorrectnessVerifiableWallInitializer.InitializeAndPreheating();
            CorrectnessVerifiableWall.InitValidationProvider(ValidationMe.Use(CorrectnessVerifiableWall.GLOBAL_CORRECT_PROVIDER_KEY));
        }

        public static IFluentGetter Type(Type declaringType, AlgorithmKind kind = AlgorithmKind.Precision)
            => new FluentGetterBuilder(declaringType, ObjectVisitorOptions.Default.With(x => x.AlgorithmKind = kind));

        public static IFluentGetter Type(Type declaringType, ObjectVisitorOptions options)
            => new FluentGetterBuilder(declaringType, options ?? ObjectVisitorOptions.Default);

        public static IFluentGetter<T> Type<T>(AlgorithmKind kind = AlgorithmKind.Precision)
            => new FluentGetterBuilder<T>(ObjectVisitorOptions.Default.With(x => x.AlgorithmKind = kind));

        public static IFluentGetter<T> Type<T>(ObjectVisitorOptions options)
            => new FluentGetterBuilder<T>(options ?? ObjectVisitorOptions.Default);

        public static IPropertyValueGetter ValueGetter => new ObjectPropertyValueGetter();
    }
}