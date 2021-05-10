using System;
using System.Dynamic;
using Cosmos.Reflection.ObjectVisitors.Core;

namespace Cosmos.Reflection.ObjectVisitors.DynamicSupported
{
    internal static class DynamicObjectHelper
    {
        public static bool IsSupportedDynamicType<T>()
        {
            return IsSupportedDynamicType(typeof(T));
        }

        public static bool IsSupportedDynamicType(Type type)
        {
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
    }
}