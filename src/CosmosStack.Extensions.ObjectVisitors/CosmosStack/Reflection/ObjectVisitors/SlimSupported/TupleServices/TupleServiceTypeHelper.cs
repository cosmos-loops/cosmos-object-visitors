using System;

namespace CosmosStack.Reflection.ObjectVisitors.SlimSupported.TupleServices
{
    internal static class TupleServiceTypeHelper
    {
        public static bool IsSupportedTupleType<T>()
        {
            return IsSupportedTupleType(typeof(T));
        }

        public static bool IsSupportedTupleType(Type type)
        {
            if (type is null)
                return false;

            if (Types.IsTupleType(type))
                return true;

            return false;
        }
    }
}