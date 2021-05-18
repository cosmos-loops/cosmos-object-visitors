using System;
using Cosmos.Reflection.ObjectVisitors.Core;
using Cosmos.Reflection.ObjectVisitors.SlimSupported.DynamicServices;

namespace Cosmos.Reflection.ObjectVisitors.SlimSupported
{
    internal static class SlimDeterminer
    {
        public static bool Check(Type type, out ObjectCallerBase caller)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            
            caller = default;
            
            if (DynamicServiceTypeHelper.IsSupportedDynamicType(type))
            {
                caller = DynamicServiceSlimObjectCallerBuilder.Ctor(type);
                return true;
            }

            return false;
        }

        public static bool Check<T>(out Func<ObjectCallerBase> callingHandler)
        {
            callingHandler = default;
            
            if (DynamicServiceTypeHelper.IsSupportedDynamicType<T>())
            {
                callingHandler = SlimObjectCallerBuilder<T>.Ctor;
                return true;
            }

            return false;
        }
    }
}