using System;

namespace Cosmos.Reflection.Metadata
{
    /// <summary>
    /// Kind of algorithm to find tree
    /// </summary>
    [Flags]
    public enum AlgorithmKind
    {
        Fuzzy,
        Hash,
        Precision
    }
}