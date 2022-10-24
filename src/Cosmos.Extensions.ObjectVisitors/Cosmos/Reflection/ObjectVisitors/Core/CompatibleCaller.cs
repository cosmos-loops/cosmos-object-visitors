#if NETFRAMEWORK
using System;
using System.Collections.Generic;
using Cosmos.Reflection.ObjectVisitors.Metadata;
using FastMember;

namespace Cosmos.Reflection.ObjectVisitors.Core
{
    /// <summary>
    /// A compatible object caller for Cosmos
    /// </summary>
    public sealed class CompatibleObjectCaller<TTarget> : ObjectCallerBase<TTarget>
    {
        private readonly Dictionary<string, ObjectMember> _objectMembers;
        private readonly TypeAccessor _typeAccessor;

        public CompatibleObjectCaller(TypeAccessor accessor, Dictionary<string, ObjectMember> members)
        {
            if (members is null) throw new ArgumentNullException(nameof(members));
            _typeAccessor = accessor ?? throw new ArgumentNullException(nameof(accessor));

            // Update Internal Member Names
            foreach (var pair in members)
                InternalMemberNames.Add(pair.Key);

            // Update ObjectMember collection for .NET452
            _objectMembers = members;
        }

        /// <inheritdoc />
        public override void New()
        {
            if (_typeAccessor.CreateNewSupported)
            {
                Instance = (TTarget) _typeAccessor.CreateNew();
            }
        }

        /// <inheritdoc />
        public override object GetObjInstance()
        {
            return Instance;
        }

        /// <inheritdoc />
        public override T Get<T>(string name)
        {
            return (T) GetObject(name);
        }

        /// <inheritdoc />
        public override void Set(string name, object value)
        {
            var member = GetMember(name);
            if (member is null)
                return;
            if (member.IsStatic)
                member.ToSetValue(Instance, value);
            else
                _typeAccessor[Instance, name] = value;
        }

        /// <inheritdoc />
        public override object GetObject(string name)
        {
            var member = GetMember(name);
            if (member is null)
                return default;
            if (member.IsStatic)
                return member.ToGetValue(Instance);
            return _typeAccessor[Instance, name];
        }

        /// <inheritdoc />
        public override ObjectMember GetMember(string name)
        {
            return _objectMembers.TryGetValue(name, out var result) ? result : default;
        }
    }
}

#endif