using CosmosStack.Reflection.ObjectVisitors.Correctness.Interfaces;

namespace CosmosStack.Reflection.ObjectVisitors.Correctness
{
    /// <summary>
    /// An interface for the entry of Object Visitor Validation <br />
    /// 对象访问器中的验证入口接口
    /// </summary>
    public interface IValidationEntry :
        IMayRegisterByStrategyForOv,
        IMayRegisterForMemberForOv,
        IMayExposeRulePackage,
        IMayOpsWithRulePackage
    {
        /// <summary>
        /// Is strict mode <br />
        /// 是否为严格模式
        /// </summary>
        bool StrictMode { get; set; }
    }

    /// <summary>
    /// An interface for the entry of Object Visitor Validation <br />
    /// 对象访问器中的验证入口接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IValidationEntry<T> :
        IMayRegisterByStrategyForOv<T>,
        IMayRegisterForMemberForOv<T>,
        IMayExposeRulePackage,
        IMayOpsWithRulePackage<T>
    {
        /// <summary>
        /// Is strict mode <br />
        /// 是否为严格模式
        /// </summary>
        bool StrictMode { get; set; }
    }
}