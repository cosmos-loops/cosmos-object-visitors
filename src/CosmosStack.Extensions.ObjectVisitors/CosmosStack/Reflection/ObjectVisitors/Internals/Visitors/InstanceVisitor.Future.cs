using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CosmosStack.Reflection.ObjectVisitors.Core;
using CosmosStack.Reflection.ObjectVisitors.Correctness;
using CosmosStack.Reflection.ObjectVisitors.Internals.Guards;
using CosmosStack.Reflection.ObjectVisitors.Internals.Members;
using CosmosStack.Reflection.ObjectVisitors.Internals.PropertyNodes;
using CosmosStack.Reflection.ObjectVisitors.Internals.Repeat;
using CosmosStack.Reflection.ObjectVisitors.Metadata;
using CosmosStack.Validation;

namespace CosmosStack.Reflection.ObjectVisitors.Internals.Visitors
{
    internal class FutureInstanceVisitor : IObjectVisitor, ICoreVisitor, IObjectGetter, IObjectSetter
    {
        private readonly ObjectOwn _objectOwnInfo;
        private readonly ObjectCallerBase _handler;
        private readonly Lazy<MemberHandler> _lazyMemberHandler;
        private readonly ObjectVisitorOptions _options;

        private HistoricalContext NormalHistoricalContext { get; set; }

        private Dictionary<string, Lazy<IPropertyNodeVisitor>> LazyPropertyNodes { get; set; }

        public FutureInstanceVisitor(ObjectCallerBase handler, Type sourceType, ObjectVisitorOptions options, IDictionary<string, object> initialValues = null)
        {
            _options = options?.Clone() ?? ObjectVisitorOptions.Default;
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            SourceType = sourceType ?? throw new ArgumentNullException(nameof(sourceType));

            _handler.New();
            NormalHistoricalContext = _options.Repeatable
                ? new HistoricalContext(sourceType, _options.AlgorithmKind)
                : null;

            _objectOwnInfo = ObjectOwn.Of(sourceType);
            _lazyMemberHandler = MemberHandler.Lazy(_handler, SourceType, _options.LiteMode);
            _correctnessContext = _options.StrictMode
                ? new CorrectnessContext(this, true)
                : null;

            if (initialValues is not null)
                SetValue(initialValues);

            LazyPropertyNodes = RootNode.New(_handler, _options);
        }

        public Type SourceType { get; }

        public bool IsStatic => false;

        public AlgorithmKind AlgorithmKind => _options.AlgorithmKind;

        #region Instance

        public object Instance => _handler.GetObjInstance();

        public int Signature => Instance?.GetHashCode() ?? 0;

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
                if (AccessGuard.ReadOnly(_objectOwnInfo, _options))
                    return;
                if (StrictMode)
                    ((CorrectnessContext) VerifiableEntry).VerifyOne(memberName, value, false).Raise();
                SetValueImpl(memberName, value);
            }
            else if (PathNavParser.TryParse(memberName, out var segments)
                  && LazyPropertyNodes.TryGetValue(segments[0], out var node))
            {
                if (node is null)
                    throw new ArgumentNullException(nameof(memberName), $"${segments[0]} is not a structured object.");

                MoveToNextNode(node).SetValue(segments, value, 1);
            }
        }

        public void SetValue(string memberName, object value, string globalVerifyProviderName, AccessMode mode = AccessMode.Concise)
        {
            if (mode == AccessMode.Concise)
            {
                if (AccessGuard.ReadOnly(_objectOwnInfo, _options))
                    return;
                if (StrictMode)
                    ((CorrectnessContext) VerifiableEntry).VerifyOne(memberName, value, true, globalVerifyProviderName).Raise();
                SetValueImpl(memberName, value);
            }
            else if (PathNavParser.TryParse(memberName, out var segments)
                  && LazyPropertyNodes.TryGetValue(segments[0], out var node))
            {
                if (node is null)
                    throw new ArgumentNullException(nameof(memberName), $"${segments[0]} is not a structured object.");

                MoveToNextNode(node).SetValue(segments, value, 1, globalVerifyProviderName);
            }
        }

        public void SetValue<TObj>(Expression<Func<TObj, object>> expression, object value)
        {
            if (expression is null)
                return;

            if (AccessGuard.ReadOnly(_objectOwnInfo, _options))
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

            if (AccessGuard.ReadOnly(_objectOwnInfo, _options))
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

            if (AccessGuard.ReadOnly(_objectOwnInfo, _options))
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

            if (AccessGuard.ReadOnly(_objectOwnInfo, _options))
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
            if (AccessGuard.ReadOnly(_objectOwnInfo, _options))
                return;
            if (StrictMode)
                ((CorrectnessContext) VerifiableEntry).VerifyMany(keyValueCollection, false).Raise();
            foreach (var keyValue in keyValueCollection)
                SetValueImpl(keyValue.Key, keyValue.Value);
        }

        public void SetValue(IDictionary<string, object> keyValueCollection, string globalVerifyProviderName)
        {
            if (keyValueCollection is null)
                throw new ArgumentNullException(nameof(keyValueCollection));
            if (AccessGuard.ReadOnly(_objectOwnInfo, _options))
                return;
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
                    ? MoveToNextNode(node).GetValue(segments, 1)
                    : default;
            return default;
        }

        public TValue GetValue<TValue>(string memberName, AccessMode mode = AccessMode.Concise)
        {
            if (mode == AccessMode.Concise)
                return _handler.Get<TValue>(memberName);
            if (PathNavParser.TryParse(memberName, out var segments))
                return LazyPropertyNodes.TryGetValue(segments[0], out var node)
                    ? (TValue) MoveToNextNode(node).GetValue(segments, 1)
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

        public bool LiteMode => _options.LiteMode;

        #region Member

        public IEnumerable<string> GetMemberNames() => _lazyMemberHandler.Value.GetNames();

        public ObjectMember GetMember(string memberName) => _lazyMemberHandler.Value.GetMember(memberName);

        #endregion

        #region Nodes

        private IPropertyNodeVisitor MoveToNextNode(Lazy<IPropertyNodeVisitor> lazyNode)
        {
            var node = lazyNode.Value;
            if (node.Signature != (_handler[node.Name]?.GetHashCode() ?? 0))
                node.Sync(_handler[node.Name]);
            return node;
        }
        
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