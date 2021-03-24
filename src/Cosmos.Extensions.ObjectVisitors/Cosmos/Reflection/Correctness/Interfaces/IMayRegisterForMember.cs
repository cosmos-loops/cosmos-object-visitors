using System;
using System.Linq.Expressions;
using System.Reflection;
using Cosmos.Validation;

namespace Cosmos.Reflection.Correctness.Interfaces
{
    public interface IMayRegisterForMemberForOv
    {
        IValidationEntry ForMember(string name, Func<IValueRuleBuilder, IValueRuleBuilder> func);

        IValidationEntry ForMember(PropertyInfo propertyInfo, Func<IValueRuleBuilder, IValueRuleBuilder> func);

        IValidationEntry ForMember(FieldInfo fieldInfo, Func<IValueRuleBuilder, IValueRuleBuilder> func);
    }

    public interface IMayRegisterForMemberForOv<T>
    {
        IValidationEntry<T> ForMember(string name, Func<IValueRuleBuilder<T>, IValueRuleBuilder<T>> func);

        IValidationEntry<T> ForMember(PropertyInfo propertyInfo, Func<IValueRuleBuilder<T>, IValueRuleBuilder<T>> func);

        IValidationEntry<T> ForMember(FieldInfo fieldInfo, Func<IValueRuleBuilder<T>, IValueRuleBuilder<T>> func);

        IValidationEntry<T> ForMember<TVal>(Expression<Func<T, TVal>> expression, Func<IValueRuleBuilder<T, TVal>, IValueRuleBuilder<T, TVal>> func);
    }
}