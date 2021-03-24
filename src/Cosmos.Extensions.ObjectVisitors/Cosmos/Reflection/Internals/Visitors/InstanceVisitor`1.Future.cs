using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Cosmos.Reflection.Core;
using Cosmos.Reflection.Correctness;
using Cosmos.Reflection.Internals.Members;
using Cosmos.Reflection.Internals.Repeat;
using Cosmos.Reflection.Metadata;
using Cosmos.Validation;

namespace Cosmos.Reflection.Internals.Visitors
{
    internal class FutureInstanceVisitor<T> : IObjectVisitor<T>, ICoreVisitor<T>, IObjectGetter<T>, IObjectSetter<T>
    {
        private readonly ObjectCallerBase<T> _handler;
        private readonly Type _sourceType;

        private readonly Lazy<MemberHandler> _lazyMemberHandler;

        protected HistoricalContext<T> GenericHistoricalContext { get; set; }

        public FutureInstanceVisitor(ObjectCallerBase<T> handler, AlgorithmKind kind, bool repeatable,
            IDictionary<string, object> initialValues = null, bool liteMode = false, bool strictMode = false)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            _sourceType = typeof(T);
            AlgorithmKind = kind;

            _handler.New();

            GenericHistoricalContext = repeatable
                ? new HistoricalContext<T>(kind)
                : null;
            LiteMode = liteMode;

            _lazyMemberHandler = MemberHandler.Lazy(() => new MemberHandler(_handler, _sourceType), liteMode);
            _correctnessContext = strictMode
                ? new CorrectnessContext<T>(this, true)
                : null;

            if (initialValues != null)
                SetValue(initialValues);
        }

        public Type SourceType => _sourceType;

        public bool IsStatic => false;

        public AlgorithmKind AlgorithmKind { get; }

        #region Instance

        public T Instance => _handler.GetInstance();

        object IObjectVisitor.Instance => _handler.GetInstance();

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

        public VerifyResult Verify() => ((CorrectnessContext<T>) VerifiableEntry).Verify();

        public void VerifyAndThrow() => Verify().Raise();

        #endregion

        #region SetValue

        public void SetValue(string memberName, object value)
        {
            if (StrictMode)
                ((CorrectnessContext<T>) VerifiableEntry).VerifyOne(memberName, value).Raise();
            SetValueImpl(memberName, value);
        }

        void IObjectVisitor.SetValue<TObj>(Expression<Func<TObj, object>> expression, object value)
        {
            if (expression is null)
                return;

            var name = PropertySelector.GetPropertyName(expression);

            if (StrictMode)
                ((CorrectnessContext<T>) VerifiableEntry).VerifyOne(name, value).Raise();
            SetValueImpl(name, value);
        }

        void IObjectVisitor.SetValue<TObj, TValue>(Expression<Func<TObj, TValue>> expression, TValue value)
        {
            if (expression is null)
                return;

            var name = PropertySelector.GetPropertyName(expression);

            if (StrictMode)
                ((CorrectnessContext<T>) VerifiableEntry).VerifyOne(name, value).Raise();
            SetValueImpl(name, value);
        }

        void IObjectSetter<T>.SetValue<TObj>(Expression<Func<TObj, object>> expression, object value)
            => ((IObjectVisitor) this).SetValue(expression, value);

        void IObjectSetter<T>.SetValue<TObj, TValue>(Expression<Func<TObj, TValue>> expression, TValue value)
            => ((IObjectVisitor) this).SetValue(expression, value);

        public void SetValue(Expression<Func<T, object>> expression, object value)
        {
            if (expression is null)
                return;

            var name = PropertySelector.GetPropertyName(expression);

            if (StrictMode)
                ((CorrectnessContext<T>) VerifiableEntry).VerifyOne(name, value).Raise();
            SetValueImpl(name, value);
        }

        public void SetValue<TValue>(Expression<Func<T, TValue>> expression, TValue value)
        {
            if (expression is null)
                return;

            var name = PropertySelector.GetPropertyName(expression);

            if (StrictMode)
                ((CorrectnessContext<T>) VerifiableEntry).VerifyOne(name, value).Raise();
            SetValueImpl(name, value);
        }

        public void SetValue(IDictionary<string, object> keyValueCollection)
        {
            if (keyValueCollection is null)
                throw new ArgumentNullException(nameof(keyValueCollection));
            if (StrictMode)
                ((CorrectnessContext<T>) VerifiableEntry).VerifyMany(keyValueCollection).Raise();
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
    }
}