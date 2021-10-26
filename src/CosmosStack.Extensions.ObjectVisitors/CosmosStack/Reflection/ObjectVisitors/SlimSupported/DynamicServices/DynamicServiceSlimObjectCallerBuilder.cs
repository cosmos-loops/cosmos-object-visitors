using System;
using CosmosStack.Reflection.ObjectVisitors.Core;

namespace CosmosStack.Reflection.ObjectVisitors.SlimSupported.DynamicServices
{
    internal static class DynamicServiceSlimObjectCallerBuilder
    {
        public static unsafe ObjectCallerBase Ctor(Type type)
        {
            var caller = DynamicServiceTypeHelper.Create(type);

            if (caller is null)
                throw new InvalidOperationException("Is not a valid dynamic type.");

            return caller;
        }
    }

    internal static class DynamicServiceSlimObjectCallerBuilder<T>
    {
        public static Func<ObjectCallerBase> Ctor => () => DynamicServiceSlimObjectCallerBuilder.Ctor(typeof(T));
    }
}