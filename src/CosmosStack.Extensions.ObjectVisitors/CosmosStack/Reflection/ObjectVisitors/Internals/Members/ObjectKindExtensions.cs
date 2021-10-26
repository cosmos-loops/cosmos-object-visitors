using System;
using CosmosStack.Validation.Objects;

namespace CosmosStack.Reflection.ObjectVisitors.Internals.Members
{
    internal static class ObjectKindExtensions
    {
        public static VerifiableObjectKind GetObjectKind(this Type type)
        {
            return type.IsBasicType() ? VerifiableObjectKind.BasicType : VerifiableObjectKind.StructureType;
        }
    }
}