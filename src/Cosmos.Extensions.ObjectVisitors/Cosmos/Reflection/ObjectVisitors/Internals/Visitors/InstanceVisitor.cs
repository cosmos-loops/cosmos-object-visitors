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
    internal class InstanceVisitor : IObjectVisitor, ICoreVisitor, IObjectGetter, IObjectSetter
    {
        private readonly ObjectCallerBase _handler;
        private readonly Lazy<MemberHandler> _lazyMemberHandler;

        private HistoricalContext NormalHistoricalContext { get; set; }
        private Dictionary<string, Lazy<IPropertyNodeVisitor>> LazyPropertyNodes { get; set; }

        public InstanceVisitor(ObjectCallerBase handler, Type sourceType, AlgorithmKind kind, bool repeatable,
            bool liteMode = false, bool strictMode = false)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            AlgorithmKind = kind;

            SourceType = sourceType ?? throw new ArgumentNullException(nameof(sourceType));
            NormalHistoricalContext = repeatable
                ? new HistoricalContext(sourceType, kind)
                : null;
            LiteMode = liteMode;

            _lazyMemberHandler = MemberHandler.Lazy(_handler, SourceType, liteMode);
            _correctnessContext = strictMode
                ? new CorrectnessContext(this, true)
                : null;

            LazyPropertyNodes = RootNode.New(_handler, kind, repeatable, strictMode);
        }

        public Type SourceType { get; }

        public bool IsStatic => false;

        public AlgorithmKind AlgorithmKind { get; }

        #region Instance

        public object Instance => _handler.GetObjInstance();

        public int Signature => Instance?.GetHashCode() ?? 0;

        public void SyncInstance(object instance)
        {
            _handler.SetObjInstance(instance);
        }

        #endregion

        #region Validation

        public bool StrictMode
        {
            get => VerifiableEntry.StrictMode;
            set => VerifiableEntry.StrictMode = value;
        }

        private CorrectnessContext _correctnessContext;

        public IValidationEntry VerifiableEntry => _correctnessContext ??= new CorrectnessContext(this, false);

        public VerifyResult Verify() => ((CorrectnessContext) VerifiableEntry).Verify(false);

        public VerifyResult Verify(string globalVerifyProviderName) => ((CorrectnessContext) VerifiableEntry).Verify(true, globalVerifyProviderName);

        public void VerifyAndThrow() => Verify().Raise();

        public void VerifyAndThrow(string globalVerifyProviderName) => Verify(globalVerifyProviderName).Raise();

        #endregion

        #region SetValue

        public void SetValue(string memberName, object value, AccessMode mode = AccessMode.Concise)
        {
            if (mode == AccessMode.Concise)
            {
                if (StrictMode)
                    ((CorrectnessContext) VerifiableEntry).VerifyOne(memberName, value, false).Raise();
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
                if (StrictMode)
                    ((CorrectnessContext) VerifiableEntry).VerifyOne(memberName, value, true, globalVerifyProviderName).Raise();
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

        public void SetValue<TObj>(Expression<Func<TObj, object>> expression, object value)
        {
            if (expression is null)
                return;

            var name = PropertySelector.GetPropertyName(expression);

            if (StrictMode)
                ((CorrectnessContext) VerifiableEntry).VerifyOne(name, value, false).Raise();
            SetValueImpl(name, value);
        }

        public void SetValue<TObj>(Expression<Func<TObj, object>> expression, object value, string globalVerifyProviderName)
        {
            if (expression is null)
                return;

            var name = PropertySelector.GetPropertyName(expression);

            if (StrictMode)
                ((CorrectnessContext) VerifiableEntry).VerifyOne(name, value, true, globalVerifyProviderName).Raise();
            SetValueImpl(name, value);
        }

        public void SetValue<TObj, TValue>(Expression<Func<TObj, TValue>> expression, TValue value)
        {
            if (expression is null)
                return;

            var name = PropertySelector.GetPropertyName(expression);

            if (StrictMode)
                ((CorrectnessContext) VerifiableEntry).VerifyOne(name, value, false).Raise();
            SetValueImpl(name, value);
        }

        public void SetValue<TObj, TValue>(Expression<Func<TObj, TValue>> expression, TValue value, string globalVerifyProviderName)
        {
            if (expression is null)
                return;

            var name = PropertySelector.GetPropertyName(expression);

            if (StrictMode)
                ((CorrectnessContext) VerifiableEntry).VerifyOne(name, value, true, globalVerifyProviderName).Raise();
            SetValueImpl(name, value);
        }

        public void SetValue(IDictionary<string, object> keyValueCollection)
        {
            if (keyValueCollection is null)
                throw new ArgumentNullException(nameof(keyValueCollection));
            if (StrictMode)
                ((CorrectnessContext) VerifiableEntry).VerifyMany(keyValueCollection, false).Raise();
            foreach (var keyValue in keyValueCollection)
                SetValueImpl(keyValue.Key, keyValue.Value);
        }

        public void SetValue(IDictionary<string, object> keyValueCollection, string globalVerifyProviderName)
        {
            if (keyValueCollection is null)
                throw new ArgumentNullException(nameof(keyValueCollection));
            if (StrictMode)
                ((CorrectnessContext) VerifiableEntry).VerifyMany(keyValueCollection, true, globalVerifyProviderName).Raise();
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

        public object this[string memberName, AccessMode mode]
        {
            get => GetValue(memberName, mode);
            set => SetValue(memberName, value, mode);
        }

        #endregion

        public HistoricalContext ExposeHistoricalContext() => NormalHistoricalContext;

        public Lazy<MemberHandler> ExposeLazyMemberHandler() => _lazyMemberHandler;

        public ObjectCallerBase ExposeObjectCaller() => _handler;

        public IObjectVisitor Owner => this;

        public bool LiteMode { get; }

        #region Member

        public IEnumerable<string> GetMemberNames() => _lazyMemberHandler.Value.GetNames();

        public ObjectMember GetMember(string memberName) => _lazyMemberHandler.Value.GetMember(memberName);

        #endregion

        #region PropertyNodes

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