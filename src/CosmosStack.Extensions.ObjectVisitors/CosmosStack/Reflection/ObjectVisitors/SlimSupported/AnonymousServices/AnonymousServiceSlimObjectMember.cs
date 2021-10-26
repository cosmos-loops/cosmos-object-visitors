#if !NETFRAMEWORK
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using CosmosStack.Reflection.ObjectVisitors.Metadata;
using CosmosStack.Validation.Objects;

namespace CosmosStack.Reflection.ObjectVisitors.SlimSupported.AnonymousServices
{
    /// <summary>
    /// Anonymous service slim for ObjectMember <br />
    /// 用于模拟匿名对象的 ObjectMember 元信息
    /// </summary>
    public sealed class AnonymousServiceSlimObjectMember : ObjectMember
    {
        public AnonymousServiceSlimObjectMember(
            string name,
            Type type,
            bool isAsync
        ) : base(false, true, false, name, type, false, isAsync, false, false, false, false, false, VerifiableMemberKind.Field) { }

        /// <summary>
        /// Slim for ...
        /// </summary>
        public SlimSupportedFor SlimFor => SlimSupportedFor.AnonymousObject;

        internal static AnonymousServiceSlimObjectMember Of(PropertyInfo pi)
        {
            var runtimeType = pi.PropertyType;
            var isAsync = runtimeType?.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) is not null;
            return new(pi.Name, runtimeType, isAsync);
        }
    }
}
#endif