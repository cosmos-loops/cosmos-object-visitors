using System;
using System.Dynamic;
using CosmosStack.Reflection.ObjectVisitors.Core;

namespace CosmosStack.Reflection.ObjectVisitors.SlimSupported.DynamicServices
{
    internal static class DynamicServiceTypeHelper
    {
        public static bool IsSupportedDynamicType<T>()
        {
            return IsSupportedDynamicType(typeof(T));
        }

        public static bool IsSupportedDynamicType(Type type)
        {
            if (type is null)
                return false;

            if (type == typeof(NullObjectClass))
                return false;

            if (type == typeof(ExpandoObject))
                return true;

            if (type == typeof(DynamicInstance))
                return true;

            if (type.IsDerivedFrom<DynamicObject>(TypeDerivedOptions.CanAbstract))
                return true;

            return false;
        }
        
        public static ObjectCallerBase<T> Create<T>()
        {
            return Create(typeof(T)).With<T>();
        }

        public static ObjectCallerBase Create(Type type)
        {
            if (type is null)
                return default;

            if (type == typeof(NullObjectClass))
                return default;

            if (type == typeof(ExpandoObject))
                return new ExpandoObjectSlimObjectCaller();

            if (type == typeof(DynamicInstance) || type.IsDerivedFrom<DynamicObject>(TypeDerivedOptions.CanAbstract))
            {
                var z = typeof(DynamicObjectSlimObjectCaller<>).MakeGenericType(type);
                return TypeVisit.CreateInstance<ObjectCallerBase>(z);
            }

            throw new InvalidOperationException("Is not a valid dynamic type.");
        }
    }
}