#if !(NET452 || NET462)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Cosmos.Conversions.Determiners;
using Cosmos.Exceptions;
using Cosmos.Reflection.ObjectVisitors.Core;
using Cosmos.Reflection.ObjectVisitors.Metadata;

namespace Cosmos.Reflection.ObjectVisitors.SlimSupported.TupleServices
{
    public sealed class TupleServiceSlimObjectCaller<T> : ObjectCallerBase<T>
        where T : ITuple
    {
        private readonly Dictionary<int, TupleServiceSlimObjectMember> _objectMembers;

        public TupleServiceSlimObjectCaller()
        {
            _objectMembers = new();
            foreach (var field in typeof(T).GetFields())
                if (CheckItemName(field.Name, out var itemNum))
                    _objectMembers[itemNum] = TupleServiceSlimObjectMember.Of(field);
        }

        private int Length => Instance.Length;

        protected override HashSet<string> InternalMemberNames => _objectMembers.Keys.Select(k => $"Item{k}").ToHashSet();

        public override TItem Get<TItem>(string name) => (TItem) GetObject(name);

        public TItem Get<TItem>(int index) => (TItem) GetObject(index);

        public override void Set(string name, object value) { }

        public override object GetObject(string name)
        {
            return CheckItemName(name, out var num)
                ? GetObject(num - 1)
                : default;
        }

        public object GetObject(int index)
        {
            var @try = Try.Create(() => Instance[index]);
            return @try.GetSafeValue(default(object));
        }

        public override ObjectMember GetMember(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            return CheckItemName(name, out var num) ? GetMember(num - 1) : default;
        }

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