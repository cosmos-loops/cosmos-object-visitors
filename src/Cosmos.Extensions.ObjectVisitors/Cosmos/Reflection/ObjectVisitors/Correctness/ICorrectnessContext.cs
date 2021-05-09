using Cosmos.Reflection.ObjectVisitors.Correctness.Interfaces;

namespace Cosmos.Reflection.ObjectVisitors.Correctness
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