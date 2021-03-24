using System;
using Cosmos.Validation.Objects;

namespace Cosmos.Reflection.Internals.Members
{
    internal static class ObjectKindExtensions
    {
        public static VerifiableObjectKind GetObjectKind(this Type type)
        {
            return type.IsBasicType() ? VerifiableObjectKind.BasicType : VerifiableObjectKind.StructureType;
        }

        public static bool IsBasicType(this Type type)
        {
            if (type.IsPrimitive)
                return true;

            if (type == TypeClass.StringClazz)
                return true;

            if (type == TypeClass.DateTimeClazz || type == TypeClass.DateTimeNullableClazz)
                return true;

            if (type == TypeClass.DateTimeOffsetClazz || type == TypeClass.DateTimeOffsetNullableClazz)
                return true;

            if (type == TypeClass.GuidClazz || type == TypeClass.GuidNullableClazz)
                return true;

            return false;
        }
    }
}