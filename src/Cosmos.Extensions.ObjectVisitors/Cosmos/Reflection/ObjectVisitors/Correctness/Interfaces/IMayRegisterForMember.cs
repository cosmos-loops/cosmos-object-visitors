using System.Linq.Expressions;
using System.Reflection;
using Cosmos.Validation;

namespace Cosmos.Reflection.ObjectVisitors.Correctness.Interfaces;

/// <summary>
/// May register for member for Object Visitor interface <br />
/// 可对成员进行注册接口
/// </summary>
public interface IMayRegisterForMemberForOv
{
    /// <summary>
    /// Register for member <br />
    /// 对成员进行注册
    /// </summary>
    /// <param name="name"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    IValidationEntry ForMember(string name, Func<IValueRuleBuilder, IValueRuleBuilder> func);

    /// <summary>
    /// Register for member <br />
    /// 对成员进行注册
    /// </summary>
    /// <param name="propertyInfo"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    IValidationEntry ForMember(PropertyInfo propertyInfo, Func<IValueRuleBuilder, IValueRuleBuilder> func);

    /// <summary>
    /// Register for member <br />
    /// 对成员进行注册
    /// </summary>
    /// <param name="fieldInfo"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    IValidationEntry ForMember(FieldInfo fieldInfo, Func<IValueRuleBuilder, IValueRuleBuilder> func);
}

/// <summary>
/// May register for member for Object Visitor interface <br />
/// 可对成员进行注册接口
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IMayRegisterForMemberForOv<T>
{
    /// <summary>
    /// Register for member <br />
    /// 对成员进行注册
    /// </summary>
    /// <param name="name"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    IValidationEntry<T> ForMember(string name, Func<IValueRuleBuilder<T>, IValueRuleBuilder<T>> func);

    /// <summary>
    /// Register for member <br />
    /// 对成员进行注册
    /// </summary>
    /// <param name="propertyInfo"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    IValidationEntry<T> ForMember(PropertyInfo propertyInfo, Func<IValueRuleBuilder<T>, IValueRuleBuilder<T>> func);

    /// <summary>
    /// Register for member <br />
    /// 对成员进行注册
    /// </summary>
    /// <param name="fieldInfo"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    IValidationEntry<T> ForMember(FieldInfo fieldInfo, Func<IValueRuleBuilder<T>, IValueRuleBuilder<T>> func);

    /// <summary>
    /// Register for member <br />
    /// 对成员进行注册
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="func"></param>
    /// <typeparam name="TVal"></typeparam>
    /// <returns></returns>
    IValidationEntry<T> ForMember<TVal>(Expression<Func<T, TVal>> expression, Func<IValueRuleBuilder<T, TVal>, IValueRuleBuilder<T, TVal>> func);
}