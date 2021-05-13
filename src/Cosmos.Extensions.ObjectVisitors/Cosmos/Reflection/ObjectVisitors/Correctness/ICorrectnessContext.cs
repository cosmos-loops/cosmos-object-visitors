using Cosmos.Reflection.ObjectVisitors.Correctness.Interfaces;

namespace Cosmos.Reflection.ObjectVisitors.Correctness
{
    public interface IValidationEntry :
        IMayRegisterByStrategyForOv,
        IMayRegisterForMemberForOv,
        IMayExposeRulePackage,
        IMayOpsWithRulePackage
    {
        bool StrictMode { get; set; }
    }

    public interface IValidationEntry<T> :
        IMayRegisterByStrategyForOv<T>,
        IMayRegisterForMemberForOv<T>,
        IMayExposeRulePackage,
        IMayOpsWithRulePackage<T>
    {
        bool StrictMode { get; set; }
    }
}