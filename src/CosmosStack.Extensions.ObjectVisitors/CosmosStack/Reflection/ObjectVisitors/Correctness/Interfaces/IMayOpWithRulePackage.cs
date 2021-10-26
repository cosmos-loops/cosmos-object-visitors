using System;
using System.Linq.Expressions;
using CosmosStack.Validation;

namespace CosmosStack.Reflection.ObjectVisitors.Correctness.Interfaces
{
    /// <summary>
    /// May exposed rule package interface
    /// </summary>
    public interface IMayExposeRulePackage
    {
        /// <summary>
        /// Expose rule package <br />
        /// 导出规则包
        /// </summary>
        /// <returns></returns>
        VerifyRulePackage ExposeRulePackage();

        /// <summary>
        /// Expose member rule package <br />
        /// 导出成员规则包
        /// </summary>
        /// <param name="memberName"></param>
        /// <returns></returns>
        VerifyMemberRulePackage ExposeMemberRulePackage(string memberName);
    }

    /// <summary>
    /// May ops with rule package interface
    /// </summary>
    public interface IMayOpsWithRulePackage
    {
        /// <summary>
        /// Set rule package <br />
        /// 设置规则包
        /// </summary>
        /// <param name="package"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        IValidationEntry SetRulePackage(VerifyRulePackage package, VerifyRuleMode mode = VerifyRuleMode.Append);
        
        /// <summary>
        /// Set member rule package <br />
        /// 设置成员规则包
        /// </summary>
        /// <param name="name"></param>
        /// <param name="package"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        IValidationEntry SetMemberRulePackage(string name, VerifyMemberRulePackage package, VerifyRuleMode mode = VerifyRuleMode.Append);
    }

    /// <summary>
    /// May ops with rule package interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMayOpsWithRulePackage<T>
    {
        /// <summary>
        /// Set rule package <br />
        /// 设置规则包
        /// </summary>
        /// <param name="package"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        IValidationEntry<T> SetRulePackage(VerifyRulePackage package, VerifyRuleMode mode = VerifyRuleMode.Append);

        /// <summary>
        /// Set member rule package <br />
        /// 设置成员规则包
        /// </summary>
        /// <param name="memberName"></param>
        /// <param name="package"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        IValidationEntry<T> SetMemberRulePackage(string memberName, VerifyMemberRulePackage package, VerifyRuleMode mode = VerifyRuleMode.Append);

        /// <summary>
        /// Set member rule package <br />
        /// 设置成员规则包
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="package"></param>
        /// <param name="mode"></param>
        /// <typeparam name="TVal"></typeparam>
        /// <returns></returns>
        IValidationEntry<T> SetMemberRulePackage<TVal>(Expression<Func<T, TVal>> expression, VerifyMemberRulePackage package, VerifyRuleMode mode = VerifyRuleMode.Append);
    }
}