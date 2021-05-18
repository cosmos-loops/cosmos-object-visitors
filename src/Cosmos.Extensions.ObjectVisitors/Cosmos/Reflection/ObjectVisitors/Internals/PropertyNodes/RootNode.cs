using System;
using System.Collections.Generic;
using Cosmos.Reflection.ObjectVisitors.Core;
using Cosmos.Reflection.ObjectVisitors.Metadata;
using Cosmos.Validation.Objects;

namespace Cosmos.Reflection.ObjectVisitors.Internals.PropertyNodes
{
    internal static class RootNode
    {
        public static Dictionary<string, Lazy<IPropertyNodeVisitor>> New(
            ObjectCallerBase handler,
            AlgorithmKind kind,
            bool repeatable,
            bool strictMode)
        {
            var result = new Dictionary<string, Lazy<IPropertyNodeVisitor>>();


            foreach (var member in handler.GetMembers())
            {
                if (member is null)
                    continue;

                if (member.MemberType.IsBasicType())
                    result[member.MemberName] = null;

                else
                    result[member.MemberName] = new(() => PropertyNodeFactory.Create(member, handler, kind, repeatable, strictMode));
            }

            return result;
        }
    }
}