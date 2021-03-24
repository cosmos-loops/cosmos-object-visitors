using System;
using Cosmos.Reflection.Correctness;
using Cosmos.Reflection.Internals;
using Cosmos.Reflection.Metadata;
using Cosmos.Validation;

namespace Cosmos.Reflection
{
    public static class ObjectGetter
    {
        static ObjectGetter()
        {
            CorrectnessVerifiableWallInitializer.InitializeAndPreheating();
            CorrectnessVerifiableWall.InitValidationProvider(ValidationMe.Use(CorrectnessVerifiableWall.GLOBAL_CORRECT_PROVIDER_KEY));
        }
        
        public static IFluentGetter Type(Type declaringType, AlgorithmKind kind = AlgorithmKind.Precision) => new FluentGetterBuilder(declaringType, kind);

        public static IFluentGetter<T> Type<T>(AlgorithmKind kind = AlgorithmKind.Precision) => new FluentGetterBuilder<T>(kind);
    }
}