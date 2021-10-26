using System;
using System.Reflection;
using System.Runtime.CompilerServices;

// ReSharper disable NonConstantEqualityExpressionHasConstantResult

namespace CosmosStack.Reflection.ObjectVisitors.Core.TypeHelpers
{
    internal static class TypeHelper
    {
        public static bool IsAnonymousType(Type type)
        {
            return type is not null
                && Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false)
                && type.IsGenericType
                && type.Name.StartsWith("<>")
                && type.Name.Contains("AnonymousType")
                && (type.Attributes & TypeAttributes.NotPublic) == TypeAttributes.NotPublic;
        }
    }
}