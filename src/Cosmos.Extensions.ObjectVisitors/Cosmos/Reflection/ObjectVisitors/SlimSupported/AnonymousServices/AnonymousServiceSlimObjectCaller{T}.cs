#if !NETFRAMEWORK
using Cosmos.Exceptions;
using Cosmos.Reflection.ObjectVisitors.Core;
using Cosmos.Reflection.ObjectVisitors.Metadata;

namespace Cosmos.Reflection.ObjectVisitors.SlimSupported.AnonymousServices
{
    /// <summary>
    /// Anonymous service slim for Object Caller <br />
    /// 用于模拟匿名对象的 Object Caller
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class AnonymousServiceSlimObjectCaller<T> : ObjectCallerBase<T>
    {
        private readonly Dictionary<string, AnonymousServiceSlimObjectMember> _objectMembers;

        public AnonymousServiceSlimObjectCaller()
        {
            _objectMembers = new();
            foreach (var property in typeof(T).GetProperties())
                _objectMembers[property.Name] = AnonymousServiceSlimObjectMember.Of(property);
        }

        /// <inheritdoc />
        protected override HashSet<string> InternalMemberNames => _objectMembers.Keys.ToHashSet();

        /// <inheritdoc />
        public override TItem Get<TItem>(string name)
        {
            return (TItem)GetObject(name);
        }

        /// <inheritdoc />
        public override void Set(string name, object value) { }

        /// <inheritdoc />
        public override object GetObject(string name)
        {
            var @try = Try.Create(() => typeof(T).GetProperty(name)?.GetValue(Instance));
            return @try.GetSafeValue(default(object));
        }

        /// <inheritdoc />
        public override ObjectMember GetMember(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            return _objectMembers.TryGetValue(name, out var member)
                ? member
                : default;
        }
    }
}
#endif