using System;
using System.Collections.Generic;
using System.Linq;
using AspectCore.Extensions.Reflection;
using CosmosStack.Reflection.ObjectVisitors.Core;
using CosmosStack.Reflection.ObjectVisitors.Metadata;
using CosmosStack.Validation;
using CosmosStack.Validation.Annotations;
using CosmosStack.Validation.Objects;

// ReSharper disable SuspiciousTypeConversion.Global
// ReSharper disable PrivateFieldCanBeConvertedToLocalVariable

namespace CosmosStack.Reflection.ObjectVisitors.Internals.Members
{
    /// <summary>
    /// An Implementation of member value contract <br />
    /// 对象的值约定实现
    /// </summary>
    public class MemberValueContractImpl : ICustomVerifiableMemberContractImpl
    {
        private readonly ObjectMember _member;
        private readonly Lazy<ObjectCallerBase> _lazyCallingHandler;
        private readonly Attribute[] _attributes;

        private readonly ICustomAttributeReflectorProvider _reflectorProvider;

        internal MemberValueContractImpl(ObjectMember member, Type declaringType)
        {
            _member = member ?? throw new ArgumentNullException(nameof(member));
            _lazyCallingHandler = new Lazy<ObjectCallerBase>(() => SafeObjectHandleSwitcher.Switch(AlgorithmKind.Precision)(declaringType));

            DeclaringType = declaringType;
            MemberKind = member.Kind;
            IsBasicType = member.MemberType.IsBasicType();

            _reflectorProvider = member.MemberType.GetReflector();
            _attributes = _reflectorProvider.GetCustomAttributes();
            IncludeAnnotations = HasValidationAnnotationDefined(_attributes);
        }

        /// <inheritdoc />
        public string MemberName => _member.MemberName;

        /// <inheritdoc />
        public Type DeclaringType { get; }

        /// <inheritdoc />
        public Type MemberType => _member.MemberType;

        /// <inheritdoc />
        public bool IsBasicType { get; }

        /// <inheritdoc />
        public VerifiableMemberKind MemberKind { get; }

        #region Value

        /// <inheritdoc />
        public object GetValue(object instance)
        {
            _lazyCallingHandler.Value.SetObjInstance(instance);
            return _lazyCallingHandler.Value.GetObject(MemberName);
        }

        /// <inheritdoc />
        public object GetValue(IDictionary<string, object> keyValueCollection)
        {
            if (keyValueCollection is null)
                return default;
            return keyValueCollection.TryGetValue(MemberName, out var value) ? value : default;
        }

        #endregion

        #region Annotation / Attribute

        /// <inheritdoc />
        public bool IncludeAnnotations { get; }

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
        public IEnumerable<IQuietVerifiableAnnotation> GetQuietVerifiableAnnotations(
            bool excludeFlagAnnotation = false,
            bool excludeObjectContextVerifiableAnnotation = false,
            bool excludeStrongVerifiableAnnotation = false)
        {
            foreach (var attribute in _attributes)
            {
                if (attribute is IQuietVerifiableAnnotation annotation)
                {
                    var skipAndGoNext = excludeFlagAnnotation && annotation is IFlagAnnotation;

                    if (!skipAndGoNext && excludeObjectContextVerifiableAnnotation && annotation is IObjectContextVerifiableAnnotation)
                        skipAndGoNext = true;

                    if (!skipAndGoNext && excludeStrongVerifiableAnnotation && annotation is IStrongVerifiableAnnotation)
                        skipAndGoNext = true;

                    if (!skipAndGoNext)
                        yield return annotation;
                }
            }
        }

        /// <inheritdoc />
        public IEnumerable<IStrongVerifiableAnnotation> GetStrongVerifiableAnnotations(
            bool excludeFlagAnnotation = false,
            bool excludeObjectContextVerifiableAnnotation = false)
        {
            foreach (var attribute in _attributes)
            {
                if (attribute is IStrongVerifiableAnnotation annotation)
                {
                    var skipAndGoNext = excludeFlagAnnotation && annotation is IFlagAnnotation;

                    if (!skipAndGoNext && excludeObjectContextVerifiableAnnotation && annotation is IObjectContextVerifiableAnnotation)
                        skipAndGoNext = true;

                    if (!skipAndGoNext)
                        yield return annotation;
                }
            }
        }

        /// <inheritdoc />
        public IEnumerable<IObjectContextVerifiableAnnotation> GetObjectContextVerifiableAnnotations(
            bool excludeFlagAnnotation = false)
        {
            foreach (var attribute in _attributes)
            {
                if (attribute is IObjectContextVerifiableAnnotation annotation)
                {
                    var skipAndGoNext = excludeFlagAnnotation && annotation is IFlagAnnotation;

                    if (!skipAndGoNext)
                        yield return annotation;
                }
            }
        }

