namespace Cosmos.Reflection.ObjectVisitors.Metadata;

/// <summary>
/// Kind of algorithm to find tree <br />
/// 核心算法模式
/// </summary>
[Flags]
public enum AlgorithmKind
{
    Fuzzy,
    Hash,
    Precision
}