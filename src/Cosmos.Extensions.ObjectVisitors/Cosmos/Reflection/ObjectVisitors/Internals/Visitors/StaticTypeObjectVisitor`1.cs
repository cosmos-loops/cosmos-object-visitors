using System;
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
    internal class StaticTypeObjectVisitor<T> : IObjectVisitor<T>, ICoreVisitor<T>, IObjectGetter<T>, IObjectSetter<T>
    {
        private readonly ObjectCallerBase _handler;

        private readonly Lazy<MemberHandler> _lazyMemberHandler;

        public StaticTypeObjectVisitor(ObjectCallerBase<T> handler, AlgorithmKind kind, bool liteMode = false, bool strictMode = false)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            AlgorithmKind = kind;

            SourceType = typeof(T);
            LiteMode = liteMode;

            _lazyMemberHandler = MemberHandler.Lazy(_handler, SourceType, liteMode);
            _correctnessContext = strictMode
                ? new CorrectnessContext<T>(this, true)
                : null;
        }

        public Type SourceType { get; }

        public bool IsStatic => true;

        public AlgorithmKind AlgorithmKind { get; }

        #region Instance

        public T Instance => default;

        object IObjectVisitor.Instance => default;

        #endregion

        #region Validation

        public bool StrictMode
        {
            get => VerifiableEntry.StrictMode;
            set => VerifiableEntry.StrictMode = value;
        }

        private CorrectnessContext<T> _correctnessContext;

        public IValidationEntry<T> VerifiableEntry => _correctnessContext ??= new CorrectnessContext<T>(this, false);

        IValidationEntry IObjectVisitor.VerifiableEntry => new LinkToCorrectnessContext<T>(_correctnessContext ??= new CorrectnessContext<T>(this, false));

        public VerifyResult Verify(bool withGlobalRules = false) => ((CorrectnessContext<T>) VerifiableEntry).Verify(withGlobalRules);

        public void VerifyAndThrow(bool withGlobalRules = false) => Verify(withGlobalRules).Raise();

        #endregion

        #region SetValue

        public void SetValue(string memberName, object value, bool validationWithGlobalRules = false)
        {
            if (StrictMode)
                ((CorrectnessContext<T>) VerifiableEntry).VerifyOne(memberName, value, validationWithGlobalRules).Raise();
            SetValueImpl(memberName, value);
        }

        void IObjectVisitor.SetValue<TObj>(Expression<Func<TObj, object>> expression, object value, bool validationWithGlobalRules)
        {
            if (expression is null)
                return;

            var name = PropertySelector.GetPropertyName(expression);

            if (StrictMode)
                ((CorrectnessContext<T>) VerifiableEntry).VerifyOne(name, value, validationWithGlobalRules).Raise();
            SetValueImpl(name, value);
        }

        void IObjectVisitor.SetValue<TObj, TValue>(Expression<Func<TObj, TValue>> expression, TValue value, bool validationWithGlobalRules)
        {
            if (expression is null)
                return;

            var name = PropertySelector.GetPropertyName(expression);

            if (StrictMode)
                ((CorrectnessContext<T>) VerifiableEntry).VerifyOne(name, value, validationWithGlobalRules).Raise();
            SetValueImpl(name, value);
        }

        void IObjectSetter<T>.SetValue<TObj>(Expression<Func<TObj, object>> expression, object value, bool validationWithGlobalRules)
            => ((IObjectVisitor) this).SetValue(expression, value, validationWithGlobalRules);

        void IObjectSetter<T>.SetValue<TObj, TValue>(Expression<Func<TObj, TValue>> expression, TValue value, bool validationWithGlobalRules)
            => ((IObjectVisitor) this).SetValue(expression, value, validationWithGlobalRules);

        public void SetValue(Expression<Func<T, object>> expression, object value, bool validationWithGlobalRules = false)
        {
            if (expression is null)
                return;

            var name = PropertySelector.GetPropertyName(expression);

            if (StrictMode)
                ((CorrectnessContext<T>) VerifiableEntry).VerifyOne(name, value, validationWithGlobalRules).Raise();
            SetValueImpl(name, value);
        }

        public void SetValue<TValue>(Expression<Func<T, TValue>> expression, TValue value, bool validationWithGlobalRules = false)
        {
            if (expression is null)
                return;

            var name = PropertySelector.GetPropertyName(expression);

            if (StrictMode)
                ((CorrectnessContext<T>) VerifiableEntry).VerifyOne(name, value, validationWithGlobalRules).Raise();
            SetValueImpl(name, value);
        }

        public void SetValue(IDictionary<string, object> keyValueCollection, bool validationWithGlobalRules = false)
        {
            if (keyValueCollection is null)
                throw new ArgumentNullException(nameof(keyValueCollection));
            if (StrictMode)
                ((CorrectnessContext<T>) VerifiableEntry).VerifyMany(keyValueCollection, validationWithGlobalRules).Raise();
            foreach (var keyValue in keyValueCollection)
                SetValueImpl(keyValue.Key, keyValue.Value);
        }

        private void SetValueImpl(string memberName, object value)
        {
            _handler[memberName] = value;
        }

        #endregion

        #region GetValue

        public object GetValue(string memberName)
        {
            return _handler[memberName];
        }

        public object GetValue(Expression<Func<T, object>> expression)
        {
            if (expression is null)
                throw new ArgumentNullException(nameof(expression));

            var name = PropertySelector.GetPropertyName(expression);

            return _handler[name];
        }

        public TValue GetValue<TValue>(string memberName)
        {
            return _handler.Get<TValue>(memberName);
        }

        object IObjectVisitor.GetValue<TObj>(Expression<Func<TObj, object>> expression)
        {
            if (expression is null)
                throw new ArgumentNullException(nameof(expression));

            var name = PropertySelector.GetPropertyName(expression);

            return _handler[name];
        }

        TValue IObjectVisitor.GetValue<TObj, TValue>(Expression<Func<TObj, TValue>> expression)
        {
            if (expression is null)
                throw new ArgumentNullException(nameof(expression));

            var name = PropertySelector.GetPropertyName(expression);

            return _handler.Get<TValue>(name);
        }

        object IObjectGetter<T>.GetValue<TObj>(Expression<Func<TObj, object>> expression)
            => ((IObjectVisitor) this).GetValue(expression);

        TValue IObjectGetter<T>.GetValue<TObj, TValue>(Expression<Func<TObj, TValue>> expression)
            => ((IObjectVisitor) this).GetValue(expression);

        public TValue GetValue<TValue>(Expression<Func<T, TValue>> expression)
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


        public HistoricalContext<T> ExposeHistoricalContext() => default;

        public Lazy<MemberHandler> ExposeLazyMemberHandler() => _lazyMemberHandler;

        public IObjectVisitor<T> Owner => this;

        public bool LiteMode { get; }

        #region Member

        public IEnumerable<string> GetMemberNames() => _lazyMemberHandler.Value.GetNames();

        public ObjectMember GetMember(string memberName) => _lazyMemberHandler.Value.GetMember(memberName);

        public ObjectMember GetMember<TValue>(Expression<Func<T, TValue>> expression)
        {
            if (expression is null)
                throw new ArgumentNullException(nameof(expression));

            var name = PropertySelector.GetPropertyName(expression);

            return _lazyMemberHandler.Value.GetMember(name);
        }

        #endregion

        #region Contains

        public bool Contains(string memberName) => _lazyMemberHandler.Value.Contains(memberName);

        #endregion

        #region ValueAccessor

        public IPropertyValueAccessor ToValueAccessor()
        {
            return new StaticPropertyValueAccessor<T>();
        }

        #endregion
    }
}