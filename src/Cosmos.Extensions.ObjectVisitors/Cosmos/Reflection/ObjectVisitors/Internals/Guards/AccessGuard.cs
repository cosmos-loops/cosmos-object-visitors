using System;
using Cosmos.Reflection.ObjectVisitors.Metadata;
using Cosmos.Validation.Internals.Extensions;

namespace Cosmos.Reflection.ObjectVisitors.Internals.Guards
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