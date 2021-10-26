using System;
using CosmosStack.Reflection.ObjectVisitors.Core.TypeHelpers;

namespace CosmosStack.Reflection.ObjectVisitors.Metadata
{
    /// <summary>
    /// Object Own <br />
    /// 对象元信息
    /// </summary>
    public class ObjectOwn
    {
        public ObjectOwn(Type type, bool isReadonly)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            IsStatic = type.IsAbstract && type.IsSealed;
            IsAbstract = type.IsAbstract;
            IsArray = type.IsArray;
            IsInterface = type.IsInterface;
            IsReadOnly = isReadonly;
            IsSealdd = type.IsSealed;

            if (IsArray)
            {
                var currentElementType = Type.GetElementType();
                var currentArrayLayer = 0;
                while (currentElementType.HasElementType)
                {
                    currentArrayLayer += 1;
                    currentElementType = currentElementType.GetElementType();
                }

                ElementType = currentElementType;
                ArrayLayer = currentArrayLayer;
                ArrayDimensions = Type.GetArrayRank();
            }
            else
            {
                ElementType = default;
                ArrayLayer = default;
                ArrayDimensions = default;
            }
        }

        public bool IsReadOnly { get; }

        public bool IsArray { get; }

        public bool IsStatic { get; }

        public bool IsAbstract { get; }

        public bool IsInterface { get; }

        public bool IsSealdd { get; }

        public Type Type { get; }

        public Type ElementType { get; }

        public int ArrayLayer { get; }

        public int ArrayDimensions { get; }

        public static ObjectOwn Of(Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            if (type.IsValueType && !type.IsPrimitive && !type.IsEnum) //struct
                return new(type, true);
            if (type.IsTupleType()) // Tuple or ValueTuple
                return new(type, true);
            if (TypeHelper.IsAnonymousType(type)) // Anonymous Type
                return new(type, true);
            return new(type, false);
        }

        public static ObjectOwn Of<T>()
        {
            return Of(typeof(T));
        }
    }
}