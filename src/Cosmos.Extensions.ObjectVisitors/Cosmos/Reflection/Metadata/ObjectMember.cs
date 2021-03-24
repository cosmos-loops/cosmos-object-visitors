using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Cosmos.Validation.Objects;

namespace Cosmos.Reflection.Metadata
{
    public sealed class ObjectMember
    {
        public ObjectMember(
            bool canWrite,
            bool canRead,
            bool isConst,
            string name,
            Type type,
            bool isStatic,
            bool isAsync,
            bool isAbstract,
            bool isVirtual,
            bool isNew,
            bool isOverride,
            bool isReadonly,
            VerifiableMemberKind kind
        )
        {
            CanWrite = canWrite;
            CanRead = canRead;
            IsConst = isConst;
            IsStatic = isStatic;
            IsAsync = isAsync;
            IsAbstract = isAbstract;
            IsVirtual = isVirtual;
            IsOverride = isOverride;
            IsNew = isNew;
            MemberName = name;
            MemberType = type;
            IsArray = MemberType.IsArray;
            IsInterface = type.IsInterface;
            IsReadOnly = isReadonly;
            Kind = kind;

            if (IsArray)
            {
                var currentElementType = MemberType.GetElementType();
                var currentArrayLayer = 0;
                while (currentElementType.HasElementType)
                {
                    currentArrayLayer += 1;
                    currentElementType = currentElementType.GetElementType();
                }

                ElementType = currentElementType;
                ArrayLayer = currentArrayLayer;
                ArrayDimensions = MemberType.GetArrayRank();
            }
            else
            {
                ElementType = default;
                ArrayLayer = default;
                ArrayDimensions = default;
            }

#if NETFRAMEWORK

            if (isStatic) { }

#endif
        }

        public bool CanWrite { get; }

        public bool CanRead { get; }

        public bool IsReadOnly { get; }

        public bool IsAsync { get; }

        public bool IsConst { get; }

        public bool IsArray { get; }

        public bool IsStatic { get; }

        public bool IsAbstract { get; }

        public bool IsVirtual { get; }

        public bool IsInterface { get; }

        public bool IsOverride { get; }

        public bool IsNew { get; }

        public string MemberName { get; }

        public Type MemberType { get; }

        public Type ElementType { get; }

        public int ArrayLayer { get; }

        public int ArrayDimensions { get; }

        public VerifiableMemberKind Kind { get; }

        public static implicit operator ObjectMember(FieldInfo info)
        {
            var isNew = false;
            var baseType = info.DeclaringType.BaseType;
            if (baseType != null && baseType != typeof(object))
            {
                isNew = baseType.GetField(info.Name) != null;
            }

            return new ObjectMember(
                !info.IsInitOnly,
                !info.IsPrivate,
                info.IsLiteral,
                info.Name,
                info.FieldType,
                info.IsStatic,
                false,
                false,
                false,
                isNew,
                false,
                info.IsInitOnly,
                VerifiableMemberKind.Field);
        }

        public static implicit operator ObjectMember(PropertyInfo info)
        {
            var (isAsync, isStatic, isAbstract, isVirtual, isNew, isOverride) =
                info.CanRead
                    ? GetMethodInfo(info.GetGetMethod())
                    : GetMethodInfo(info.GetSetMethod());

            return new ObjectMember(
                info.CanWrite,
                info.CanRead,
                info.CanRead && !info.CanWrite,
                info.Name,
                info.PropertyType,
                isStatic,
                isAsync,
                isAbstract,
                isVirtual,
                isNew,
                isOverride,
                false,
                VerifiableMemberKind.Property);
        }

        public static implicit operator ObjectMember(MethodInfo info)
        {
            var (isAsync, isStatic, isAbstract, isVirtual, isNew, isOverride) = GetMethodInfo(info);
            return new ObjectMember(
                false,
                true,
                false,
                info.Name,
                info.ReturnType,
                isStatic,
                isAsync,
                isAbstract,
                isVirtual,
                isNew,
                isOverride,
                false,
                VerifiableMemberKind.Property);
        }

        public static (bool isAsync,
            bool isStatic,
            bool isAbstract,
            bool isVirtual,
            bool isNew,
            bool isOverride
            ) GetMethodInfo(MethodInfo info)
        {
            var isAsync = info?.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) != null;
            bool isStatic = info?.IsStatic ?? false;
            bool isAbstract = false;
            bool isVirtual = false;
            bool isNew = false;
            bool isOverride = false;
            if (info is not null && !info.DeclaringType.IsInterface && !isStatic)
            {
                //如果没有被重写
                if (info.Equals(info.GetBaseDefinition()))
                {
                    if (info.IsAbstract)
                    {
                        isAbstract = true;
                    }
                    else if (!info.IsFinal && info.IsVirtual)
                    {
                        isVirtual = true;
                    }
                    else
                    {
                        var baseType = info.DeclaringType.BaseType;
                        if (baseType != null && baseType != typeof(object))
                        {
                            var baseInfo = info.DeclaringType.BaseType.GetMethod(info.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                            if (info != baseInfo)
                            {
                                isNew = true;
                            }
                        }
                    }
                }
                else
                {
                    isOverride = true;
                }
            }

            return (isAsync, isStatic, isAbstract, isVirtual, isNew, isOverride);
        }

#if NETFRAMEWORK
        internal Func<object, object> ToGetValue { get; set; }

        internal Action<object, object> ToSetValue { get; set; }

#endif
    }
}