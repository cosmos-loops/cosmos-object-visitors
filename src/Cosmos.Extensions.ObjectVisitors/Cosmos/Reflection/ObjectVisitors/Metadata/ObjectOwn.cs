using System;

namespace Cosmos.Reflection.ObjectVisitors.Metadata
{
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
            return new(type, true);
        }
        
        public static ObjectOwn Of<T>()
        {
            return Of(typeof(T));
        }
    }
}