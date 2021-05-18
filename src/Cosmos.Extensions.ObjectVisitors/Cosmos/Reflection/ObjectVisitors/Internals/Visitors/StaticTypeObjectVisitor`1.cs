using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Cosmos.Reflection.ObjectVisitors.Core;
using Cosmos.Reflection.ObjectVisitors.Correctness;
using Cosmos.Reflection.ObjectVisitors.Internals.Members;
using Cosmos.Reflection.ObjectVisitors.Internals.PropertyNodes;
using Cosmos.Reflection.ObjectVisitors.Internals.Repeat;
using Cosmos.Reflection.ObjectVisitors.Metadata;
using Cosmos.Validation;

namespace Cosmos.Reflection.ObjectVisitors.Internals.Visitors
{
    internal class StaticTypeObjectVisitor<T> : IObjectVisitor<T>, ICoreVisitor<T>, IObjectGetter<T>, IObjectSetter<T>
    {
        private readonly ObjectOwn _objectOwnInfo;
        private readonly ObjectCallerBase<T> _handler;
        private readonly Lazy<MemberHandler> _lazyMemberHandler;

        private Dictionary<string, Lazy<IPropertyNodeVisitor>> LazyPropertyNodes { get; set; }

        public StaticTypeObjectVisitor(ObjectCallerBase<T> handler, AlgorithmKind kind, bool liteMode = false, bool strictMode = false)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            AlgorithmKind = kind;

            SourceType = typeof(T);
            LiteMode = liteMode;

            _objectOwnInfo = ObjectOwn.Of<T>();
            _lazyMemberHandler = MemberHandler.Lazy(_handler, SourceType, liteMode);
            _correctnessContext = strictMode
                ? new CorrectnessContext<T>(this, true)
                : null;

            LazyPropertyNodes = RootNode.New(_handler, kind, RpMode.NON_REPEATABLE, strictMode);
        }

        public Type SourceType { get; }

        public bool IsStatic => true;

        public AlgorithmKind AlgorithmKind { get; }

        #region Instance

        public T Instance => default;

        object IObjectVisitor.Instance => default;

        public int Signature => 0;

        public void SyncInstance(object instance) { }

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

        public void SetValue(string memberName, object value, AccessMode mode = AccessMode.Concise)
        {
            if (mode == AccessMode.Concise)
            {
                if (_objectOwnInfo.IsReadOnly)
                    return;
                if (StrictMode)
                    ((CorrectnessContext<T>) VerifiableEntry).VerifyOne(memberName, value, false).Raise();
                SetValueImpl(memberName, value);
            }
            else if (PathNavParser.TryParse(memberName, out var segments)
                  && LazyPropertyNodes.TryGetValue(segments[0], out var node))
            {
                if (node is null)
                    throw new ArgumentNullException(nameof(memberName), $"${segments[0]} is not a structured object.");

                node.Value.SetValue(segments, value, 1);
            }
        }

        public void SetValue(string memberName, object value, string globalVerifyProviderName, AccessMode mode = AccessMode.Concise)
        {
            if (mode == AccessMode.Concise)
            {
                if (_objectOwnInfo.IsReadOnly)
                    return;
                if (StrictMode)
                    ((CorrectnessContext<T>) VerifiableEntry).VerifyOne(memberName, value, true, globalVerifyProviderName).Raise();
                SetValueImpl(memberName, value);
            }
            else if (PathNavParser.TryParse(memberName, out var segments)
                  && LazyPropertyNodes.TryGetValue(segments[0], out var node))
            {
                if (node is null)
                    throw new ArgumentNullException(nameof(memberName), $"${segments[0]} is not a structured object.");

                node.Value.SetValue(segments, value, 1, globalVerifyProviderName);
            }
        }

        public void SetValue(Expression<Func<T, object>> expression, object value)
        {
            if (expression is null)
                return;

            if (_objectOwnInfo.IsReadOnly)
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

            if (_objectOwnInfo.IsReadOnly)
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

            if (_objectOwnInfo.IsReadOnly)
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

            if (_objectOwnInfo.IsReadOnly)
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
            if (_objectOwnInfo.IsReadOnly)
                return;
            if (StrictMode)
                ((CorrectnessContext<T>) VerifiableEntry).VerifyMany(keyValueCollection, false).Raise();
            foreach (var keyValue in keyValueCollection)
                SetValueImpl(keyValue.Key, keyValue.Value);
        }

        public void SetValue(IDictionary<string, object> keyValueCollection, string globalVerifyProviderName)
        {
            if (keyValueCollection is null)
                throw new ArgumentNullException(nameof(keyValueCollection));
            if (_objectOwnInfo.IsReadOnly)
                return;
            if (StrictMode)
                ((CorrectnessContext<T>) VerifiableEntry).VerifyMany(keyValueCollection, true, globalVerifyProviderName).Raise();
            foreach (var keyValue in keyValueCollection)
                SetValueImpl(keyValue.Key, keyValue.Value);
        }

        private void SetValueImpl(string memberName, object value)
        {
            _handler[memberName] = value;
        }

        void IObjectVisitor.SetValue<TObj>(Expression<Func<TObj, object>> expression, object value)
        {
            if (expression is null)
                return;

            if (_objectOwnInfo.IsReadOnly)
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

            if (_objectOwnInfo.IsReadOnly)
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

            if (_objectOwnInfo.IsReadOnly)
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

            if (_objectOwnInfo.IsReadOnly)
                return;
            
            var name = PropertySelector.GetPropertyName(expression);

            if (StrictMode)
                ((CorrectnessContext<T>) VerifiableEntry).VerifyOne(name, value, true, globalVerifyProviderName).Raise();
          
            SetValueImpl(name, value);
        }

        void IObjectSetter<T>.SetValue<TObj>(Expression<Func<TObj, object>> expression, object value) => ((IObjectVisitor) this).SetValue(expression, value);

        void IObjectSetter<T>.SetValue<TObj>(Expression<Func<TObj, object>> expression, object value, string globalVerifyProviderName) => ((IObjectVisitor) this).SetValue(expression, value, globalVerifyProviderName);

        void IObjectSetter<T>.SetValue<TObj, TValue>(Expression<Func<TObj, TValue>> expression, TValue value) => ((IObjectVisitor) this).SetValue(expression, value);

        void IObjectSetter<T>.SetValue<TObj, TValue>(Expression<Func<TObj, TValue>> expression, TValue value, string globalVerifyProviderName) => ((IObjectVisitor) this).SetValue(expression, value, globalVerifyProviderName);

        #endregion

        #region GetValue

        public object GetValue(string memberName, AccessMode mode = AccessMode.Concise)
        {
            if (mode == AccessMode.Concise)
                return _handler[memberName];
            if (PathNavParser.TryParse(memberName, out var segments))
                return LazyPropertyNodes.TryGetValue(segments[0], out var node)
                    ? node.Value.GetValue(segments, 1)
                    : default;
            return default;
        }

        public object GetValue(Expression<Func<T, object>> expression)
        {
            if (expression is null)
                throw new ArgumentNullException(nameof(expression));

            var name = PropertySelector.GetPropertyName(expression);

            return _handler[name];
        }

        public TValue GetValue<TValue>(string memberName, AccessMode mode = AccessMode.Concise)
        {
            if (mode == AccessMode.Concise)
                return _handler.Get<TValue>(memberName);
            if (PathNavParser.TryParse(memberName, out var segments))
                return LazyPropertyNodes.TryGetValue(segments[0], out var node)
                    ? (TValue) node.Value.GetValue(segments, 1)
                    : default;
            return default;
        }

        public TValue GetValue<TValue>(Expression<Func<T, TValue>> expression)
        {
            if (expression is null)
                throw new ArgumentNullException(nameof(expression));

            var name = PropertySelector.GetPropertyName(expression);

            return _handler.Get<TValue>(name);
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

        object IObjectGetter<T>.GetValue<TObj>(Expression<Func<TObj, object>> expression) => ((IObjectVisitor) this).GetValue(expression);

        TValue IObjectGetter<T>.GetValue<TObj, TValue>(Expression<Func<TObj, TValue>> expression) => ((IObjectVisitor) this).GetValue(expression);

        #endregion

        #region Index

        public object this[string memberName]
        {
            get => GetValue(memberName);
            set => SetValue(memberName, value);
        }

        public object this[string memberName, AccessMode mode]
        {
            get => GetValue(memberName, mode);
            set => SetValue(memberName, value, mode);
        }

        #endregion


        public HistoricalContext<T> ExposeHistoricalContext() => default;

        public Lazy<MemberHandler> ExposeLazyMemberHandler() => _lazyMemberHandler;

        public ObjectCallerBase<T> ExposeObjectCaller() => _handler;

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

        #region PropertyNodes

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