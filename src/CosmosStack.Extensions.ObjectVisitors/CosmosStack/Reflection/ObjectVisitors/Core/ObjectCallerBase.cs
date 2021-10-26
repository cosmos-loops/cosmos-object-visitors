using System.Collections.Generic;
using System.Linq;
using CosmosStack.Reflection.ObjectVisitors.Metadata;

namespace CosmosStack.Reflection.ObjectVisitors.Core
{
    /// <summary>
    /// Object caller base <br />
    /// 对象调用基类
    /// </summary>
    public abstract class ObjectCallerBase : CoreCallerBase
    {
        /// <summary>
        /// Gets or sets value by member name
        /// </summary>
        /// <param name="name"></param>
        public object this[string name]
        {
            get => GetObject(name);
            set => Set(name, value);
        }

        /// <summary>
        /// Sets object instance
        /// </summary>
        /// <param name="obj"></param>
        public abstract void SetObjInstance(object obj);

        /// <summary>
        /// Gets object instance 
        /// </summary>
        /// <returns></returns>
        public abstract object GetObjInstance();

        /// <summary>
        /// Get object
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public abstract unsafe object GetObject(string name);

        protected virtual HashSet<string> InternalMemberNames { get; } = new();

        /// <summary>
        /// Gets all members' name
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetMemberNames() => InternalMemberNames;

        /// <summary>
        /// Gets can-read member name
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetCanReadMemberNames() => GetCanReadMembers().Select(member => member.MemberName);

        /// <summary>
        /// Gets can-write member name
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetCanWriteMemberNames() => GetCanWriteMembers().Select(member => member.MemberName);

        /// <summary>
        /// Gets all members
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ObjectMember> GetMembers() => InternalMemberNames.Select(GetMember);

        /// <summary>
        /// Gets all can-read members
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ObjectMember> GetCanReadMembers() => GetMembers().Where(member => member.CanRead);

        /// <summary>
        /// Gets all can-write members
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ObjectMember> GetCanWriteMembers() => GetMembers().Where(member => member.CanWrite);

        /// <summary>
        /// Gets ObjectMember
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public abstract unsafe ObjectMember GetMember(string name);

        /// <summary>
        /// Contains
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Contains(string name)
        {
            return !string.IsNullOrWhiteSpace(name) && InternalMemberNames.Contains(name);
        }
    }
}