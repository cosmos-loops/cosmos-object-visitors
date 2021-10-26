using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CosmosStack.Reflection.ObjectVisitors.Core;
using CosmosStack.Reflection.ObjectVisitors.Metadata;

namespace CosmosStack.Reflection.ObjectVisitors.Internals.Members
{
    internal class MemberHandler
    {
        private readonly ObjectCallerBase _handler;
        private readonly List<string> _memberNames;

        public MemberHandler(ObjectCallerBase handler, Type sourceType)
        {
            SourceType = sourceType ?? throw new ArgumentNullException(nameof(sourceType));
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            _memberNames = handler.GetMemberNames().ToList();
        }

        public bool Contains(string name) => _memberNames.Contains(name);

        public IReadOnlyList<string> GetNames() => _memberNames;

        public Type SourceType { get; }

        #region Factory

        public static Lazy<MemberHandler> Lazy(ObjectCallerBase handler, Type sourceType, bool liteMode)
        {
            return liteMode
                ? default
                : new Lazy<MemberHandler>(() => new MemberHandler(handler, sourceType));
        }

        public static MemberHandler For(Type declaringType, AlgorithmKind kind = AlgorithmKind.Precision)
        {
            if (declaringType is null) return default;
            var handler = SafeObjectHandleSwitcher.Switch(kind)(declaringType);
            return new(handler, declaringType);
        }

        public static MemberHandler For<T>(AlgorithmKind kind = AlgorithmKind.Precision)
        {
            var handler = UnsafeObjectHandleSwitcher.Switch<T>(kind)();
            return new(handler, typeof(T));
        }

        #endregion

        #region Instance

        internal object GetObjInstance() => _handler.GetObjInstance();

        internal void UpdateObjInstance(Type declaringType, object instance)
        {
            if (declaringType is not null && SourceType == declaringType)
                _handler.SetObjInstance(instance);
        }

        internal void UpdateInstance<T>(T instance)
        {
            UpdateObjInstance(typeof(T), instance);
        }

        internal object ReNew()
        {
            _handler.New();

            return _handler.GetInstance();
        }

        #endregion

        #region Value

        internal object GetValueObject(string name) => _handler[name];

        internal TVal GetValue<TVal>(string name) => _handler.Get<TVal>(name);

        internal TVal GetValue<T, TVal>(Expression<Func<T, TVal>> selector) => _handler.Get<TVal>(PropertySelector.GetPropertyName(selector));

        #endregion

        #region Index

        public ObjectMember this[string name] => _handler.GetMember(name);

        #endregion

        #region Member

        public ObjectMember GetMember(string name) => _handler.GetMember(name);

        public IEnumerable<ObjectMember> GetMembers() => _handler.GetMembers();

        #endregion
    }
}