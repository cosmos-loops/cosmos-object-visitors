﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Cosmos.Reflection.ObjectVisitors.Core;
using Cosmos.Reflection.ObjectVisitors.Correctness;
using Cosmos.Reflection.ObjectVisitors.Internals.Members;
using Cosmos.Reflection.ObjectVisitors.Internals.Repeat;
using Cosmos.Reflection.ObjectVisitors.Metadata;
using Cosmos.Validation;

namespace Cosmos.Reflection.ObjectVisitors.Internals.Visitors
{
    internal class FutureInstanceVisitor : IObjectVisitor, ICoreVisitor, IObjectGetter, IObjectSetter
    {
        private readonly ObjectCallerBase _handler;
        private readonly Type _sourceType;

        private readonly Lazy<MemberHandler> _lazyMemberHandler;

        private HistoricalContext NormalHistoricalContext { get; set; }

        public FutureInstanceVisitor(ObjectCallerBase handler, Type sourceType, AlgorithmKind kind, bool repeatable,
            IDictionary<string, object> initialValues = null, bool liteMode = false, bool strictMode = false)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            _sourceType = sourceType ?? throw new ArgumentNullException(nameof(sourceType));
            AlgorithmKind = kind;

            _handler.New();
            NormalHistoricalContext = repeatable
                ? new HistoricalContext(sourceType, kind)
                : null;
            LiteMode = liteMode;


            _lazyMemberHandler = MemberHandler.Lazy(_handler, _sourceType, liteMode);
            _correctnessContext = strictMode
                ? new CorrectnessContext(this, true)
                : null;

            if (initialValues is not null)
                SetValue(initialValues);
        }

        public Type SourceType => _sourceType;

        public bool IsStatic => false;

        public AlgorithmKind AlgorithmKind { get; }

        #region Instance

        public object Instance => _handler.GetInstance();

        #endregion

        #region Validation

        public bool StrictMode
        {
            get => VerifiableEntry.StrictMode;
            set => VerifiableEntry.StrictMode = value;
        }

        private CorrectnessContext _correctnessContext;

        public IValidationEntry VerifiableEntry => _correctnessContext ??= new CorrectnessContext(this, false);

        public VerifyResult Verify(bool withGlobalRules = false) => ((CorrectnessContext) VerifiableEntry).Verify(withGlobalRules);

        public void VerifyAndThrow(bool withGlobalRules = false) => Verify(withGlobalRules).Raise();

        #endregion

        #region SetValue

        public void SetValue(string memberName, object value, bool validationWithGlobalRules = false)
        {
            if (StrictMode)
                ((CorrectnessContext) VerifiableEntry).VerifyOne(memberName, value, validationWithGlobalRules).Raise();
            SetValueImpl(memberName, value);
        }

        public void SetValue<TObj>(Expression<Func<TObj, object>> expression, object value, bool validationWithGlobalRules = false)
        {
            if (expression is null)
                return;

            var name = PropertySelector.GetPropertyName(expression);

            if (StrictMode)
                ((CorrectnessContext) VerifiableEntry).VerifyOne(name, value, validationWithGlobalRules).Raise();
            SetValueImpl(name, value);
        }

        public void SetValue<TObj, TValue>(Expression<Func<TObj, TValue>> expression, TValue value, bool validationWithGlobalRules = false)
        {
            if (expression is null)
                return;

            var name = PropertySelector.GetPropertyName(expression);

            if (StrictMode)
                ((CorrectnessContext) VerifiableEntry).VerifyOne(name, value, validationWithGlobalRules).Raise();
            SetValueImpl(name, value);
        }

        public void SetValue(IDictionary<string, object> keyValueCollection, bool validationWithGlobalRules = false)
        {
            if (keyValueCollection is null)
                throw new ArgumentNullException(nameof(keyValueCollection));
            if (StrictMode)
                ((CorrectnessContext) VerifiableEntry).VerifyMany(keyValueCollection, validationWithGlobalRules).Raise();
            foreach (var keyValue in keyValueCollection)
                SetValueImpl(keyValue.Key, keyValue.Value);
        }

        private void SetValueImpl(string memberName, object value)
        {
            NormalHistoricalContext?.RegisterOperation(c => c[memberName] = value);
            _handler[memberName] = value;
        }

        #endregion

        #region GetValue

        public object GetValue(string memberName)
        {
            return _handler[memberName];
        }

        public TValue GetValue<TValue>(string memberName)
        {
            return _handler.Get<TValue>(memberName);
        }

        public object GetValue<TObj>(Expression<Func<TObj, object>> expression)
        {
            if (expression is null)
                throw new ArgumentNullException(nameof(expression));

            var name = PropertySelector.GetPropertyName(expression);

            return _handler[name];
        }

        public TValue GetValue<TObj, TValue>(Expression<Func<TObj, TValue>> expression)
        {
            if (expression is null)
                throw new ArgumentNullException(nameof(expression));

            var name = PropertySelector.GetPropertyName(expression);

            return _handler.Get<TValue>(name);
        }

        #endregion

        #region Index

        public object this[string memberName]
        {
            get => GetValue(memberName);
            set => SetValue(memberName, value);
        }

        #endregion

        public HistoricalContext ExposeHistoricalContext() => NormalHistoricalContext;

        public Lazy<MemberHandler> ExposeLazyMemberHandler() => _lazyMemberHandler;

        public IObjectVisitor Owner => this;

        public bool LiteMode { get; }

        #region Member

        public IEnumerable<string> GetMemberNames() => _lazyMemberHandler.Value.GetNames();

        public ObjectMember GetMember(string memberName) => _lazyMemberHandler.Value.GetMember(memberName);

        #endregion

        #region Contains

        public bool Contains(string memberName) => _lazyMemberHandler.Value.Contains(memberName);

        #endregion

        #region ValueAccessor

        public IPropertyValueAccessor ToValueAccessor()
        {
            return new ObjectPropertyValueAccessor(Instance, SourceType);
        }

        #endregion
    }
}