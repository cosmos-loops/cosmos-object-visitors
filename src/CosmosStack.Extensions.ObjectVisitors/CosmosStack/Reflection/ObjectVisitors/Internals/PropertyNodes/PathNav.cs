using System;
using System.Collections.Generic;

namespace CosmosStack.Reflection.ObjectVisitors.Internals.PropertyNodes
{
    internal class PathNav
    {
        public PathNav(string value, PathNavTypes type, bool root)
        {
            Next = default;
            Value = value;
            NavType = type;
            IsRoot = root;
        }

        public PathNav(string value, PathNavTypes type, PathNav next, bool root)
        {
            Next = next;
            Value = value;
            NavType = type;
            IsRoot = root;
        }

        public PathNavTypes NavType { get; }

        public PathNav Next { get; }

        public bool HasNext() => Next is not null;

        public PathNav GetNext(int index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (index > 0 && IsEnd())
                throw new ArgumentOutOfRangeException(nameof(index));
            if (index == 0)
                return this;
            return Next.GetNext(index - 1);
        }

        public bool IsEnd() => !HasNext();

        public bool IsRoot { get; }

        public string Value { get; }

        public string GetValue() => Value;

        public string GetValue(int index) => GetNext(index).GetValue();

        public int[] IndexOfArray { get; } = {-1}; //当前暂不支持 Array 模式，因此固定为「一位数组」，且 index 值为「-1」

        public static PathNav Parse(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));
            if (PathNavParser.TryParse(path, out var segments))
                return MakePathNavImpl(segments);
            throw new FormatException($"The path '{path}' is invalid.");
        }

        private static PathNav MakePathNavImpl(List<string> pathSegments)
        {
            PathNav current = null;

            for (var i = pathSegments.Count - 1; i >= 0; --i)
            {
                //TODO 检测 segment 是否是数组，形如 A[0] 或 A[1][2]
                current = new PathNav(pathSegments[i], PathNavTypes.Normal, current, i == 0);
            }

            return current;
        }
    }
}