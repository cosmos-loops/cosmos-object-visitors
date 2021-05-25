using System;
using Cosmos.Reflection.ObjectVisitors.Core;
using Cosmos.Reflection.ObjectVisitors.SlimSupported.DynamicServices;
using Cosmos.Reflection.ObjectVisitors.SlimSupported.TupleServices;

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

            if (TupleServiceTypeHelper.IsSupportedTupleType(type))
            {
                caller = TupleServiceSlimObjectCallerBuilder.Ctor(type);
                return true;
            }

            return false;
        }

        public static bool Check<T>(out Func<ObjectCallerBase> callingHandler)
        {
            callingHandler = default;

            if (DynamicServiceTypeHelper.IsSupportedDynamicType<T>())
            {
                callingHandler = DynamicServiceSlimObjectCallerBuilder<T>.Ctor;
                return true;
            }

            if (TupleServiceTypeHelper.IsSupportedTupleType<T>())
            {
                callingHandler = TupleServiceSlimObjectCallerBuilder<T>.Ctor;
                return true;
            }

            return false;
        }
    }
}