#if !NETFRAMEWORK
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Cosmos.Reflection.ObjectVisitors.Metadata;
using Cosmos.Validation.Objects;

namespace Cosmos.Reflection.ObjectVisitors.SlimSupported.AnonymousServices
{
    public sealed class AnonymousServiceSlimObjectMember : ObjectMember
    {
        public AnonymousServiceSlimObjectMember(
            string name,
            Type type,
            bool isAsync
        ) : base(false, true, false, name, type, false, isAsync, false, false, false, false, false, VerifiableMemberKind.Field) { }

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