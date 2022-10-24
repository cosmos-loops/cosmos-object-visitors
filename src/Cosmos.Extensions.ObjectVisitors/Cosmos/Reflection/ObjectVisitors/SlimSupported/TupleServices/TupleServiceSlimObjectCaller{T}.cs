#if !(NET452 || NET462)
using System.Runtime.CompilerServices;
using Cosmos.Conversions.Determiners;
using Cosmos.Exceptions;
using Cosmos.Reflection.ObjectVisitors.Core;
using Cosmos.Reflection.ObjectVisitors.Metadata;

namespace Cosmos.Reflection.ObjectVisitors.SlimSupported.TupleServices
{
    /// <summary>
    /// Tuple Service Slim for Object Caller <br />
    /// 元组服务模拟
    /// </summary>
    /// <typeparam name="T"></typeparam>
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

        /// <inheritdoc />
        public override TItem Get<TItem>(string name) => (TItem) GetObject(name);

        /// <summary>
        /// Get member by index <br />
        /// 根据索引获取成员
        /// </summary>
        /// <param name="index"></param>
        /// <typeparam name="TItem"></typeparam>
        /// <returns></returns>
        public TItem Get<TItem>(int index) => (TItem) GetObject(index);

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
            var @try = Try.Create(() => Instance[index]);
            return @try.GetSafeValue(default(object));
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