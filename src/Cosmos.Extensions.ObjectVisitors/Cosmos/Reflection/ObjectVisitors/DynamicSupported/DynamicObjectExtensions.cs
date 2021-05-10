using System.Collections.Generic;
using System.Dynamic;

namespace Cosmos.Reflection.ObjectVisitors.DynamicSupported
{
    internal static class DynamicObjectExtensions
    {
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