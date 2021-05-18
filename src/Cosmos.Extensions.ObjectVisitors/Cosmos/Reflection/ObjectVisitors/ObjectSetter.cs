using System;
using Cosmos.Reflection.ObjectVisitors.Correctness;
using Cosmos.Reflection.ObjectVisitors.Internals;
using Cosmos.Reflection.ObjectVisitors.Metadata;
using Cosmos.Validation;

namespace Cosmos.Reflection.ObjectVisitors
{
    public static class ObjectSetter
    {
        static ObjectSetter()
        {
            CorrectnessVerifiableWallInitializer.InitializeAndPreheating();
            CorrectnessVerifiableWall.InitValidationProvider(ValidationMe.Use(CorrectnessVerifiableWall.GLOBAL_CORRECT_PROVIDER_KEY));
        }

        public static IFluentSetter Type(Type type, AlgorithmKind kind = AlgorithmKind.Precision) 
            => new FluentSetterBuilder(type, ObjectVisitorOptions.Default.With(x => x.AlgorithmKind = kind));
        
        public static IFluentSetter Type(Type type, ObjectVisitorOptions options) 
            => new FluentSetterBuilder(type, options?? ObjectVisitorOptions.Default);
        
        public static IFluentSetter<T> Type<T>(AlgorithmKind kind = AlgorithmKind.Precision) 
            => new FluentSetterBuilder<T>(ObjectVisitorOptions.Default.With(x => x.AlgorithmKind = kind));
        
        public static IFluentSetter<T> Type<T>(ObjectVisitorOptions options) 
            => new FluentSetterBuilder<T>(options?? ObjectVisitorOptions.Default);
        
        public static IPropertyValueSetter ValueSetter => new ObjectPropertyValueSetter();
    }
}