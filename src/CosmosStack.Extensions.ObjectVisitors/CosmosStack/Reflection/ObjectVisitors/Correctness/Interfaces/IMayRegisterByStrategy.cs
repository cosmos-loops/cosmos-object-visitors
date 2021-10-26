using CosmosStack.Validation;
using CosmosStack.Validation.Strategies;

namespace CosmosStack.Reflection.ObjectVisitors.Correctness.Interfaces
{
    /// <summary>
    /// May register by strategy for Object Visitor interface <br />
    /// 可注册策略接口
    /// </summary>
    public interface IMayRegisterByStrategyForOv
    {
        /// <summary>
        /// Set strategy <br />
        /// 设置策略
        /// </summary>
        /// <param name="mode"></param>
        /// <typeparam name="TStrategy"></typeparam>
        /// <returns></returns>
        IValidationEntry SetStrategy<TStrategy>(StrategyMode mode = StrategyMode.OverallOverwrite) where TStrategy : class, IValidationStrategy, new();

        /// <summary>
        /// Set strategy <br />
        /// 设置策略
        /// </summary>
        /// <param name="strategy"></param>
        /// <param name="mode"></param>
        /// <typeparam name="TStrategy"></typeparam>
        /// <returns></returns>
        IValidationEntry SetStrategy<TStrategy>(TStrategy strategy, StrategyMode mode = StrategyMode.OverallOverwrite) where TStrategy : class, IValidationStrategy, new();
    }

    /// <summary>
    /// May register by strategy for Object Visitor interface <br />
    /// 可注册策略接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMayRegisterByStrategyForOv<T>
    {
        /// <summary>
        /// Set strategy <br />
        /// 设置策略
        /// </summary>
        /// <param name="mode"></param>
        /// <typeparam name="TStrategy"></typeparam>
        /// <returns></returns>
        IValidationEntry<T> SetStrategy<TStrategy>(StrategyMode mode = StrategyMode.OverallOverwrite) where TStrategy : class, IValidationStrategy<T>, new();

        /// <summary>
        /// Set strategy <br />
        /// 设置策略
        /// </summary>
        /// <param name="strategy"></param>
        /// <param name="mode"></param>
        /// <typeparam name="TStrategy"></typeparam>
        /// <returns></returns>
        IValidationEntry<T> SetStrategy<TStrategy>(TStrategy strategy, StrategyMode mode = StrategyMode.OverallOverwrite) where TStrategy : class, IValidationStrategy<T>, new();
    }
}