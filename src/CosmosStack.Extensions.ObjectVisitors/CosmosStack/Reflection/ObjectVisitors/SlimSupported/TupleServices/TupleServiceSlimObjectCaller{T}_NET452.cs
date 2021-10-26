#if NET452 || NET462
using System;
using System.Collections.Generic;
using System.Linq;
using CosmosStack.Collections;
using CosmosStack.Conversions.Determiners;
using CosmosStack.Reflection.ObjectVisitors.Core;
using CosmosStack.Reflection.ObjectVisitors.Metadata;
using FastMember;

namespace CosmosStack.Reflection.ObjectVisitors.SlimSupported.TupleServices
{
    /// <summary>
    /// A slim ObjectCaller for Tuple <br />
    /// 元组服务模拟
    /// </summary>
    public sealed class TupleServiceSlimObjectCaller<T> : ObjectCallerBase<T>
    {
        private readonly Dictionary<int, TupleServiceSlimObjectMember> _objectMembers;

        private readonly TypeAccessor _typeAccessor;
        //private readonly int _length;

        public TupleServiceSlimObjectCaller(TypeAccessor accessor)
        {
            _objectMembers = new();
            _typeAccessor = accessor ?? throw new ArgumentNullException(nameof(accessor));

            foreach (var member in accessor.GetMembers())
                if (CheckItemName(member.Name, out var itemNum))
                    _objectMembers[itemNum - 1] = TupleServiceSlimObjectMember.Of(member);
        }

        private int Length => _objectMembers.Keys.Count;

        protected override HashSet<string> InternalMemberNames => _objectMembers.Keys.Select(k => $"Item{k}").ToHashSet();

        /// <inheritdoc />
        public override TItem Get<TItem>(string name) => (TItem)GetObject(name);

        /// <summary>
        /// Get member by index <br />
        /// 根据索引获取成员
        /// </summary>
        /// <param name="index"></param>
        /// <typeparam name="TItem"></typeparam>
        /// <returns></returns>
        public TItem Get<TItem>(int index) => (TItem)GetObject(index);

        /// <inheritdoc />
        public override void Set(string name, object value) { }

        /// <inheritdoc />
        public override object GetObject(string name)
        {
            return CheckItemName(name, out var num)
                ? GetObject(num - 1)
                : default;
        }

        /// <summary>
        /// Get object by index <br />
        /// 根据索引获取值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object GetObject(int index)
        {
            var member = GetMember(index);
            if (member is null)
                return default;
            return _typeAccessor[Instance, member.MemberName];
        }

        /// <inheritdoc />
        public override ObjectMember GetMember(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            return CheckItemName(name, out var num) ? GetMember(num - 1) : default;
        }

        /// <summary>
        /// Get Object Member by index <br />
        /// 根据索引获取元信息
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public ObjectMember GetMember(int index)
        {
            if (index < 0 || index >= Length)
                throw new ArgumentOutOfRangeException(nameof(index));

            return _objectMembers.TryGetValue(index, out var member)
                ? member
                : default;
        }

        private static bool CheckItemName(string name, out int itemNum)
        {
            var num = 0;
            var ret = !string.IsNullOrWhiteSpace(name)
                   && name.Length >= 5
                   && name.StartsWith("Item")
                   && StringIntDeterminer.Is(name.Substring(4), matchedCallback: n => num = n);
            itemNum = num;
            return ret && num > 0;
        }
    }
}
#endif