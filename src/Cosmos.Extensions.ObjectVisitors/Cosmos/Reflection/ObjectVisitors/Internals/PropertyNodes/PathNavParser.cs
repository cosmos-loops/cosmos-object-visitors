using System.Collections.Generic;
using Cosmos.Numeric;
using Cosmos.Splitters;

namespace Cosmos.Reflection.ObjectVisitors.Internals.PropertyNodes
{
    internal static class PathNavParser
    {
        private static bool? IsDirectPath(string propertyPath)
        {
            // ""
            if (string.IsNullOrWhiteSpace(propertyPath))
                return null;

            var length = propertyPath.Length;

            //"."
            if (length == 1 && propertyPath == ".")
                return null;

            var firstIdxOfPoint = propertyPath.IndexOf('.');

            // ".." => false
            if (firstIdxOfPoint == 0)
                return null;

            // "A." => false ==> return !false -> true
            // "AA." => false ==> return !false -> true
            // "A.A" => true ==> return !true -> false
            return !NumericJudge.IsBetween(firstIdxOfPoint, 1, length - 2);
        }

        public static bool TryParse(string propertyPath, out List<string> pathSegments)
        {
            var valid = IsDirectPath(propertyPath);
            pathSegments = default;

            if (!valid.HasValue)
                return false;

            if (valid.Value)
            {
                pathSegments = new List<string> {propertyPath};
                return false;
            }

            pathSegments = Splitter.On('.').OmitEmptyStrings().TrimResults().SplitToList(propertyPath);
            return true;
        }
    }
}