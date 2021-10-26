using System;
using System.Collections.Generic;
using CosmosStack.Reflection.ObjectVisitors.Core;
using CosmosStack.Validation.Objects;

namespace CosmosStack.Reflection.ObjectVisitors.Internals.PropertyNodes
{
    internal static class RootNode
    {
        public static Dictionary<string, Lazy<IPropertyNodeVisitor>> New(ObjectCallerBase handler, ObjectVisitorOptions options)
        {
            var result = new Dictionary<string, Lazy<IPropertyNodeVisitor>>();


            foreach (var member in handler.GetMembers())
            {
                if (member is null)
                    continue;

                if (member.MemberType.IsBasicType())
                    result[member.MemberName] = null;

                else
                    result[member.MemberName] = new(() => PropertyNodeFactory.Create(member, handler, options));
            }

            return result;
        }
    }
}