using System;
using System.Collections.Generic;
using System.Linq;
using CosmosStack.Collections;
using CosmosStack.Reflection.ObjectVisitors.Core;
using CosmosStack.Reflection.ObjectVisitors.Correctness;
using CosmosStack.Reflection.ObjectVisitors.Internals.Members;
using CosmosStack.Reflection.ObjectVisitors.Internals.Repeat;
using CosmosStack.Reflection.ObjectVisitors.Internals.Visitors;
using CosmosStack.Reflection.ObjectVisitors.Metadata;
using CosmosStack.Validation;
using CosmosStack.Validation.Objects;

// ReSharper disable InconsistentNaming

namespace CosmosStack.Reflection.ObjectVisitors.Internals.PropertyNodes
{
    internal class PropertyNodeVisitor : IPropertyNodeVisitor, ICoreVisitor
    {
        private readonly ObjectMember _member;
        private readonly Dictionary<string, IPropertyNodeVisitor> _childrenNodes;
        private readonly ObjectVisitorOptions _options;
        private InstanceVisitor _visitor;
        private int _signature;

        //root

        public PropertyNodeVisitor(
            ObjectMember member,
            InstanceVisitor visitor,
            ObjectVisitorOptions options,
            Action<object> memberValueSyncHandler)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _member = member ?? throw new ArgumentNullException(nameof(member));
            Deep = 0;
            Root = this;
            Parent = null;
            _visitor = visitor ?? throw new ArgumentNullException(nameof(visitor));
            _signature = visitor.Signature;
            _rootSignatureCached = new() {_signature};
            _parentSignatureCached = default;
            _signatureCached = new() {_signature};
            _childrenNodes = new();
            MemberValueSyncHandler = memberValueSyncHandler ?? throw new ArgumentNullException(nameof(memberValueSyncHandler));
        }

        //child

        public PropertyNodeVisitor(
            ObjectMember member,
            PropertyNodeVisitor rootVisitor, PropertyNodeVisitor parentVisitor, InstanceVisitor visitor,
            ObjectVisitorOptions options,
            Action<object> memberValueSyncHandler,
            List<int> rootSignatureCacheRef, List<int> parentSignatureCacheRef,
            int deep)
        {
            _member = member ?? throw new ArgumentNullException(nameof(member));
            Deep = deep;
            Root = rootVisitor ?? throw new ArgumentNullException(nameof(rootVisitor));
            Parent = parentVisitor ?? throw new ArgumentNullException(nameof(parentVisitor));
            _visitor = visitor ?? throw new ArgumentNullException(nameof(visitor));
            _signature = visitor.Signature;
            _rootSignatureCached = rootSignatureCacheRef;
            _parentSignatureCached = parentSignatureCacheRef;
            _signatureCached = new() {_signature};
            _childrenNodes = new();
            MemberValueSyncHandler = memberValueSyncHandler ?? throw new ArgumentNullException(nameof(memberValueSyncHandler));

            _rootSignatureCached.Add(_signature);
            _parentSignatureCached.Add(_signature);
        }

        public string Name => _member.MemberName;

        public PropertyNodeVisitor Root { get; }

        public PropertyNodeVisitor Parent { get; }

        private List<int> _rootSignatureCached { get; }

        private List<int> _parentSignatureCached { get; }

        private List<int> _signatureCached { get; }

        public int Deep { get; }

        public int Signature => _signature;

        public bool Repeatable => _options.Repeatable;

        public bool StrictMode  => _options.StrictMode;

        #region Init Resolve

        /*
         * 初始化解析
         * 当创建本 NodeVisitor 时，不执行初始化解析
         * 当且仅当第一次使用 NodeVisitor 时，才会初始化本节点的 InstanceVisitor
         */

        private bool _hasBeenInitialized = false;
        private readonly object _initLockObj = new();

        private void Init()
        {
            if (!_hasBeenInitialized)
            {
                lock (_initLockObj)
                {
                    if (!_hasBeenInitialized)
                    {
                        foreach (var memberName in _visitor.GetMemberNames())
                        {
                            var member = _visitor.GetMember(memberName);

                            if (member.MemberType is null)
                                continue;

                            if (member.MemberType.IsBasicType())
                                _childrenNodes[memberName] = null;

                            else
                                _childrenNodes[memberName] = PropertyNodeFactory.Create(
                                    member,
                                    _visitor.ExposeObjectCaller(), 
                                    _options,
                                    Root,
                                    this,
                                    _rootSignatureCached,
                                    _signatureCached,
                                    Deep
                                );
                        }

                        _hasBeenInitialized = true;
                    }
                }
            }
        }

        #endregion

        #region Implementations for ICoreVisitor

        public Type SourceType => _member.MemberType;

        public bool IsStatic => false;

