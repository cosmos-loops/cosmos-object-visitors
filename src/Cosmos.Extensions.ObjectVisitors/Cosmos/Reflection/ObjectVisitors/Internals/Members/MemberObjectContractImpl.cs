using System.Reflection;
using Cosmos.Reflection.Reflectors;
using Cosmos.Validation;
using Cosmos.Validation.Annotations;
using Cosmos.Validation.Objects;

// ReSharper disable SuspiciousTypeConversion.Global
// ReSharper disable PrivateFieldCanBeConvertedToLocalVariable

namespace Cosmos.Reflection.ObjectVisitors.Internals.Members;

/// <summary>
/// An implementation of Member Object Contract <br />
/// 对象的成员约定实现
/// </summary>
public class MemberObjectContractImpl : ICustomVerifiableObjectContractImpl
{
    private readonly MemberHandler _handler;
    private readonly Attribute[] _attributes;

    private readonly ICustomAttributeReflectorProvider _reflectorProvider;

    private readonly Dictionary<string, VerifiableMemberContract> _map = new();
    private readonly string[] _valueKeys;

    internal MemberObjectContractImpl(MemberHandler handler)
    {
        _handler = handler ?? throw new ArgumentNullException(nameof(handler));

        Type = handler.SourceType;
        ObjectKind = handler.SourceType.GetObjectKind();
        IsBasicType = ObjectKind == VerifiableObjectKind.BasicType;

        foreach (var member in _handler.GetMembers())
            _map[member.MemberName] = member.ConvertToVerifiableMemberContract(Type);
        _valueKeys = _map.Keys.ToArray();

        _reflectorProvider = Type.GetReflector();
        _attributes = _reflectorProvider.GetAttributes();
        IncludeAnnotationsForType = HasValidationAnnotationDefined(_attributes);
    }

    /// <inheritdoc />
    public Type Type { get; }

    /// <inheritdoc />
    public VerifiableObjectKind ObjectKind { get; }

    /// <inheritdoc />
    public bool IsBasicType { get; }

    #region Value

    /// <inheritdoc />
    public object GetValue(object instance, string memberName)
    {
        return GetMemberContract(memberName)?.GetValue(instance);
    }

    /// <inheritdoc />
    public object GetValue(object instance, int memberIndex)
    {
        return GetMemberContract(memberIndex)?.GetValue(instance);
    }

    /// <inheritdoc />
    public object GetValue(IDictionary<string, object> keyValueCollection, string memberName)
    {
        return GetMemberContract(memberName)?.GetValue(keyValueCollection);
    }

    /// <inheritdoc />
    public object GetValue(IDictionary<string, object> keyValueCollection, int memberIndex)
    {
        return GetMemberContract(memberIndex)?.GetValue(keyValueCollection);
    }

    #endregion

    #region MemberContract

    /// <inheritdoc />
    public VerifiableMemberContract GetMemberContract(string name)
    {
        return _map.TryGetValue(name, out var contract) ? contract : default;
    }

    /// <inheritdoc />
    public VerifiableMemberContract GetMemberContract(PropertyInfo propertyInfo)
    {
        if (propertyInfo is null)
            throw new ArgumentNullException(nameof(propertyInfo));
        return _map.TryGetValue(propertyInfo.Name, out var contract) ? contract : default;
    }

    /// <inheritdoc />
    public VerifiableMemberContract GetMemberContract(FieldInfo fieldInfo)
    {
        if (fieldInfo is null)
            throw new ArgumentNullException(nameof(fieldInfo));
        return _map.TryGetValue(fieldInfo.Name, out var contract) ? contract : default;
    }

    /// <inheritdoc />
    public VerifiableMemberContract GetMemberContract(int memberIndex)
    {
        if (memberIndex < 0 || memberIndex >= _valueKeys.Length)
            throw new ArgumentOutOfRangeException(nameof(memberIndex), memberIndex, $"Index '{memberIndex}' is out of range.");
        return GetMemberContract(_valueKeys[memberIndex]);
    }

    /// <inheritdoc />
    public IEnumerable<VerifiableMemberContract> GetMemberContracts()
    {
        return _map.Values;
    }

    /// <inheritdoc />
    public Dictionary<string, VerifiableMemberContract> GetMemberContractMap()
    {
        return _map;
    }

    /// <inheritdoc />
    public bool ContainsMember(string memberName)
    {
        return _map.ContainsKey(memberName);
    }

    #endregion

    #region Annotation / Attribute

    private bool IncludeAnnotationsForType { get; }

    /// <inheritdoc />
    public bool IncludeAnnotations => IncludeAnnotationsForType || _map.Values.Any(x => x.IncludeAnnotations);

    /// <inheritdoc />
    public IReadOnlyCollection<Attribute> Attributes => _attributes;

    /// <inheritdoc />
    public IEnumerable<TAttribute> GetAttributes<TAttribute>() where TAttribute : Attribute
    {
        foreach (var attribute in _attributes)
            if (attribute is TAttribute t)
                yield return t;
    }

    /// <inheritdoc />
    public IEnumerable<VerifiableParamsAttribute> GetParameterAnnotations()
    {
        foreach (var attribute in _attributes)
            if (attribute is VerifiableParamsAttribute annotation)
                yield return annotation;
    }

    /// <inheritdoc />
    public IEnumerable<IQuietVerifiableAnnotation> GetQuietVerifiableAnnotations()
    {
        foreach (var attribute in _attributes)
            if (attribute is IQuietVerifiableAnnotation annotation)
                yield return annotation;
    }

    /// <inheritdoc />
    public IEnumerable<IStrongVerifiableAnnotation> GetStrongVerifiableAnnotations()
    {
        foreach (var attribute in _attributes)
            if (attribute is IStrongVerifiableAnnotation annotation)
                yield return annotation;
    }

    private static bool HasValidationAnnotationDefined(Attribute[] attributes)
    {
        foreach (var attribute in attributes)
        {
            if (attribute is VerifiableParamsAttribute)
                return true;
            if (Types.IsInterfaceDefined<Attribute, IFlagAnnotation>(attribute))
                return true;
            if (Types.IsInterfaceDefined<Attribute, IVerifiable>(attribute))
                return true;
        }

        return false;
    }

    #endregion
}