using System.Dynamic;
using CosmosStack.Reflection.ObjectVisitors.Core;

namespace CosmosStack.Reflection.ObjectVisitors.SlimSupported.DynamicServices
{
    internal static class DynamicServiceTypeExtensions
    {
        public static ObjectCallerBase AndSetExpandoObject(this ExpandoObjectSlimObjectCaller handler, ExpandoObject instance)
        {
            handler.SetObjInstance(instance);
            return handler;
        }

        public static ObjectCallerBase AndSetDynamicObject(this ObjectCallerBase handler, DynamicObject instance)
        {
            handler.SetObjInstance(instance);
            return handler;
        }

        public static ObjectCallerBase<T> AndSetDynamicObject<T>(this ObjectCallerBase<T> handler, T instance)
            where T : DynamicObject, new()
        {
            handler.SetObjInstance(instance);
            return handler;
        }
    }
}