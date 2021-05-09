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
    }
}