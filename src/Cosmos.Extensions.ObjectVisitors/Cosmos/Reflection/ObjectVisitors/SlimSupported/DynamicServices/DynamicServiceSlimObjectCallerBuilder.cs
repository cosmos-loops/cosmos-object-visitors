using System;
using Cosmos.Reflection.ObjectVisitors.Core;

namespace Cosmos.Reflection.ObjectVisitors.SlimSupported.DynamicServices
{
    internal static class DynamicServiceSlimObjectCallerBuilder
    {
        public static unsafe ObjectCallerBase Ctor(Type type)
        {
            ObjectCallerBase caller;

            if (DynamicServiceTypeHelper.IsSupportedDynamicType(type))
                caller = new DynamicServiceSlimObjectCaller();
            else
                throw new InvalidOperationException("Is not a valid dynamic type.");

            return caller;
        }
    }

    internal static class SlimObjectCallerBuilder<T>
    {
        public static Func<ObjectCallerBase> Ctor => () => DynamicServiceSlimObjectCallerBuilder.Ctor(typeof(T));
    }
}