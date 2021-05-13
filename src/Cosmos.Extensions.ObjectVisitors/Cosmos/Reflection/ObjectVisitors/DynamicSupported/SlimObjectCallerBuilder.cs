using System;
using Cosmos.Reflection.ObjectVisitors.Core;

namespace Cosmos.Reflection.ObjectVisitors.DynamicSupported
{
    internal static class SlimObjectCallerBuilder
    {
        public static unsafe ObjectCallerBase Ctor(Type type)
        {
            ObjectCallerBase caller;

            if (DynamicTypeHelper.IsSupportedDynamicType(type))
                caller = new SlimObjectCaller();
            else
                throw new InvalidOperationException("Is not a valid dynamic type.");

            return caller;
        }
    }

    internal static class SlimObjectCallerBuilder<T>
    {
        public static Func<ObjectCallerBase> Ctor => () => SlimObjectCallerBuilder.Ctor(typeof(T));
    }
}