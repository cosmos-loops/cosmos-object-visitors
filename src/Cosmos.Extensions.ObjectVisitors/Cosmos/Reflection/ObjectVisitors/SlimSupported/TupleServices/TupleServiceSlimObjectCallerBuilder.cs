using System;
using Cosmos.Reflection.ObjectVisitors.Core;

#if NET452 || NET462
using Cosmos.Reflection.ObjectVisitors.Core.TypeHelpers;

#endif

namespace Cosmos.Reflection.ObjectVisitors.SlimSupported.TupleServices
{
    internal class TupleServiceSlimObjectCallerBuilder
    {
        public static unsafe ObjectCallerBase Ctor(Type type)
        {
            ObjectCallerBase caller;

            if (TupleServiceTypeHelper.IsSupportedTupleType(type))
            {
                var callerType = typeof(TupleServiceSlimObjectCaller<>).MakeGenericType(type);
#if NET452 || NET462
                var accessor = type.CreateTypeAccessor(true);
                var @params = new object[] {accessor};
                caller = TypeVisit.CreateInstance<ObjectCallerBase>(callerType, @params);
#else
                caller = TypeVisit.CreateInstance<ObjectCallerBase>(callerType);
#endif
            }
            else
                throw new InvalidOperationException("Is not a valid tuple type.");

            return caller;
        }
    }

    internal static class TupleServiceSlimObjectCallerBuilder<T>
    {
        public static Func<ObjectCallerBase> Ctor => () => TupleServiceSlimObjectCallerBuilder.Ctor(typeof(T));
    }
}