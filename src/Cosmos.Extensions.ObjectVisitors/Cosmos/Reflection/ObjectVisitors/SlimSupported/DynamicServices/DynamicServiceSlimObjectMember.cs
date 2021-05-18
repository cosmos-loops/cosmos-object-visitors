﻿using System;
using Cosmos.Reflection.ObjectVisitors.Metadata;
using Cosmos.Validation.Objects;

namespace Cosmos.Reflection.ObjectVisitors.SlimSupported.DynamicServices
{
    public sealed class DynamicServiceSlimObjectMember : ObjectMember
    {
        public DynamicServiceSlimObjectMember(
            string name,
            Type type,
            bool isAsync,
            SlimSupportedFor dynamicType
        ) : base(true, true, false, name, type, false, isAsync, false, false, false, false, false, VerifiableMemberKind.Field)
        {
            DynamicType = dynamicType;
        }

        public SlimSupportedFor DynamicType;
    }
}