using System;
using System.Collections.Generic;
using System.Dynamic;
using Cosmos.Reflection.ObjectVisitors.Core;

namespace Cosmos.Reflection.ObjectVisitors.SlimSupported.DynamicServices
{
    internal static class DynamicServiceTypeHelper
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

        public static Dictionary<string, object> ToDictionary<T>(this T dynamicObject)
            where T : DynamicObject
        {
            var d = new Dictionary<string, object>();
            if (dynamicObject is null)
                return d;

            dynamic ptr = dynamicObject;
            foreach (var name in dynamicObject.GetDynamicMemberNames())
                d[name] = ptr[name];

            return d;
        }
    }
}