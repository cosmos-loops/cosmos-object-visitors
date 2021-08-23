using System;

namespace Cosmos.Reflection.ObjectVisitors.Internals.Guards
{
    internal static class TypeAmGuard
    {
        public static void RejectSimpleType<T>()
        {
            RejectSimpleType(typeof(T));
        }

        public static void RejectSimpleType(Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            if (TypeJudge.IsEnumType(type))
                throw new ArgumentException("ObjectVisitor does not support direct processing of enum types.", nameof(type));

            if (TypeJudge.IsNumericType(type))
                throw new ArgumentException("ObjectVisitor does not support direct processing of numeric types.", nameof(type));

            if (type.IsPrimitive)
                throw new ArgumentException("ObjectVisitor does not support direct processing of primitive types.", nameof(type));

            if (type == typeof(string) || type == typeof(Guid) || type == typeof(Type))
                throw new ArgumentException("ObjectVisitor does not support direct processing of String, Type or Guid.", nameof(type));
        }

        public static void RejectSimpleType<T>(out Type type)
        {
            RejectSimpleType<T>();
            type = typeof(T);
        }
    }
}