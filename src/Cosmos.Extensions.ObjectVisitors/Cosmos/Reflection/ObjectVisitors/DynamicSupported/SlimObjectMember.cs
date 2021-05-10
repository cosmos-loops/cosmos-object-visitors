using System;
using Cosmos.Reflection.ObjectVisitors.Metadata;
using Cosmos.Validation.Objects;

namespace Cosmos.Reflection.ObjectVisitors.DynamicSupported
{
    public sealed class SlimObjectMember : ObjectMember
    {
        public SlimObjectMember(
            string name,
            Type type,
            bool isAsync
        ) : base(true, true, false, name, type, false, isAsync, false, false, false, false, false, VerifiableMemberKind.Field) { }
    }
}