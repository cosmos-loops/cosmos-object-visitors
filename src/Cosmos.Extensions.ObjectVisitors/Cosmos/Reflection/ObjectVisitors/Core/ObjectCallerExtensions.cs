using System.Reflection;

namespace Cosmos.Reflection.ObjectVisitors.Core
{
    internal static class ObjectCallerExtensions
    {
        public static ObjectCallerBase<TObj> With<TObj>(this ObjectCallerBase handler)
        {
            return (ObjectCallerBase<TObj>) handler;
        }

        public static object GetInstance(this ObjectCallerBase handler)
        {
            // Get 'Instance' Field and touch from 'Instance' Field by reflection.
            //
            var fieldInfo = handler.GetType().GetField("Instance", BindingFlags.Instance | BindingFlags.Public);

            return fieldInfo?.GetValue(handler);
        }

        public static TObject GetInstance<TObject>(this ObjectCallerBase<TObject> handler)
        {
            // Get 'Instance' Field and touch from 'Instance' Field by reflection.
            //
            var fieldInfo = typeof(ObjectCallerBase<TObject>)
                .GetField("Instance", BindingFlags.Instance | BindingFlags.Public);

            return (TObject) fieldInfo?.GetValue(handler);
        }

        public static ObjectCallerBase AndSetInstance(this ObjectCallerBase handler, object instance)
        {
            handler.SetObjInstance(instance);
            return handler;
        }

        public static ObjectCallerBase<T> AndSetInstance<T>(this ObjectCallerBase<T> handler, T instance)
        {
            handler.SetObjInstance(instance);
            return handler;
        }
    }
}