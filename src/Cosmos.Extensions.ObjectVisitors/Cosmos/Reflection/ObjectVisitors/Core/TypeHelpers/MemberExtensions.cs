#if NETFRAMEWORK
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cosmos.Collections;
using Cosmos.Reflection.ObjectVisitors.Metadata;

namespace Cosmos.Reflection.ObjectVisitors.Core.TypeHelpers
{
    internal static class MemberExtensions
    {
        public static Dictionary<string, ObjectMember> GetObjectMembers(this Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            var fields = new List<ObjectMember>();
            var properties = new List<ObjectMember>();
            var flags = BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            foreach (var field in type.GetFields(flags).Where(x => !(x.IsSpecialName || x.Name.Contains("<"))))
            {
                var member = (ObjectMember) field;
                member.ToGetValue = o => field.GetValue(o);
                member.ToSetValue = (o, v) => field.SetValue(o, v);
                fields.Add(member);
            }

            foreach (var property in type.GetProperties(flags))
            {
                var member = (ObjectMember) property;
                member.ToGetValue = o => property.GetValue(o);
                member.ToSetValue = (o, v) => property.SetValue(o, v);
                properties.Add(member);
            }

            return fields.Merge(properties).ToDictionary(x => x.MemberName, x => x);
        }
    }
}

#endif