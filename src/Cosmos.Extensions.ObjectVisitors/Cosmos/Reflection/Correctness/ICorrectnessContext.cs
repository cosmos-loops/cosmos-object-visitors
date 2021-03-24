using Cosmos.Reflection.Correctness.Interfaces;
using Cosmos.Validation;
using Cosmos.Validation.Strategies;

namespace Cosmos.Reflection.Correctness
{
    public interface IValidationEntry :
        IMayRegisterByStrategyForOv,
        IMayRegisterForMemberForOv
    {
        bool StrictMode { get; set; }
    }

    public interface IValidationEntry<T> :
        IMayRegisterByStrategyForOv<T>,
        IMayRegisterForMemberForOv<T>
    {
        bool StrictMode { get; set; }
    }
}