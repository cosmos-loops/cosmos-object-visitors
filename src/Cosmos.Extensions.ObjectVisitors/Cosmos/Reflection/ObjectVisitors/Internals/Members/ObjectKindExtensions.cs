using System;
using Cosmos.Validation.Objects;

namespace Cosmos.Reflection.ObjectVisitors.Internals.Members
{
    internal static class ObjectKindExtensions
    {
        public static VerifiableObjectKind GetObjectKind(this Type type)
        {
            return type.IsBasicType() ? VerifiableObjectKind.BasicType : VerifiableObjectKind.StructureType;
        }
    }
}