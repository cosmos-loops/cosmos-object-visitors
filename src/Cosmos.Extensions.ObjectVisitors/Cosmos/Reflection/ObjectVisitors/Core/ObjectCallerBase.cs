using System.Collections.Generic;
using System.Linq;
using Cosmos.Reflection.ObjectVisitors.Metadata;

namespace Cosmos.Reflection.ObjectVisitors.Core
{
    public abstract class ObjectCallerBase : CoreCallerBase
    {
        public object this[string name]
        {
            get => GetObject(name);
            set => Set(name, value);
        }

        public abstract void SetObjInstance(object obj);

        public abstract object GetObjInstance();

        public abstract unsafe object GetObject(string name);

        protected virtual HashSet<string> InternalMemberNames { get; } = new();

        public IEnumerable<string> GetMemberNames() => InternalMemberNames;

        public IEnumerable<string> GetCanReadMemberNames() => GetCanReadMembers().Select(member => member.MemberName);

        public IEnumerable<string> GetCanWriteMemberNames() => GetCanWriteMembers().Select(member => member.MemberName);

        public IEnumerable<ObjectMember> GetMembers() => InternalMemberNames.Select(GetMember);

        public IEnumerable<ObjectMember> GetCanReadMembers() => GetMembers().Where(member => member.CanRead);

        public IEnumerable<ObjectMember> GetCanWriteMembers() => GetMembers().Where(member => member.CanWrite);

        public abstract unsafe ObjectMember GetMember(string name);

        public bool Contains(string name)
        {
            return !string.IsNullOrWhiteSpace(name) && InternalMemberNames.Contains(name);
        }
    }
}