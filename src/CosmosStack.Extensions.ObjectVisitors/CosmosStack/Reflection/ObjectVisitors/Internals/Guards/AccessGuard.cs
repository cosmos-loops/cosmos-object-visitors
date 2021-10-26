using System;
using CosmosStack.Reflection.ObjectVisitors.Metadata;
using CosmosStack.Validation.Internals.Extensions;

namespace CosmosStack.Reflection.ObjectVisitors.Internals.Guards
{
    internal static class AccessGuard
    {
        public static bool ReadOnly(ObjectOwn own, ObjectVisitorOptions options)
        {
            if (!own.IsReadOnly)
                return false;

            if (options.SilenceIfNotWritable)
                return true;

            throw new InvalidOperationException($"This type ({own.Type.GetFriendlyName()}) is read only.");
        }
    }
}