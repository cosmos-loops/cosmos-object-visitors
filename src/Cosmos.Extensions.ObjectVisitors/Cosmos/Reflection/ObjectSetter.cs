using System;
using Cosmos.Reflection.Correctness;
using Cosmos.Reflection.Internals;
using Cosmos.Reflection.Metadata;
using Cosmos.Validation;

namespace Cosmos.Reflection
{
    public static class ObjectSetter
    {
        static ObjectSetter()
        {
            CorrectnessVerifiableWallInitializer.InitializeAndPreheating();
            CorrectnessVerifiableWall.InitValidationProvider(ValidationMe.Use(CorrectnessVerifiableWall.GLOBAL_CORRECT_PROVIDER_KEY));
        }

        public static IFluentSetter Type(Type type, AlgorithmKind kind = AlgorithmKind.Precision) => new FluentSetterBuilder(type, kind);

        public static IFluentSetter<T> Type<T>(AlgorithmKind kind = AlgorithmKind.Precision) => new FluentSetterBuilder<T>(kind);
    }
}