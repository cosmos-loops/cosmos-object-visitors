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
    internal class InstanceVisitor<T> : IObjectVisitor<T>, ICoreVisitor<T>, IObjectGetter<T>, IObjectSetter<T>
    {
        private readonly ObjectCallerBase<T> _handler;
        private readonly T _instance;

        private readonly Lazy<MemberHandler> _lazyMemberHandler;

        private HistoricalContext<T> GenericHistoricalContext { get; set; }

        public InstanceVisitor(ObjectCallerBase<T> handler, T instance, AlgorithmKind kind, bool repeatable,
            bool liteMode = false, bool strictMode = false)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            _instance = instance;
            AlgorithmKind = kind;

            _handler.SetInstance(_instance);

            SourceType = typeof(T);
            GenericHistoricalContext = repeatable
                ? new HistoricalContext<T>(kind)
                : null;
            LiteMode = liteMode;

            _lazyMemberHandler = MemberHandler.Lazy(_handler, SourceType, liteMode);
            _correctnessContext = strictMode
                ? new CorrectnessContext<T>(this, true)
                : null;
        }

        public Type SourceType { get; }

        public bool IsStatic => false;

        public AlgorithmKind AlgorithmKind { get; }

        #region Instance

        public T Instance => _instance;

        object IObjectVisitor.Instance => _instance;

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

        public VerifyResult Verify() => ((CorrectnessContext<T>) VerifiableEntry).Verify(false);

        public VerifyResult Verify(string globalVerifyProviderName) => ((CorrectnessContext<T>) VerifiableEntry).Verify(true, globalVerifyProviderName);

        public void VerifyAndThrow() => Verify().Raise();

        public void VerifyAndThrow(string globalVerifyProviderName) => Verify(globalVerifyProviderName).Raise();

        #endregion

        #region SetValue

        public void SetValue(string memberName, object value)
        {
            if (StrictMode)
                ((CorrectnessContext<T>) VerifiableEntry).VerifyOne(memberName, value, false).Raise();
            SetValueImpl(memberName, value);
        }

        public void SetValue(string memberName, object value, string globalVerifyProviderName)
        {
            if (StrictMode)
                ((CorrectnessContext<T>) VerifiableEntry).VerifyOne(memberName, value, true, globalVerifyProviderName).Raise();
            SetValueImpl(memberName, value);
        }

        void IObjectVisitor.SetValue<TObj>(Expression<Func<TObj, object>> expression, object value)
        {
            if (expression is null)
                return;

            var name = PropertySelector.GetPropertyName(expression);

            if (StrictMode)
                ((CorrectnessContext<T>) VerifiableEntry).VerifyOne(name, value, false).Raise();
            SetValueImpl(name, value);
        }

        void IObjectVisitor.SetValue<TObj>(Expression<Func<TObj, object>> expression, object value, string globalVerifyProviderName)
        {
            if (expression is null)
                return;

            var name = PropertySelector.GetPropertyName(expression);

            if (StrictMode)
                ((CorrectnessContext<T>) VerifiableEntry).VerifyOne(name, value, true, globalVerifyProviderName).Raise();
            SetValueImpl(name, value);
        }

        void IObjectVisitor.SetValue<TObj, TValue>(Expression<Func<TObj, TValue>> expression, TValue value)
        {
            if (expression is null)
                return;

            var name = PropertySelector.GetPropertyName(expression);

            if (StrictMode)
                ((CorrectnessContext<T>) VerifiableEntry).VerifyOne(name, value, false).Raise();
            SetValueImpl(name, value);
        }

        void IObjectVisitor.SetValue<TObj, TValue>(Expression<Func<TObj, TValue>> expression, TValue value, string globalVerifyProviderName)
        {
            if (expression is null)
                return;

            var name = PropertySelector.GetPropertyName(expression);

            if (StrictMode)
                ((CorrectnessContext<T>) VerifiableEntry).VerifyOne(name, value, true, globalVerifyProviderName).Raise();
            SetValueImpl(name, value);
        }

        void IObjectSetter<T>.SetValue<TObj>(Expression<Func<TObj, object>> expression, object value)
            => ((IObjectVisitor) this).SetValue(expression, value);

        void IObjectSetter<T>.SetValue<TObj>(Expression<Func<TObj, object>> expression, object value, string globalVerifyProviderName)
            => ((IObjectVisitor) this).SetValue(expression, value, globalVerifyProviderName);

        void IObjectSetter<T>.SetValue<TObj, TValue>(Expression<Func<TObj, TValue>> expression, TValue value)
            => ((IObjectVisitor) this).SetValue(expression, value);

        void IObjectSetter<T>.SetValue<TObj, TValue>(Expression<Func<TObj, TValue>> expression, TValue value, string globalVerifyProviderName)
            => ((IObjectVisitor) this).SetValue(expression, value, globalVerifyProviderName);

        public void SetValue(Expression<Func<T, object>> expression, object value)
        {
            if (expression is null)
                return;

            var name = PropertySelector.GetPropertyName(expression);

            if (StrictMode)
                ((CorrectnessContext<T>) VerifiableEntry).VerifyOne(name, value, false).Raise();
            SetValueImpl(name, value);
        }

        public void SetValue(Expression<Func<T, object>> expression, object value, string globalVerifyProviderName)
        {
            if (expression is null)
                return;

            var name = PropertySelector.GetPropertyName(expression);

            if (StrictMode)
                ((CorrectnessContext<T>) VerifiableEntry).VerifyOne(name, value, true, globalVerifyProviderName).Raise();
            SetValueImpl(name, value);
        }

        public void SetValue<TValue>(Expression<Func<T, TValue>> expression, TValue value)
        {
            if (expression is null)
                return;

            var name = PropertySelector.GetPropertyName(expression);

            if (StrictMode)
                ((CorrectnessContext<T>) VerifiableEntry).VerifyOne(name, value, false).Raise();
            SetValueImpl(name, value);
        }

        public void SetValue<TValue>(Expression<Func<T, TValue>> expression, TValue value, string globalVerifyProviderName)
        {
            if (expression is null)
                return;

            var name = PropertySelector.GetPropertyName(expression);

            if (StrictMode)
                ((CorrectnessContext<T>) VerifiableEntry).VerifyOne(name, value, true, globalVerifyProviderName).Raise();
            SetValueImpl(name, value);
        }

        public void SetValue(IDictionary<string, object> keyValueCollection)
        {
            if (keyValueCollection is null)
                throw new ArgumentNullException(nameof(keyValueCollection));
            if (StrictMode)
                ((CorrectnessContext<T>) VerifiableEntry).VerifyMany(keyValueCollection, false).Raise();
            foreach (var keyValue in keyValueCollection)
                SetValueImpl(keyValue.Key, keyValue.Value);
        }

        public void SetValue(IDictionary<string, object> keyValueCollection, string globalVerifyProviderName)
        {
            if (keyValueCollection is null)
                throw new ArgumentNullException(nameof(keyValueCollection));
            if (StrictMode)
                ((CorrectnessContext<T>) VerifiableEntry).VerifyMany(keyValueCollection, true, globalVerifyProviderName).Raise();
            foreach (var keyValue in keyValueCollection)
                SetValueImpl(keyValue.Key, keyValue.Value);
        }

        private void SetValueImpl(string memberName, object value)
        {
            GenericHistoricalContext?.RegisterOperation(c => c[memberName] = value);
            _handler[memberName] = value;
        }

        #endregion

        #region GetValue

        public object GetValue(string memberName)
        {
            return _handler[memberName];
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

        public HistoricalContext<T> ExposeHistoricalContext() => GenericHistoricalContext;

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
            return new ObjectPropertyValueAccessor<T>(Instance);
        }

        #endregion
    }
}