        public AlgorithmKind AlgorithmKind => _options.AlgorithmKind;

        public object Instance => _visitor.Instance;

        public HistoricalContext ExposeHistoricalContext() => _visitor.ExposeHistoricalContext();

        public Lazy<MemberHandler> ExposeLazyMemberHandler() => _visitor.ExposeLazyMemberHandler();

        public ObjectCallerBase ExposeObjectCaller() => _visitor.ExposeObjectCaller();

        public IObjectVisitor Owner => _visitor.Owner;

        public bool LiteMode  => _options.LiteMode;

        #endregion

        #region Validation

        public IValidationEntry VerifiableEntry => _visitor.VerifiableEntry;

        public VerifyResult Verify() => _visitor.Verify();

        public VerifyResult Verify(string globalVerifyProviderName) => _visitor.Verify(globalVerifyProviderName);

        public void VerifyAndThrow() => _visitor.VerifyAndThrow();

        public void VerifyAndThrow(string globalVerifyProviderName) => _visitor.VerifyAndThrow(globalVerifyProviderName);

        #endregion

        #region Value Getter/Setter

        public void SetValue(List<string> pathSegments, object value, int startIndex)
        {
            var path = GetPath(pathSegments, startIndex, out var isCurrentNode);
            if (Instance is null)
                ReNew();
            if (!isCurrentNode)
                MoveToNextNode(path)?.SetValue(pathSegments, value, startIndex + 1);
            else if (IsEnd())
                _visitor.SetValue(path, value);
            else
                Sync(value);
        }

        public void SetValue(List<string> pathSegments, object value, int startIndex, string globalVerifyProviderName)
        {
            var path = GetPath(pathSegments, startIndex, out var isCurrentNode);
            if (Instance is null)
                ReNew();
            if (!isCurrentNode)
                MoveToNextNode(path)?.SetValue(pathSegments, value, startIndex + 1);
            else if (IsEnd())
                _visitor.SetValue(path, value, globalVerifyProviderName);
            else
                Sync(value);
        }

        public object GetValue(List<string> pathSegments, int startIndex)
        {
            var path = GetPath(pathSegments, startIndex, out var isCurrentNode);
            if (Instance is null)
                return default;
            if (!isCurrentNode)
                return MoveToNextNode(path)?.GetValue(pathSegments, startIndex + 1);
            return _visitor.GetValue(path);
        }

        #endregion

        #region Members

        public IEnumerable<string> GetMemberNames() => _visitor.GetMemberNames();

        public ObjectMember GetMember(string memberName) => _visitor.GetMember(memberName);

        public bool Contains(string memberName) => _visitor.Contains(memberName);

        #endregion

        #region Nodes

        public bool HasChildren() => _childrenNodes.Any();

        public bool IsEnd() => !HasChildren();

        private string GetPath(List<string> pathSegments, int startIndex, out bool isCurrentNode)
        {
            if (startIndex <= 0 || startIndex >= pathSegments.Count)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            isCurrentNode = startIndex == pathSegments.Count - 1;
            return pathSegments[startIndex];
        }

        private IPropertyNodeVisitor MoveToNextNode(string path)
        {
            if (!_hasBeenInitialized)
                Init();
            return _childrenNodes.TryGetValue(path, out var visitor)
                ? visitor
                : default;
        }

        #endregion

        #region ReNew/Sync

        private Action<object> MemberValueSyncHandler { get; set; }

        public void ReNew()
        {
            var handler = _visitor.ExposeLazyMemberHandler().Value;

            var instance = handler.ReNew();

            Sync(instance);
        }

        public void Sync(object instance)
        {
            // Sync
            Sync(_visitor.ExposeObjectCaller(), instance);
        }

        public void Sync(ObjectCallerBase handler, object instance)
        {
            // Sync
            // - Step 1: Sync to parent data holder
            // - Step 2: Sync to current node
            MemberValueSyncHandler.Invoke(instance);
            _visitor = ObjectVisitorFactoryCore.CreateForInstance(_member.MemberType, instance, _options);

            _childrenNodes.Clear();

            _rootSignatureCached.RemoveIf(x => _signatureCached.Contains(x));
            _parentSignatureCached?.RemoveIf(x => _signatureCached.Contains(x));
            _signatureCached.Clear();

            if (instance is null)
            {
                _signature = 0;
            }
            else
            {
                _signature = instance.GetHashCode();
                _rootSignatureCached.Add(_signature);
                _parentSignatureCached?.Add(_signature);
                _signatureCached.Add(_signature);
            }

            _hasBeenInitialized = false;
        }

        #endregion

        #region ValueAccessor

        public IPropertyValueAccessor ToValueAccessor() => _visitor.ToValueAccessor();

        #endregion
    }
}