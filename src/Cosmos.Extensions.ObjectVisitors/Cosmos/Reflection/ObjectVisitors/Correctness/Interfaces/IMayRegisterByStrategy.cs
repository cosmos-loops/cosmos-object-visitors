using Cosmos.Validation;
using Cosmos.Validation.Strategies;

namespace Cosmos.Reflection.ObjectVisitors.Correctness.Interfaces
{
    public interface IMayRegisterByStrategyForOv
    {
        IValidationEntry SetStrategy<TStrategy>(StrategyMode mode = StrategyMode.OverallOverwrite) where TStrategy : class, IValidationStrategy, new();

        IValidationEntry SetStrategy<TStrategy>(TStrategy strategy, StrategyMode mode = StrategyMode.OverallOverwrite) where TStrategy : class, IValidationStrategy, new();
    }

    public interface IMayRegisterByStrategyForOv<T>
    {
        IValidationEntry<T> SetStrategy<TStrategy>(StrategyMode mode = StrategyMode.OverallOverwrite) where TStrategy : class, IValidationStrategy<T>, new();

        IValidationEntry<T> SetStrategy<TStrategy>(TStrategy strategy, StrategyMode mode = StrategyMode.OverallOverwrite) where TStrategy : class, IValidationStrategy<T>, new();
    }
}