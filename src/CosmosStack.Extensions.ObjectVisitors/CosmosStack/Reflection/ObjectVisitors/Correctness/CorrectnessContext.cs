using System;
using System.Collections.Generic;
using System.Reflection;
using CosmosStack.Reflection.ObjectVisitors.Internals;
using CosmosStack.Validation;
using CosmosStack.Validation.Strategies;

namespace CosmosStack.Reflection.ObjectVisitors.Correctness
{
    internal class CorrectnessContext : IValidationEntry
    {
        private readonly ICoreVisitor _visitor;

        public CorrectnessContext(ICoreVisitor visitor, bool strictMode)
        {
            _visitor = visitor ?? throw new ArgumentNullException(nameof(visitor));
            Options = CorrectnessOptions.Copy();
            StrictMode = strictMode;
            CorrectRuleChain = new();
        }

        public bool StrictMode { get; set; }

        public ValidationOptions Options { get; }

        #region CorrectRuleChain

        private RegisterRuleChain CorrectRuleChain { get; set; }

        internal RegisterRuleChain ExposeCorrectRuleChain() => CorrectRuleChain;

        #endregion

        #region Register

        public IValidationEntry SetStrategy<TStrategy>(StrategyMode mode = StrategyMode.OverallOverwrite) where TStrategy : class, IValidationStrategy, new()
        {
            CorrectRuleChain.RegisterStrategy<TStrategy>(mode);
            _needToBuild = true;
            return this;
        }

        public IValidationEntry SetStrategy<TStrategy>(TStrategy strategy, StrategyMode mode = StrategyMode.OverallOverwrite) where TStrategy : class, IValidationStrategy, new()
        {
            if (strategy is null)
                throw new ArgumentNullException(nameof(strategy));
            CorrectRuleChain.RegisterStrategy(strategy, mode);
            _needToBuild = true;
            return this;
        }

        public IValidationEntry SetRulePackage(VerifyRulePackage package, VerifyRuleMode mode = VerifyRuleMode.Append)
        {
            if (package is null)
                throw new ArgumentNullException(nameof(package));
            CorrectRuleChain.RegisterRulePackage(_visitor.SourceType, package, mode);
            _needToBuild = true;
            return this;
        }

        public IValidationEntry SetMemberRulePackage(string name, VerifyMemberRulePackage package, VerifyRuleMode mode = VerifyRuleMode.Append)
        {
            if (package is null)
                throw new ArgumentNullException(nameof(package));
            CorrectRuleChain.RegisterMemberRulePackage(_visitor.SourceType, name, package, mode);
            _needToBuild = true;
            return this;
        }

        public IValidationEntry ForMember(string name, Func<IValueRuleBuilder, IValueRuleBuilder> func)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));
            CorrectRuleChain.RegisterMember(_visitor.SourceType, name, func);
            _needToBuild = true;
            return this;
        }

        public IValidationEntry ForMember(PropertyInfo propertyInfo, Func<IValueRuleBuilder, IValueRuleBuilder> func)
        {
            if (propertyInfo is null)
                throw new ArgumentNullException(nameof(propertyInfo));
            CorrectRuleChain.RegisterMember(_visitor.SourceType, propertyInfo, func);
            _needToBuild = true;
            return this;
        }

        public IValidationEntry ForMember(FieldInfo fieldInfo, Func<IValueRuleBuilder, IValueRuleBuilder> func)
        {
            if (fieldInfo is null)
                throw new ArgumentNullException(nameof(fieldInfo));
            CorrectRuleChain.RegisterMember(_visitor.SourceType, fieldInfo, func);
            _needToBuild = true;
            return this;
        }

        #endregion

        #region Handler

        private bool _hasBeenBuilt;
        private bool _needToBuild;

        private ValidationHandler _handler;

        protected ValidationHandler ValidationHandler
        {
            get
            {
                if (_needToBuild || !_hasBeenBuilt)
                {
                    _handler = CorrectRuleChain.Build(Options);
                    _hasBeenBuilt = true;
                    _needToBuild = false;
                }

                return _handler;
            }
        }

        #endregion

        #region VerifyRulePackage

        public VerifyRulePackage ExposeRulePackage()
        {
            if (ValidationHandler is null)
                return VerifyRulePackage.Empty;
            return ValidationHandler.ExposeRulePackage(_visitor.SourceType);
        }

        public VerifyMemberRulePackage ExposeMemberRulePackage(string memberName)
        {
            if (ValidationHandler is null)
                return VerifyMemberRulePackage.Empty;
            return ValidationHandler.ExposeMemberRulePackage(_visitor.SourceType, memberName);
        }

        #endregion

        #region Verify

        public virtual VerifyResult Verify(bool withGlobalRules, string withGlobalProviderName = "")
        {
            if (ValidationHandler is null)
                return VerifyResult.Success;
            var master = ValidationHandler.Verify(_visitor.SourceType, _visitor.Instance);
            var slave = withGlobalRules
                ? ValidationMe.Resolve(_visitor.SourceType, withGlobalProviderName).Verify(_visitor.SourceType, _visitor.Instance)
                : VerifyResult.Success;
            return VerifyResult.Merge(master, slave);
        }

        public virtual VerifyResult VerifyOne(string memberName, bool withGlobalRules, string withGlobalProviderName = "")
        {
            if (ValidationHandler is null)
                return VerifyResult.Success;
            var value = _visitor.ExposeLazyMemberHandler().Value.GetValueObject(memberName);
            var master = ValidationHandler.VerifyOne(_visitor.SourceType, value, memberName);
            var slave = withGlobalRules
                ? ValidationMe.Resolve(_visitor.SourceType, withGlobalProviderName).Verify(_visitor.SourceType, _visitor.Instance)
                : VerifyResult.Success;
            return VerifyResult.Merge(master, slave);
        }

        public virtual VerifyResult VerifyOne(string memberName, object value, bool withGlobalRules, string withGlobalProviderName = "")
        {
            if (ValidationHandler is null)
                return VerifyResult.Success;
            var master = ValidationHandler.VerifyOne(_visitor.SourceType, value, memberName);
            var slave = withGlobalRules
                ? ValidationMe.Resolve(_visitor.SourceType, withGlobalProviderName).VerifyOne(_visitor.SourceType, value, memberName)
                : VerifyResult.Success;
            return VerifyResult.Merge(master, slave);
        }

        public virtual VerifyResult VerifyMany(IDictionary<string, object> keyValueCollections, bool withGlobalRules, string withGlobalProviderName = "")
        {
            if (ValidationHandler is null)
                return VerifyResult.Success;
            var master = ValidationHandler.VerifyMany(_visitor.SourceType, keyValueCollections);
            var slave = withGlobalRules
                ? ValidationMe.Resolve(_visitor.SourceType, withGlobalProviderName).VerifyMany(_visitor.SourceType, keyValueCollections)
                : VerifyResult.Success;
            return VerifyResult.Merge(master, slave);
        }

        #endregion
    }
}