        /// <inheritdoc />
        public IEnumerable<IFlagAnnotation> GetFlagAnnotations(
            bool excludeVerifiableAnnotation = false)
        {
            foreach (var attribute in _attributes)
            {
                if (attribute is IFlagAnnotation annotation)
                {
                    if (excludeVerifiableAnnotation)
                    {
                        if (annotation is not IQuietVerifiableAnnotation &&
                            annotation is not IStrongVerifiableAnnotation &&
                            annotation is not IObjectContextVerifiableAnnotation)
                            yield return annotation;
                    }
                    else
                    {
                        yield return annotation;
                    }
                }
            }
        }

        /// <inheritdoc />
        public IEnumerable<IVerifiable> GetVerifiableAnnotations(
            bool excludeFlagAnnotation = false)
        {
            foreach (var attribute in _attributes)
            {
                if (attribute is IVerifiable annotation)
                {
                    if (excludeFlagAnnotation)
                    {
                        if (annotation is not IFlagAnnotation)
                            yield return annotation;
                        else if (annotation is IQuietVerifiableAnnotation ||
                                 annotation is IStrongVerifiableAnnotation ||
                                 annotation is IObjectContextVerifiableAnnotation)
                            yield return annotation;
                    }
                    else
                    {
                        yield return annotation;
                    }
                }
            }
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

        /// <inheritdoc />
        public bool HasAttributeDefined<TAttr>()
            where TAttr : Attribute
        {
            return _attributes.OfType<TAttr>().Any();
        }

        /// <inheritdoc />
        public bool HasAttributeDefined<TAttr1, TAttr2>()
            where TAttr1 : Attribute
            where TAttr2 : Attribute
        {
            foreach (var attribute in _attributes)
            {
                switch (attribute)
                {
                    case TAttr1:
                    case TAttr2:
                        return true;
                }
            }

            return false;
        }

        /// <inheritdoc />
        public bool HasAttributeDefined<TAttr1, TAttr2, TAttr3>()
            where TAttr1 : Attribute
            where TAttr2 : Attribute
            where TAttr3 : Attribute
        {
            foreach (var attribute in _attributes)
            {
                switch (attribute)
                {
                    case TAttr1:
                    case TAttr2:
                    case TAttr3:
                        return true;
                }
            }

            return false;
        }

        /// <inheritdoc />
        public bool HasAttributeDefined<TAttr1, TAttr2, TAttr3, TAttr4>()
            where TAttr1 : Attribute
            where TAttr2 : Attribute
            where TAttr3 : Attribute
            where TAttr4 : Attribute
        {
            foreach (var attribute in _attributes)
            {
                switch (attribute)
                {
                    case TAttr1:
                    case TAttr2:
                    case TAttr3:
                    case TAttr4:
                        return true;
                }
            }

            return false;
        }

        /// <inheritdoc />
        public bool HasAttributeDefined<TAttr1, TAttr2, TAttr3, TAttr4, TAttr5>()
            where TAttr1 : Attribute
            where TAttr2 : Attribute
            where TAttr3 : Attribute
            where TAttr4 : Attribute
            where TAttr5 : Attribute
        {
            foreach (var attribute in _attributes)
            {
                switch (attribute)
                {
                    case TAttr1:
                    case TAttr2:
                    case TAttr3:
                    case TAttr4:
                    case TAttr5:
                        return true;
                }
            }

            return false;
        }

        public bool HasAttributeDefined<TAttr1, TAttr2, TAttr3, TAttr4, TAttr5, TAttr6>()
            where TAttr1 : Attribute
            where TAttr2 : Attribute
            where TAttr3 : Attribute
            where TAttr4 : Attribute
            where TAttr5 : Attribute
            where TAttr6 : Attribute
        {
            foreach (var attribute in _attributes)
            {
                switch (attribute)
                {
                    case TAttr1:
                    case TAttr2:
                    case TAttr3:
                    case TAttr4:
                    case TAttr5:
                    case TAttr6:
                        return true;
                }
            }

            return false;
        }

        /// <inheritdoc />
        public bool HasAttributeDefined<TAttr1, TAttr2, TAttr3, TAttr4, TAttr5, TAttr6, TAttr7>()
            where TAttr1 : Attribute
            where TAttr2 : Attribute
            where TAttr3 : Attribute
            where TAttr4 : Attribute
            where TAttr5 : Attribute
            where TAttr6 : Attribute
            where TAttr7 : Attribute
        {
            foreach (var attribute in _attributes)
            {
                switch (attribute)
                {
                    case TAttr1:
                    case TAttr2:
                    case TAttr3:
                    case TAttr4:
                    case TAttr5:
                    case TAttr6:
                    case TAttr7:
                        return true;
                }
            }

            return false;
        }

        #endregion
    }
}