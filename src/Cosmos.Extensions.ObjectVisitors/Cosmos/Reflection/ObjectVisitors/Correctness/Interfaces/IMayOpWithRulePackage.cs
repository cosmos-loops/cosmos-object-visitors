using System;
using System.Linq.Expressions;
using Cosmos.Validation;

namespace Cosmos.Reflection.ObjectVisitors.Correctness.Interfaces
{
    public interface IMayExposeRulePackage
    {
        VerifyRulePackage ExposeRulePackage();

        VerifyMemberRulePackage ExposeMemberRulePackage(string memberName);
    }

    public interface IMayOpsWithRulePackage
    {
        IValidationEntry SetRulePackage(VerifyRulePackage package, VerifyRuleMode mode = VerifyRuleMode.Append);
        IValidationEntry SetMemberRulePackage(string name, VerifyMemberRulePackage package, VerifyRuleMode mode = VerifyRuleMode.Append);
    }

    public interface IMayOpsWithRulePackage<T>
    {
        IValidationEntry<T> SetRulePackage(VerifyRulePackage package, VerifyRuleMode mode = VerifyRuleMode.Append);

        IValidationEntry<T> SetMemberRulePackage(string memberName, VerifyMemberRulePackage package, VerifyRuleMode mode = VerifyRuleMode.Append);

        IValidationEntry<T> SetMemberRulePackage<TVal>(Expression<Func<T, TVal>> expression, VerifyMemberRulePackage package, VerifyRuleMode mode = VerifyRuleMode.Append);
    }
}