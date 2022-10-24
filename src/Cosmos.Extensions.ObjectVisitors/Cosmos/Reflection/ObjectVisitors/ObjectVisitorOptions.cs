using Cosmos.Reflection.ObjectVisitors.Internals;
using Cosmos.Reflection.ObjectVisitors.Metadata;

namespace Cosmos.Reflection.ObjectVisitors;

/// <summary>
/// Object Visitor Options <br />
/// 对象访问器选项
/// </summary>
public class ObjectVisitorOptions
{
    /// <summary>
    /// Algorithm Kind <br />
    /// 算法种类
    /// </summary>
    public AlgorithmKind AlgorithmKind { get; set; } = AlgorithmKind.Precision;

    /// <summary>
    /// Lite Mode <br />
    /// 轻量级模式
    /// </summary>
    public bool LiteMode { get; set; } = LvMode.FULL;

    /// <summary>
    /// Repeat Mode <br />
    /// 可重复度模式
    /// </summary>
    public bool Repeatable { get; set; } = RpMode.REPEATABLE;

    /// <summary>
    /// Strict Mode <br />
    /// 严格模式
    /// </summary>
    public bool StrictMode { get; set; } = StMode.NORMALE;

    /// <summary>
    /// Silence if not writable <br />
    /// 如果不可写，则不抛出异常
    /// </summary>
    public bool SilenceIfNotWritable { get; set; } = true;

    /// <summary>
    /// Use configuration <br />
    /// 使用配置
    /// </summary>
    /// <param name="updateAct"></param>
    /// <returns></returns>
    public ObjectVisitorOptions With(Action<ObjectVisitorOptions> updateAct)
    {
        updateAct?.Invoke(this);
        return this;
    }

    /// <summary>
    /// Clone <br />
    /// 克隆选项
    /// </summary>
    /// <returns></returns>
    public ObjectVisitorOptions Clone()
    {
        return new()
        {
            AlgorithmKind = AlgorithmKind,
            LiteMode = LiteMode,
            Repeatable = Repeatable,
            StrictMode = StrictMode,
            SilenceIfNotWritable = SilenceIfNotWritable
        };
    }

    /// <summary>
    /// Clone <br />
    /// 克隆选项
    /// </summary>
    /// <param name="updateAct"></param>
    /// <returns></returns>
    public ObjectVisitorOptions Clone(Action<ObjectVisitorOptions> updateAct)
    {
        return Clone().With(updateAct);
    }

    /// <summary>
    /// Gets default options <br />
    /// 获取默认选项
    /// </summary>
    public static ObjectVisitorOptions Default => new();
}