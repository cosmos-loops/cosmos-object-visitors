using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AspectCore.Extensions.Reflection;
using Cosmos.Validation;
using Cosmos.Validation.Annotations;
using Cosmos.Validation.Objects;

// ReSharper disable PrivateFieldCanBeConvertedToLocalVariable

namespace Cosmos.Reflection.Internals.Members
{
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
            _attributes = _reflectorProvider.GetCustomAttributes();
            IncludeAnnotationsForType = HasValidationAnnotationDefined(_attributes);
        }

        public Type Type { get; }

        public VerifiableObjectKind ObjectKind { get; }

        public bool IsBasicType { get; }

        #region Value

        public object GetValue(object instance, string memberName)
        {
            return GetMemberContract(memberName)?.GetValue(instance);
        }

        public object GetValue(object instance, int memberIndex)
        {
            return GetMemberContract(memberIndex)?.GetValue(instance);
        }

        public object GetValue(IDictionary<string, object> keyValueCollection, string memberName)
        {
            return GetMemberContract(memberName)?.GetValue(keyValueCollection);
        }

        public object GetValue(IDictionary<string, object> keyValueCollection, int memberIndex)
        {
            return GetMemberContract(memberIndex)?.GetValue(keyValueCollection);
        }

        #endregion

        #region MemberContract

        public VerifiableMemberContract GetMemberContract(string name)
        {
            return _map.TryGetValue(name, out var contract) ? contract : default;
        }

        public VerifiableMemberContract GetMemberContract(PropertyInfo propertyInfo)
        {
            if (propertyInfo is null)
                throw new ArgumentNullException(nameof(propertyInfo));
            return _map.TryGetValue(propertyInfo.Name, out var contract) ? contract : default;
        }

        public VerifiableMemberContract GetMemberContract(FieldInfo fieldInfo)
        {
            if (fieldInfo is null)
                throw new ArgumentNullException(nameof(fieldInfo));
            return _map.TryGetValue(fieldInfo.Name, out var contract) ? contract : default;
        }

        public VerifiableMemberContract GetMemberContract(int memberIndex)
        {
            if (memberIndex < 0 || memberIndex >= _valueKeys.Length)
                throw new ArgumentOutOfRangeException(nameof(memberIndex), memberIndex, $"Index '{memberIndex}' is out of range.");
            return GetMemberContract(_valueKeys[memberIndex]);
        }

        public IEnumerable<VerifiableMemberContract> GetMemberContracts()
        {
            return _map.Values;
        }

        public Dictionary<string, VerifiableMemberContract> GetMemberContractMap()
        {
            return _map;
        }

        public bool ContainsMember(string memberName)
        {
            return _map.ContainsKey(memberName);
        }

        #endregion

        #region Annotation / Attribute

        private bool IncludeAnnotationsForType { get; }

        public bool IncludeAnnotations => IncludeAnnotationsForType || _map.Values.Any(x => x.IncludeAnnotations);

        public IReadOnlyCollection<Attribute> Attributes => _attributes;

        public IEnumerable<TAttribute> GetAttributes<TAttribute>() where TAttribute : Attribute
        {
            foreach (var attribute in _attributes)
                if (attribute is TAttribute t)
                    yield return t;
        }

        public IEnumerable<ValidationParameterAttribute> GetParameterAnnotations()
        {
            foreach (var attribute in _attributes)
                if (attribute is ValidationParameterAttribute annotation)
                    yield return annotation;
        }

        public IEnumerable<IQuietVerifiableAnnotation> GetQuietVerifiableAnnotations()
        {
            foreach (var attribute in _attributes)
                if (attribute is IQuietVerifiableAnnotation annotation)
                    yield return annotation;
        }

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
                if (attribute is ValidationParameterAttribute)
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
}